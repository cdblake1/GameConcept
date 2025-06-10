using GameData.src.Effect.Status;
using GameData.src.Shared;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill.SkillStep;
using GameData.src.Talent.TalentActions;
using GameLogic.Combat.Snapshots;
using GameLogic.Combat.Snapshots.Steps;
using GameLogic.Player;
using GameLogic.Ports;

namespace GameLogic.Combat
{
    public class CombatEngine
    {
        private static readonly CombatStartEvent combatStartEvent = new();
        private static readonly CombatEndEvent combatEndEvent = new();
        private static readonly FleeEvent combatFleeCommand = new(true);
        private static readonly PlayerDiedEvent playerDiedEvent = new();

        internal readonly ISkillRepository skillRepository;
        internal readonly IEffectRepository effectRepository;
        internal readonly ITalentRepository talentRepository;

        private readonly CombatEntity player;
        private readonly CombatEntity mob;

        public bool IsCombatActive => this.combatActive;

        private bool combatActive = false;
        private bool intialized = false;
        private readonly PriorityQueue<CombatEntity, float> turnQueue = new();

        public CombatEngine(ISkillRepository skillRepository,
            IEffectRepository effectRepository,
            ITalentRepository talentRepository,
            PlayerInstance player,
            MobInstance mob)
        {
            this.skillRepository = skillRepository;
            this.effectRepository = effectRepository;
            this.talentRepository = talentRepository;
            this.player = this.InitializePlayer(player);
            this.mob = this.InitializeMob(mob);
        }

        public List<CombatEvent> ProcessCombat(CombatCommand command)
        {
            var events = new List<CombatEvent>();

            if (!this.intialized)
            {
                this.combatActive = true;
                this.intialized = true;
                events.Add(combatStartEvent);
            }

            if (!this.combatActive)
            {
                events.Add(combatEndEvent);
                return events;
            }

            ProccessEntityStatusBuffer(this.player, events);
            if (IsEntityDead(this.player))
            {
                events.Add(playerDiedEvent);
                events.Add(combatEndEvent);
                return events;
            }

            ProccessEntityStatusBuffer(this.mob, events);
            if (IsEntityDead(this.mob))
            {
                events.Add(new MobDiedEvent(this.mob.Identifier));
                events.Add(combatEndEvent);
                return events;
            }

            do
            {
                var entity = this.turnQueue.Dequeue();

                if (entity.IsPlayer)
                {
                    switch (command)
                    {
                        case UseSkillCommand cmd:

                            this.ProcessAttack(FindSkill(cmd.SkillId, this.player), this.player, this.mob, events);

                            break;
                        case CombatFleeCommand cmd:
                            events.Add(combatFleeCommand);
                            break;
                    }
                }
                else
                {
                    var skill = entity.Skills.Span[Random.Shared.Next(entity.Skills.Length)];
                    this.ProcessAttack(skill, this.mob, this.player, events);
                }

            } while (this.IsCombatActive && this.turnQueue.Count > 0);

            return events;
        }

        internal static void ProccessEntityStatusBuffer(CombatEntity source, in List<CombatEvent> events)
        {
            //TODO(Caleb): decrement all statuses. Remove status from buffer if at duration.
            var effects = source.ActiveEffects.GetEffects();
            foreach (var effect in effects)
            {
                if (effect.Damage is DamageSnapshot dmg)
                {
                    (bool isCrit, float damage) = CalculateDamage(dmg, source);
                    source.CurrentHealth = Math.Min(0, (int)(source.CurrentHealth - damage));
                    events.Add(new DamageApplied(source.Identifier, source.Identifier, dmg.SkillDefinition.Id, damage, isCrit));
                }
            }
        }

        internal void ProcessAttack(SkillSnapshot skill, CombatEntity source, CombatEntity target, in List<CombatEvent> events)
        {
            foreach (var effect in skill.DamageSteps.Span)
            {
                (bool isCrit, float damage) = CalculateDamage(skill, effect, source, target);

                target.CurrentHealth = Math.Min(0, (int)(target.CurrentHealth - damage));
                events.Add(new DamageApplied("mob", "player", skill.skillDefinition.Id, damage, isCrit));

                if (IsEntityDead(target))
                {
                    events.Add(new MobDiedEvent(target.Identifier));
                    events.Add(combatEndEvent);
                    return;
                }
            }

            foreach (var effect in skill.DotEffects.Span)
            {
                (bool isCrit, float damage) = CalculateDamage(skill, effect, source, target);
                target.CurrentHealth = Math.Min(0, (int)(target.CurrentHealth - damage));
                events.Add(new DamageApplied("mob", "player", skill.skillDefinition.Id, damage, isCrit));

                if (IsEntityDead(target))
                {
                    events.Add(new MobDiedEvent(target.Identifier));
                    events.Add(combatEndEvent);
                    return;
                }
            }

            foreach (var effect in skill.ApplyEffects.Span)
            {
                var effectSnapshot = EffectSnapshot.FromEffect(this.effectRepository.Get(effect.EffectId));
                target.ActiveEffects.AddEffect(effectSnapshot);
                events.Add(new EffectApplied(source.Identifier, target.Identifier, effect.EffectId));

                if (IsEntityDead(target))
                {
                    events.Add(new MobDiedEvent(target.Identifier));
                    events.Add(combatEndEvent);
                    return;
                }
            }
        }

        internal static bool IsEntityDead(CombatEntity entity)
        {
            return entity.CurrentHealth <= 0;
        }

        internal static SkillSnapshot FindSkill(string skillId, CombatEntity entity)
        {
            foreach (var skill in entity.Skills.Span)
            {
                if (skill.skillDefinition.Id == skillId)
                {
                    return skill;
                }
            }

            return default;
        }

        internal CombatEntity InitializePlayer(PlayerInstance player)
        {
            var combatEntity = new CombatEntity(nameof(player),
                player.Stats,
                this.CreateSkillSnapshots(player.GetSkills()),
                true);

            var talents = player.GetTalents();
            for (int i = 0; i < talents.Length; i++)
            {
                var talent = this.talentRepository.Get(talents[i]);
                for (int j = 0; j < talent.Actions.Count; j++)
                {
                    switch (talent.Actions[j])
                    {
                        case AddDotDamageAction action:
                            for (int k = 0; k < combatEntity.Skills.Span.Length; k++)
                            {
                                var skill = combatEntity.Skills.Span[k];
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    skill.DotEffects.Span[skill.DotEffects.Length]
                                        = DotDamageStepSnapshot.FromStep(action.DotDamage);

                                    break;
                                }
                            }
                            break;
                        case AddHitDamageAction action:
                            for (int k = 0; k < combatEntity.Skills.Span.Length; k++)
                            {
                                var skill = combatEntity.Skills.Span[k];
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    skill.DamageSteps.Span[skill.DamageSteps.Length] = DamageStepSnapshot.FromStep(action.HitDamage);
                                    break;
                                }
                            }
                            break;
                        case ApplyEffectAction action:
                            for (int k = 0; k < combatEntity.Skills.Span.Length; k++)
                            {
                                var skill = combatEntity.Skills.Span[k];
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    skill.ApplyEffects.Span[skill.ApplyEffects.Length] = new ApplyEffectSnapshot(action.EffectId);
                                    break;
                                }
                            }
                            break;
                        case ModifyDotDamageAction action:
                            foreach (var skill in combatEntity.Skills.Span)
                            {
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    for (int k = 0; k < skill.DotEffects.Length; k++)
                                    {
                                        var effect = skill.DotEffects.Span[k];
                                        if (action.Duration is not null)
                                        {
                                            if (action.Duration.Kind == DurationKind.Permanent)
                                            {
                                                effect.Duration = DurationSnapshot.FromDuration(Duration.Permanent());
                                            }
                                            else if (action.Duration.Kind == DurationKind.ExpiresWith
                                                && action.Duration.ExpiresWith is Duration.ExpiresWith ew)
                                            {
                                                effect.Duration = DurationSnapshot
                                                    .FromDuration(Duration.FromExpiry(ew));
                                            }
                                            else if (action.Duration.Kind == DurationKind.Turns
                                                && action.Duration.Turns is not null)
                                            {
                                                effect.Duration = DurationSnapshot.FromDuration(
                                                    Duration.FromTurns(AddScalar(effect.Duration.Turns, action.Duration.Turns)));
                                            }
                                        }

                                        effect.Frequency = AddScalar(effect.Frequency, action.Frequency);
                                        effect.MinBaseDamage = AddScalar(effect.MinBaseDamage, action.MinBaseDamage);
                                        effect.MaxBaseDamage = AddScalar(effect.MaxBaseDamage, action.MaxBaseDamage);

                                        if (action.StackStrategy is not null && effect.StackStrategy.StackType == StackType.Default)
                                        {
                                            effect.StackStrategy.StacksPerApplication = AddScalar(effect.StackStrategy.StacksPerApplication, action.StackStrategy.StacksPerApplication);
                                            effect.StackStrategy.MaxStacks = AddScalar(effect.StackStrategy.MaxStacks, action.StackStrategy.Maxstacks);
                                        }
                                    }
                                }
                            }
                            break;
                        case ModifyHitDamageAction action:
                            foreach (var skill in combatEntity.Skills.Span)
                            {
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    for (int k = 0; k < skill.DamageSteps.Length; k++)
                                    {
                                        var effect = skill.DamageSteps.Span[k];
                                        effect.MinBaseDamage = AddScalar(effect.MinBaseDamage, action.MinBaseDamage);
                                        effect.MaxBaseDamage = AddScalar(effect.MaxBaseDamage, action.MaxBaseDamage);
                                    }
                                }
                            }
                            break;
                        case ModifyEffectAction action:
                            break;
                        case ModifySkillAction action:
                            for (int k = 0; k < combatEntity.Skills.Span.Length; k++)
                            {
                                var skill = combatEntity.Skills.Span[k];
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    skill.Cooldown = AddScalar(skill.Cooldown, action.Cooldown);
                                    skill.Cost = AddScalar(skill.Cost, action.Cost);
                                    skill.ActivationRequirement = action.ActivationRequirement ?? skill.ActivationRequirement;
                                }
                            }
                            break;
                    }
                }
            }

            return combatEntity;

            static int AddScalar(int value, ScalarOperation? operation)
            {
                if (operation is null)
                {
                    return value;
                }

                var additive = operation.ScaleAdded;
                var increased = operation.ScaleIncreased;
                var empowered = operation.ScaleEmpowered;

                return (int)((additive + value) * (1 + (increased / 100)) * (1 + (empowered / 100)));
            }
        }

        internal CombatEntity InitializeMob(MobInstance mob)
        {
            var skills = this.CreateSkillSnapshots([.. mob.Skills]);

            return new CombatEntity(nameof(mob), mob.Stats, skills, false);
        }

        internal Memory<SkillSnapshot> CreateSkillSnapshots(string[] skillIds)
        {
            var skills = new Memory<SkillSnapshot>();
            for (int i = 0; i < skillIds.Length; i++)
            {
                Memory<ApplyEffectSnapshot>? applyEffects = null;
                Memory<DotDamageStepSnapshot>? dotEffects = null;
                Memory<DamageStepSnapshot>? dmgEffects = null;

                var skill = this.skillRepository.Get(skillIds[i]);

                for (int j = 0; j < skill.Effects.Count; j++)
                {
                    var effect = skill.Effects[j];
                    switch (effect)
                    {
                        case ApplyEffectStep step:
                            applyEffects ??= new();
                            applyEffects.Value.Span[j] = new(step.EffectId);
                            break;
                        case DotDamageStep step:
                            dotEffects ??= new();
                            dotEffects.Value.Span[j] = DotDamageStepSnapshot.FromStep(step);
                            break;
                        case HitDamageStep step:
                            dmgEffects ??= new();
                            dmgEffects.Value.Span[j] = DamageStepSnapshot.FromStep(step);
                            break;
                    }
                }

                skills.Span[i] = new SkillSnapshot(
                    skill,
                    skill.Cost,
                    skill.Cooldown,
                    applyEffects,
                    dmgEffects,
                    dotEffects
                );
            }

            return skills;
        }

        internal static (bool isCrit, float damage) CalculateDamage(SkillSnapshot skill, DamageStepSnapshot step, CombatEntity source, CombatEntity target)
        {
            var snapshot = DamageSnapshotBuilder.BuildNew(skill.skillDefinition, step);
            return CalculateDamage(snapshot, source, target);
        }

        internal static (bool isCrit, float damage) CalculateDamage(SkillSnapshot skill, DotDamageStepSnapshot step, CombatEntity source, CombatEntity target)
        {
            var snapshot = DamageSnapshotBuilder.BuildNew(skill.skillDefinition, step);
            return CalculateDamage(snapshot, source, target);
        }

        internal static (bool isCrit, float damage) CalculateDamage(DamageSnapshot snapshot, CombatEntity source, CombatEntity target)
        {
            snapshot.AddStats(source.Stats);
            snapshot.AddModifiers(GetModifiers(source.ActiveEffects.GetEffects()));

            snapshot.AddStats(target.Stats);
            snapshot.AddModifiers(GetModifiers(target.ActiveEffects.GetEffects()));

            return snapshot.CalculateDamage();
        }

        internal static (bool isCrit, float damage) CalculateDamage(DamageSnapshot snapshot, CombatEntity target)
        {
            snapshot.AddStats(target.Stats);
            snapshot.AddModifiers(GetModifiers(target.ActiveEffects.GetEffects()));

            return snapshot.CalculateDamage();
        }

        internal static Span<IModifier> GetModifiers(ReadOnlySpan<EffectSnapshot> effects)
        {
            Span<IModifier> modifiers = new();
            int modifierIndex = 0;
            for (int i = 0; i < effects.Length; i++)
            {
                var effect = effects[i];
                if (effect.Modifiers.HasValue)
                {
                    var effectModifiers = effect.Modifiers.Value.Span;
                    for (int j = 0; j < effectModifiers.Length; j++)
                    {
                        modifiers[modifierIndex++] = effectModifiers[j];
                    }
                }
            }

            return modifiers;
        }

        public abstract record CombatEvent;
        public record CombatStartEvent : CombatEvent;
        public record CombatEndEvent : CombatEvent;
        public record PlayerDiedEvent : CombatEvent;
        public record PlayerFledEvent(bool Fled) : CombatEvent;
        public record MobDiedEvent(string TargetId) : CombatEvent;
        public record FleeEvent(bool Success) : CombatEvent;
        public record DamageApplied(string SourceId, string TargetId, string SkillId, float Amount, bool IsCrit) : CombatEvent;
        public record DotDamageApplied(string SourceId, string TargetId, string SkillId, float Amount, bool IsCrit) : CombatEvent;
        public record EffectApplied(string SourceId, string TargetId, string EffectId) : CombatEvent;
        public record StatusApplied(IStatus Status, string SourceId) : CombatEvent;

        public abstract record CombatCommand;
        public record UseSkillCommand(string SkillId, string TargetId) : CombatCommand;
        public record CombatFleeCommand : CombatCommand;
    }
}