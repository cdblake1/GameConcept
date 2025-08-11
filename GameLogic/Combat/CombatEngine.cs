using GameData.src.Effect.Status;
using GameData.src.Shared;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill.SkillStep;
using GameData.src.Talent.TalentActions;
using GameLogic.Combat.Snapshots;
using GameLogic.Combat.Snapshots.Steps;
using GameLogic.Mob;
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

        internal readonly CombatEntity player;
        internal readonly CombatEntity mob;

        public bool IsCombatActive => this.combatActive;

        private bool combatActive = false;
        private bool intialized = false;
        private readonly PriorityQueue<CombatEntity, float> turnQueue = new();

        internal readonly List<CombatEvent> events = [];

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
            this.events.Clear();
            if (!this.intialized)
            {
                this.combatActive = true;
                this.intialized = true;
                this.events.Add(combatStartEvent);
            }

            if (!this.combatActive)
            {
                this.events.Add(combatEndEvent);
                return this.events;
            }

            this.ProccessEntityStatusBuffer(this.player);
            if (IsEntityDead(this.player))
            {
                this.events.Add(playerDiedEvent);
                this.events.Add(combatEndEvent);
                return this.events;
            }

            this.turnQueue.Enqueue(this.player, this.player.Stats.GetStatValue(GlobalStat.Speed));

            this.ProccessEntityStatusBuffer(this.mob);
            if (IsEntityDead(this.mob))
            {
                this.events.Add(new MobDiedEvent(this.mob.Identifier));
                this.events.Add(combatEndEvent);
                return this.events;
            }

            this.turnQueue.Enqueue(this.mob, this.mob.Stats.GetStatValue(GlobalStat.Speed));


            do
            {
                var entity = this.turnQueue.Dequeue();

                if (entity.IsPlayer)
                {
                    switch (command)
                    {
                        case UseSkillCommand cmd:

                            this.ProcessAttack(FindSkill(cmd.SkillId, this.player), this.player, this.mob);

                            break;
                        case CombatFleeCommand cmd:
                            this.events.Add(combatFleeCommand);
                            break;
                    }
                }
                else
                {
                    var skill = entity.Skills[Random.Shared.Next(entity.Skills.Count)];
                    this.ProcessAttack(skill, this.mob, this.player);
                }

            } while (this.IsCombatActive && this.turnQueue.Count > 0);

            return this.events;
        }

        internal void ProccessEntityStatusBuffer(CombatEntity source)
        {
            //TODO(Caleb): decrement all statuses. Remove status from buffer if at duration.
            var effects = source.ActiveEffects.GetEffects();
            foreach (var effect in effects)
            {
                if (effect.Damage is DamageSnapshot dmg)
                {
                    (bool isCrit, float damage) = CalculateDamage(dmg, source);
                    source.CurrentHealth = (int)(source.CurrentHealth - damage);
                    this.events.Add(new DamageAppliedEvent(source.Identifier, source.Identifier, dmg.SkillDefinition.Id, damage, isCrit));
                }
            }
        }

        internal void ProcessAttack(SkillSnapshot skill, CombatEntity source, CombatEntity target)
        {
            foreach (var effect in skill.DamageSteps)
            {
                (bool isCrit, float damage) = CalculateDamage(skill, effect, source, target);

                target.CurrentHealth = (int)(target.CurrentHealth - damage);
                this.events.Add(new DamageAppliedEvent(source.Identifier, target.Identifier, skill.skillDefinition.Id, damage, isCrit));

                if (IsEntityDead(target))
                {
                    this.events.Add(new MobDiedEvent(target.Identifier));
                    this.events.Add(combatEndEvent);
                    return;
                }
            }

            foreach (var effect in skill.DotEffects)
            {
                (bool isCrit, float damage) = CalculateDamage(skill, effect, source, target);
                target.CurrentHealth = (int)(target.CurrentHealth - damage);
                this.events.Add(new DamageAppliedEvent(source.Identifier, target.Identifier, skill.skillDefinition.Id, damage, isCrit));

                if (IsEntityDead(target))
                {
                    this.events.Add(new MobDiedEvent(target.Identifier));
                    this.events.Add(combatEndEvent);
                    return;
                }
            }

            foreach (var effect in skill.ApplyEffects)
            {
                var effectSnapshot = EffectSnapshot.FromEffect(this.effectRepository.Get(effect.EffectId));
                target.ActiveEffects.AddEffect(effectSnapshot);
                this.events.Add(new EffectApplied(source.Identifier, target.Identifier, effect.EffectId));

                if (IsEntityDead(target))
                {
                    this.events.Add(new MobDiedEvent(target.Identifier));
                    this.events.Add(combatEndEvent);
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
            foreach (var skill in entity.Skills)
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
                //TODO(Caleb) clean this up later, should not have second to list
                this.CreateSkillSnapshots(player.GetSelectedSkills().ToList()),
                true);

            var talents = player.GetSelectedTalents();
            for (int i = 0; i < talents.Count; i++)
            {
                var talent = this.talentRepository.Get(talents[i].Id);
                for (int j = 0; j < talent.Actions.Count; j++)
                {
                    switch (talent.Actions[j])
                    {
                        case AddDotDamageAction action:
                            for (int k = 0; k < combatEntity.Skills.Count; k++)
                            {
                                var skill = combatEntity.Skills[k];
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    skill.DotEffects[skill.DotEffects.Count]
                                        = DotDamageStepSnapshot.FromStep(action.DotDamage);

                                    break;
                                }
                            }
                            break;
                        case AddHitDamageAction action:
                            for (int k = 0; k < combatEntity.Skills.Count; k++)
                            {
                                var skill = combatEntity.Skills[k];
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    skill.DamageSteps[skill.DamageSteps.Count] = DamageStepSnapshot.FromStep(action.HitDamage);
                                    break;
                                }
                            }
                            break;
                        case ApplyEffectAction action:
                            for (int k = 0; k < combatEntity.Skills.Count; k++)
                            {
                                var skill = combatEntity.Skills[k];
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    skill.ApplyEffects[skill.ApplyEffects.Count] = new ApplyEffectSnapshot(action.EffectId);
                                    break;
                                }
                            }
                            break;
                        case ModifyDotDamageAction action:
                            foreach (var skill in combatEntity.Skills)
                            {
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    for (int k = 0; k < skill.DotEffects.Count; k++)
                                    {
                                        var effect = skill.DotEffects[k];
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
                            foreach (var skill in combatEntity.Skills)
                            {
                                if (skill.skillDefinition.Id == action.SkillId)
                                {
                                    for (int k = 0; k < skill.DamageSteps.Count; k++)
                                    {
                                        var effect = skill.DamageSteps[k];
                                        effect.MinBaseDamage = AddScalar(effect.MinBaseDamage, action.MinBaseDamage);
                                        effect.MaxBaseDamage = AddScalar(effect.MaxBaseDamage, action.MaxBaseDamage);
                                    }
                                }
                            }
                            break;
                        case ModifyEffectAction action:
                            break;
                        case ModifySkillAction action:
                            for (int k = 0; k < combatEntity.Skills.Count; k++)
                            {
                                var skill = combatEntity.Skills[k];
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

        internal List<SkillSnapshot> CreateSkillSnapshots(List<string> skillIds)
        {
            var skills = new List<SkillSnapshot>();
            for (int i = 0; i < skillIds.Count; i++)
            {
                List<ApplyEffectSnapshot> applyEffects = [];
                List<DotDamageStepSnapshot> dotEffects = [];
                List<DamageStepSnapshot> dmgEffects = [];

                var skill = this.skillRepository.Get(skillIds[i]);

                for (int j = 0; j < skill.Effects.Count; j++)
                {
                    var effect = skill.Effects[j];
                    switch (effect)
                    {
                        case ApplyEffectStep step:
                            applyEffects.Add(new(step.EffectId));
                            break;
                        case DotDamageStep step:
                            dotEffects.Add(DotDamageStepSnapshot.FromStep(step));
                            break;
                        case HitDamageStep step:
                            dmgEffects.Add(DamageStepSnapshot.FromStep(step));
                            break;
                    }
                }

                skills.Add(new SkillSnapshot(
                    skill,
                    skill.Cost,
                    skill.Cooldown,
                    applyEffects,
                    dmgEffects,
                    dotEffects
                ));
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

        internal static List<IModifier> GetModifiers(List<EffectSnapshot> effects)
        {
            List<IModifier> modifiers = new();
            int modifierIndex = 0;
            for (int i = 0; i < effects.Count; i++)
            {
                var effect = effects[i];
                var effectModifiers = effect.Modifiers;
                for (int j = 0; j < effectModifiers.Count; j++)
                {
                    modifiers[modifierIndex++] = effectModifiers[j];
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
        public record DamageAppliedEvent(string SourceId, string TargetId, string SkillId, float Amount, bool IsCrit) : CombatEvent;
        public record DotDamageApplied(string SourceId, string TargetId, string SkillId, float Amount, bool IsCrit) : CombatEvent;
        public record EffectApplied(string SourceId, string TargetId, string EffectId) : CombatEvent;
        public record StatusApplied(IStatus Status, string SourceId) : CombatEvent;

        public abstract record CombatCommand;
        public record UseSkillCommand(string SkillId, string TargetId) : CombatCommand;
        public record CombatFleeCommand : CombatCommand;
    }
}