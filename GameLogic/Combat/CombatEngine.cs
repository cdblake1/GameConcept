using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using GameData;
using GameData.src.Class;
using GameData.src.Effect;
using GameData.src.Effect.Stack;
using GameData.src.Effect.Status;
using GameData.src.Effect.Talent;
using GameData.src.Mob;
using GameData.src.Player;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill;
using GameData.src.Skill.SkillStep;
using GameData.src.Stat;
using GameData.src.Talent.TalentActions;
using GameLogic.Ports;
using static GameData.src.Skill.SkillStep.DotDamageStep;

namespace GameLogic.Combat
{
    /** 
        PreProcess();
            load characters into combat entities
            process talent modifiers onto the character

        PreCombat();
            process active effects
        
        ProcessTurns();
            apply damage

        PostCombat();
            process active effects/dtos at end_turn timing
        
        CheckStatus();
            After Each Turn and Post Combat, check the state of combat

            Status Applied
            Damage Done
            Player Won
            Player Lost
            Player Fled
            Mob Died
    */
    public class CombatEngine
    {
        internal readonly ISkillRepository skillRepository;
        internal readonly IEffectRepository effectRepository;
        internal readonly ITalentRepository talentRepository;

        private readonly CombatEntity player;
        private readonly CombatEntity mob;

        public int playerCurrentHealth;

        public bool IsCombatActive => combatActive;

        private bool combatActive = false;
        private bool intialized = false;
        private readonly PriorityQueue<CombatEntity, int> turnQueue = new();

        public CombatEngine(ISkillRepository skillRepository,
            IEffectRepository effectRepository,
            ITalentRepository talentRepository,
            Player player,
            Mob mob)
        {
            this.skillRepository = skillRepository;
            this.effectRepository = effectRepository;
            this.talentRepository = talentRepository;
            this.player = this.InitializePlayer(player);
            this.mob = this.InitializeMob(mob);
        }

        public TurnResult ProcessCombat(CombatCommand command)
        {
            var events = new List<CombatEvent>();

            if (!intialized)
            {
                this.combatActive = true;
                this.intialized = true;
                events.Add(new CombatStartEvent());
            }

            if (!this.combatActive)
            {
                events.Add(new CombatEndEvent());
                return new TurnResult(events);
            }

            this.turnQueue.Enqueue(player, player.BaseStats.Speed);
            this.turnQueue.Enqueue(mob, mob.BaseStats.Speed);

            do
            {
                var entity = this.turnQueue.Dequeue();

                if (entity.Identifier == player.Identifier)
                {
                    switch (command)
                    {
                        case UseSkillCommand s:
                            {
                                if (!entity.Skills.TryGetValue(s.SkillId, out var skill))
                                {
                                    throw new InvalidOperationException($"Skill with ID '{s.SkillId}' not found.");
                                }

                                if (mob.Identifier != s.TargetId)
                                {
                                    throw new InvalidOperationException("identifier does not match any combat entity");
                                }

                                foreach (var combatEvent in ApplyDamage(SelectSkill(entity), entity, player, events))
                                {
                                    events.Add(combatEvent);
                                }
                            }

                            break;
                        case FleeCommand:
                            {
                                events.Add(new PlayerFledEvent(true));
                                combatActive = false;
                            }
                            break;
                    }
                }
                else
                {
                    foreach (var combatEvent in ApplyDamage(SelectSkill(entity), entity, player, events))
                    {
                        events.Add(combatEvent);
                    }
                }

                foreach (var combatEvent in this.CalculateCombatState(events))
                {
                    events.Add(combatEvent);
                }

            } while (this.turnQueue.Count > 0 && combatActive);

            return new TurnResult(events);
        }

        public List<CombatEvent> PreCombatTurn(CombatEntity entity, List<CombatEvent> events)
        {
            return events;
        }

        public List<CombatEvent> PostCombatTurn(CombatEntity entity, List<CombatEvent> events)
        {
            return events;
        }

        public static List<CombatEvent> ApplyDamage(SkillCombatDefinition skill,
            CombatEntity source,
            CombatEntity target,
            List<CombatEvent> events)
        {
            var hitDamage = skill.Skill.Effects.OfType<HitDamageStep>();
            foreach (var hit in hitDamage)
            {
                var snapshot = new DamageSnapshot(hit);

                snapshot.Damage += snapshot.ScaleCoefficient.Stat switch
                {
                    StatKind.PhysicalDamage => snapshot.ScaleCoefficient.Operation.ApplyScalarModifier((int)source.BaseStats.AttackPower),
                    StatKind.MeleeDamage => snapshot.ScaleCoefficient.Operation.ApplyScalarModifier((int)source.BaseStats.AttackPower),
                    _ => 0
                };

                foreach (var damageType in snapshot.DamageTypes)
                {
                    snapshot.Damage += damageType switch
                    {
                        DamageType.Melee => (int)source.BaseStats.AttackPower,
                        DamageType.Physical => (int)source.BaseStats.AttackPower,
                        DamageType.Bleed => (int)source.BaseStats.AttackPower,
                        _ => 0
                    };
                }

                if (snapshot.StackFromEffect is not null)
                {
                    var effects = source.ActiveEffects.Where(ae => ae.Identifier == snapshot.StackFromEffect.FromEffect).ToList();
                    if (effects.Count > 0)
                    {
                        snapshot.Damage *= effects.Count;
                    }
                }

                foreach (var action in skill.HitActions)
                {
                    if (action.Skill != skill.Skill.Id)
                    {
                        continue;
                    }

                    if (action.BaseDamage is not null)
                    {
                        snapshot.Damage = action.BaseDamage.ApplyScalarModifier(snapshot.Damage);
                    }

                    snapshot.CanCrit = action.Crit ?? snapshot.CanCrit;

                    if (action.DamageTypes is not null)
                    {
                        snapshot.DamageTypes = action.DamageTypes.Operation.ApplyCollectionModifier(snapshot.DamageTypes);
                    }
                }

                events.Add(new DamageApplied(source.Identifier, target.Identifier, skill.Skill.Id, hit.BaseDamage));
            }

            // var dotDamage = skill.Skill.Effects.OfType<DotDamageStep>();
            // foreach (var dot in dotDamage)
            // {
            //     var snapshot = CombatDotDamageSnapshot.FromStep(dot);
            //     foreach (var action in skill.DotActions)
            //     {
            //         if (action.SkillId != skill.Skill.Id)
            //         {
            //             continue;
            //         }

            //         if (action.BaseDamage is not null)
            //         {
            //             snapshot.BaseDamage = action.BaseDamage.ApplyScalarModifier(snapshot.BaseDamage);
            //         }

            //         if (action.Frequency is not null)
            //         {
            //             snapshot.Frequency = action.Frequency.ApplyScalarModifier(snapshot.Frequency);
            //         }

            //         if (action.Duration is not null)
            //         {
            //             snapshot.Duration = action.Duration.ApplyDurationModifier(snapshot.Duration);
            //         }

            //         if (action.StackStrategy is not null)
            //         {
            //             snapshot.Stacking = action.StackStrategy;
            //         }

            //         if (action.Timing is TimingKind t)
            //         {
            //             snapshot.Timing = t;
            //         }

            //         if (action.DamageTypes is not null)
            //         {
            //             snapshot.DamageTypes = action.DamageTypes.Operation.ApplyCollectionModifier(snapshot.DamageTypes);
            //         }

            //         snapshot.Crit = action.Crit ?? snapshot.Crit;

            //     }

            //     events.Add(new DamageApplied(source.Identifier, target.Identifier, skill.Skill.Id, dot.BaseDamage));
            // }

            return events;
        }

        public static SkillCombatDefinition SelectSkill(CombatEntity entity)
        {
            var random = new Random();
            var skillIndex = random.Next(entity.Skills.Count);
            return entity.Skills.Values.ElementAt(skillIndex);
        }

        public List<CombatEvent> CalculateCombatState(List<CombatEvent> events)
        {
            if (!combatActive)
            {
                return events;
            }

            if (player.CurrentHealth <= 0)
            {
                events.Add(new PlayerDiedEvent());
                events.Add(new CombatEndEvent());

                this.combatActive = false;
            }

            if (mob.CurrentHealth <= 0)
            {
                events.Add(new MobDiedEvent(mob.Identifier));
                this.combatActive = false;
            }

            return events;
        }

        private CombatEntity InitializePlayer(Player player)
        {
            var activeSkills = new Dictionary<string, SkillCombatDefinition>();
            foreach (var entry in player.ClassDefinition.SkillEntry)
            {
                if (entry.Level <= player.Level)
                {
                    activeSkills[entry.Id] = new(this.skillRepository.Get(entry.Id));
                }
            }

            var combatEntity = new CombatEntity(nameof(player), player.BaseStats, activeSkills, player.Level);

            var actions = player.Talents.Select(t => t.Actions);

            foreach (var action in actions)
            {
                if (action is ApplyEffectAction ae)
                {
                    if (combatEntity.Skills.TryGetValue(ae.FromSkill, out var skill))
                    {
                        skill.Effects[ae.Effect] = this.effectRepository.Get(ae.Effect);
                    }
                }
                else if (action is ModifyHitDamageAction hd)
                {
                    if (combatEntity.Skills.TryGetValue(hd.Skill, out var skill))
                    {
                        skill.HitActions.Add(hd);
                    }
                }
                else if (action is ModifyDotDamageAction dd)
                {
                    if (combatEntity.Skills.TryGetValue(dd.SkillId, out var skill))
                    {
                        skill.DotActions.Add(dd);
                    }
                }
            }

            return combatEntity;
        }

        private CombatEntity InitializeMob(Mob mob)
        {
            var skills = new Dictionary<string, SkillCombatDefinition>();
            foreach (var skillId in mob.Skills)
            {
                skills[skillId] = new(this.skillRepository.Get(skillId));
            }

            return new CombatEntity(nameof(mob), mob.BaseStats, skills, mob.Level);
        }

        public class CombatEntity
        {
            public string Identifier { get; }
            private int currentHealth;
            public int MaxHealth { get; }
            public int CurrentHealth
            {
                get => currentHealth;
                set => currentHealth = Math.Max(0, value);
            }

            public List<EffectSnapshot> ActiveEffects = [];

            public Dictionary<string, SkillCombatDefinition> Skills { get; }
            public StatTemplate BaseStats { get; }
            int Level { get; }

            public CombatEntity(string identifier,
                StatTemplate baseStats,
                Dictionary<string, SkillCombatDefinition> skills,
                int level)
            {
                this.BaseStats = baseStats;
                this.Skills = skills;
                this.Level = level;
                this.Identifier = identifier;
            }
        }

        public class SkillCombatDefinition
        {
            public List<ModifySkillAction> skillActions { get; } = [];
            public List<ModifyEffectAction> activeEffectModifiers { get; } = [];
            public Dictionary<string, EffectDefinition> Effects { get; } = [];
            public List<ModifyDotDamageAction> DotActions { get; } = [];
            public List<ModifyHitDamageAction> HitActions { get; } = [];
            public SkillDefinition Skill { get; }

            public SkillCombatDefinition(SkillDefinition skill)
            {
                Skill = skill;
            }
        }

        public struct DamageSnapshot(HitDamageStep d)
        {
            public AttackKind Kind { get; set; } = d.Kind;

            public List<DamageType> DamageTypes { get; set; } = [.. d.DamageTypes];

            public int Damage { get; set; } = d.BaseDamage;

            public ScaleCoefficient ScaleCoefficient { get; set; } = d.ScaleCoefficient;

            public bool CanCrit { get; set; } = d.Crit;

            public StackFromEffect? StackFromEffect { get; set; } = d.StackFromEffect;
        }

        public struct CombatDotDamageSnapshot(DotDamageStep damage)
        {
            public AttackKind Kind { get; set; } = damage.Kind;

            public List<DamageType> DamageTypes { get; set; } = [.. damage.DamageTypes];

            public int BaseDamage { get; set; } = damage.BaseDamage;

            public ScaleCoefficient ScaleCoefficient { get; set; } = damage.ScaleCoefficient;

            public Duration Duration { get; set; } = damage.Duration;

            public int Frequency { get; set; } = damage.Frequency;

            public TimingKind Timing { get; set; } = damage.Timing;

            public IStackStrategy Stacking { get; set; } = damage.Stacking;

            public bool Crit { get; set; } = damage.Crit;

            public static CombatDotDamageSnapshot operator +(CombatDotDamageSnapshot a, CombatDotDamageSnapshot b)
            {
                return new CombatDotDamageSnapshot
                {
                    Kind = a.Kind,
                    DamageTypes = [.. a.DamageTypes, .. b.DamageTypes],
                    BaseDamage = a.BaseDamage + b.BaseDamage,
                    ScaleCoefficient = b.ScaleCoefficient,
                    Duration = b.Duration,
                    Frequency = a.Frequency + b.Frequency,
                    Stacking = b.Stacking,
                    Timing = b.Timing
                };
            }

            public static CombatDotDamageSnapshot FromStep(DotDamageStep step)
            {
                return new CombatDotDamageSnapshot()
                {
                    BaseDamage = step.BaseDamage,
                    DamageTypes = [.. step.DamageTypes],
                    Duration = step.Duration,
                    Frequency = step.Frequency,
                    Kind = AttackKind.Dot,
                    ScaleCoefficient = step.ScaleCoefficient,
                    Stacking = step.Stacking,
                    Timing = step.Timing
                };
            }
        }

        // public struct CombatHitDamageSnapshot(HitDamageStep damage)
        // {
        //     public AttackKind Kind { get; set; } = damage.Kind;

        //     public List<DamageType> DamageTypes { get; set; } = [.. damage.DamageTypes];

        //     public required int BaseDamage { get; set; } = damage.BaseDamage;

        //     public required bool Crit { get; set; } = damage.Crit;

        //     public required ScaleCoefficient ScaleCoefficient { get; set; } = damage.ScaleCoefficient;

        //     public StackFromEffect? StackFromEffect { get; set; } = damage.StackFromEffect;

        //     public static CombatHitDamageSnapshot operator +(CombatHitDamageSnapshot a, CombatHitDamageSnapshot b)
        //     {
        //         return new CombatHitDamageSnapshot
        //         {
        //             Kind = a.Kind,
        //             DamageTypes = a.DamageTypes.Concat(b.DamageTypes).ToList(),
        //             BaseDamage = a.BaseDamage + b.BaseDamage,
        //             Crit = a.Crit || b.Crit,
        //             ScaleCoefficient = b.ScaleCoefficient,
        //             StackFromEffect = a.StackFromEffect ?? b.StackFromEffect
        //         };
        //     }

        //     public static CombatHitDamageSnapshot FromStep(HitDamageStep hit)
        //     {
        //         return new CombatHitDamageSnapshot()
        //         {
        //             BaseDamage = hit.BaseDamage,
        //             Crit = hit.Crit,
        //             ScaleCoefficient = hit.ScaleCoefficient,
        //             DamageTypes = [.. hit.DamageTypes],
        //             Kind = hit.Kind,
        //             StackFromEffect = hit.StackFromEffect
        //         };
        //     }
        // }

        public struct EffectSnapshot(EffectDefinition effect)
        {
            public readonly string Identifier => effect.Id;
            public EffectDefinition.Kind Category { get; set; } = effect.Category;

            public Duration Duration { get; set; } = effect.Duration;

            public IStackStrategy Stacking { get; set; } = effect.Stacking;

            public List<ScalarModifierBase> Modifiers { get; init; } = [.. effect.Modifiers];

            public int Leech { get; init; } = effect.Leech;

            public List<DamageType> DamageTypes { get; init; } = [.. effect.DamageTypes];

            public List<IStatus> ApplyStatus { get; init; } = [.. effect.ApplyStatus];
        }

        public abstract record CombatEvent;
        public record CombatStartEvent : CombatEvent;
        public record CombatEndEvent : CombatEvent;
        public record PlayerDiedEvent : CombatEvent;
        public record PlayerFledEvent(bool Fled) : CombatEvent;
        public record MobDiedEvent(string TargetId) : CombatEvent;
        public record DamageApplied(string SourceId, string TargetId, string SkillId, int Amount) : CombatEvent;
        public record EffectApplied(string SourceId, string TargetId, string EffectId) : CombatEvent;
        public record StatusApplied(IStatus Status, string SourceId) : CombatEvent;

        public readonly struct TurnResult(IReadOnlyList<CombatEvent> events)
        {
            public IReadOnlyList<CombatEvent> Events => events;
        }

        public abstract record CombatCommand;
        public record UseSkillCommand(string SkillId, string TargetId) : CombatCommand;
        public record FleeCommand : CombatCommand;

    }
}