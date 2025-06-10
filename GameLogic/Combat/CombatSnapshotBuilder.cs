using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill;
using GameData.src.Skill.SkillStep;
using GameLogic.Combat.Snapshots;
using GameLogic.Combat.Snapshots.Steps;
using GameLogic.Player;

namespace GameLogic.Combat
{
    public static class DamageSnapshotBuilder
    {
        public static DamageSnapshot BuildNew(
            SkillDefinition skills,
            DamageStepSnapshot damageStep)
        {
            return new DamageSnapshot()
            {
                Damage = Random.Shared.Next(damageStep.MinBaseDamage, damageStep.MaxBaseDamage),
                SkillDefinition = skills,
                DamageType = damageStep.DamageType,
                AttackType = damageStep.AttackType,
                WeaponType = damageStep.WeaponType,
            };
        }

        public static DamageSnapshot BuildNew(
            SkillDefinition skills,
            DotDamageStepSnapshot damageStep)
        {
            return new DamageSnapshot()
            {
                Damage = Random.Shared.Next(damageStep.MinBaseDamage, damageStep.MaxBaseDamage),
                SkillDefinition = skills,
                DamageType = damageStep.DamageType,
                AttackType = damageStep.AttackType,
                WeaponType = damageStep.WeaponType,
            };
        }

        public static DamageSnapshot AddStats(this DamageSnapshot snapshot, StatCollection stats)
        {
            snapshot.ScaleAdditive
                += stats.GetStat(snapshot.DamageType, ScalarOpType.Additive)
                + stats.GetStat(snapshot.AttackType, ScalarOpType.Additive)
                + stats.GetStat(snapshot.WeaponType, ScalarOpType.Additive);

            snapshot.ScaleEmpowered
                += stats.GetStat(snapshot.DamageType, ScalarOpType.Empowered)
                + stats.GetStat(snapshot.AttackType, ScalarOpType.Empowered)
                + stats.GetStat(snapshot.WeaponType, ScalarOpType.Empowered);

            snapshot.ScaleIncreased
                += stats.GetStat(snapshot.DamageType, ScalarOpType.Increased)
                + stats.GetStat(snapshot.AttackType, ScalarOpType.Increased)
                + stats.GetStat(snapshot.WeaponType, ScalarOpType.Increased);

            snapshot.CritAdditive
                += stats.GetStat(GlobalStat.Crit, ScalarOpType.Additive);
            snapshot.CritEmpowered
                += stats.GetStat(GlobalStat.Crit, ScalarOpType.Empowered);
            snapshot.CritIncreased
                += stats.GetStat(GlobalStat.Crit, ScalarOpType.Increased);

            return snapshot;
        }

        public static DamageSnapshot AddModifiers(this DamageSnapshot snapshot, ReadOnlySpan<IModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Length; i++)
            {
                var action = modifiers[i];

                switch (action)
                {
                    case SkillModifier mod when mod.SkillId == mod.SkillId:
                        snapshot.ScaleAdditive += mod.Operation.ScaleAdded;
                        snapshot.ScaleIncreased += mod.Operation.ScaleIncreased;
                        snapshot.ScaleEmpowered += mod.Operation.ScaleEmpowered;
                        break;
                    case DamageModifier mod when (snapshot.DamageType & mod.DamageType) != 0:
                        snapshot.ScaleAdditive += mod.Operation.ScaleAdded;
                        snapshot.ScaleIncreased += mod.Operation.ScaleIncreased;
                        snapshot.ScaleEmpowered += mod.Operation.ScaleEmpowered;
                        break;
                    case AttackModifier mod when mod.AttackType == snapshot.AttackType:
                        snapshot.ScaleAdditive += mod.Operation.ScaleAdded;
                        snapshot.ScaleIncreased += mod.Operation.ScaleIncreased;
                        snapshot.ScaleEmpowered += mod.Operation.ScaleEmpowered;
                        break;
                    case WeaponModifier mod when mod.WeaponType == snapshot.WeaponType:
                        snapshot.ScaleAdditive += mod.Operation.ScaleAdded;
                        snapshot.ScaleIncreased += mod.Operation.ScaleIncreased;
                        snapshot.ScaleEmpowered += mod.Operation.ScaleEmpowered;
                        break;
                    case GlobalModifier mod:
                        if (mod.GlobalStat == GlobalStat.Crit)
                        {
                            snapshot.CritAdditive += mod.Operation.ScaleAdded;
                            snapshot.CritIncreased += mod.Operation.ScaleIncreased;
                            snapshot.CritEmpowered += mod.Operation.ScaleEmpowered;
                        }
                        break;
                }
            }

            return snapshot;
        }

        private static bool CalculateCrit(this DamageSnapshot snapshot)
        {
            var critChance = snapshot.BaseCritChance + snapshot.CritAdditive;
            var crit = Random.Shared.NextSingle() < critChance * (1 + snapshot.CritIncreased) * (1 + snapshot.CritEmpowered);
            return crit;
        }

        public static (bool isCrit, float damage) CalculateDamage(this DamageSnapshot snapshot)
        {
            var crit = CalculateCrit(snapshot);
            var baseDamage = snapshot.Damage * (1 + snapshot.ScaleAdditive) * (1 + snapshot.ScaleIncreased) * (1 + snapshot.ScaleEmpowered);
            return (crit, baseDamage);
        }
    }
}