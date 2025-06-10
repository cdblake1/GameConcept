using GameData.src.Effect.Stack;
using GameData.src.Shared.Enums;
using GameData.src.Skill;
using GameData.src.Skill.SkillStep;

namespace GameLogic.Combat.Snapshots.Steps
{
    public readonly struct ApplyEffectSnapshot(string effectId)
    {
        public readonly string EffectId = effectId;
    }

    public struct DamageStepSnapshot
    {
        public AttackType AttackType;
        public DamageType DamageType;
        public WeaponType WeaponType;

        public int MinBaseDamage;
        public int MaxBaseDamage;
        public bool CanCrit;

        public SkillScaleProperties ScaleProperties;
        public StackFromEffect? StackFromEffect;

        public static DamageStepSnapshot FromStep(HitDamageStep step)
        {
            return new DamageStepSnapshot
            {
                AttackType = step.AttackType,
                DamageType = step.DamageType,
                WeaponType = step.WeaponType,
                MinBaseDamage = step.MinBaseDamage,
                MaxBaseDamage = step.MaxBaseDamage,
                CanCrit = step.Crit,
                ScaleProperties = step.ScaleProperties,
                StackFromEffect = step.StackFromEffect
            };
        }
    }

    public struct DotDamageStepSnapshot
    {
        public AttackType AttackType;
        public DamageType DamageType;
        public WeaponType WeaponType;
        public int MinBaseDamage;
        public int MaxBaseDamage;
        public bool CanCrit;
        public int Frequency;
        public DurationSnapshot Duration;
        public StackSnapshot StackStrategy;
        public SkillScaleProperties ScaleProperties;

        public static DotDamageStepSnapshot FromStep(DotDamageStep step)
        {
            return new DotDamageStepSnapshot
            {
                AttackType = step.AttackType,
                DamageType = step.DamageType,
                WeaponType = step.WeaponType,
                MinBaseDamage = step.MinBaseDamage,
                MaxBaseDamage = step.MaxBaseDamage,
                CanCrit = step.Crit,
                Frequency = step.Frequency,
                Duration = DurationSnapshot.FromDuration(step.Duration),
                StackStrategy = StackSnapshot.FromStrategy(step.StackStrategy),
                ScaleProperties = step.ScaleProperties
            };
        }
    }
}