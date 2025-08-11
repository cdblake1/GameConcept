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
            SkillDefinition skill,
            DamageStepSnapshot damageStep)
        {
            return new DamageSnapshot()
            {
                Damage = Random.Shared.Next(damageStep.MinBaseDamage, damageStep.MaxBaseDamage),
                SkillDefinition = skill,
                DamageType = damageStep.DamageType,
                AttackType = damageStep.AttackType,
                WeaponType = damageStep.WeaponType,
            };
        }

        public static DamageSnapshot BuildNew(
            SkillDefinition skill,
            DotDamageStepSnapshot damageStep)
        {
            return new DamageSnapshot()
            {
                Damage = Random.Shared.Next(damageStep.MinBaseDamage, damageStep.MaxBaseDamage),
                SkillDefinition = skill,
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

        private static void ApplyScalarOperation(DamageSnapshot snapshot, ScalarOpType operation, int value, bool isCrit = false)
        {
            if (isCrit)
            {
                switch (operation)
                {
                    case ScalarOpType.Additive:
                        snapshot.CritAdditive += value;
                        break;
                    case ScalarOpType.Increased:
                        snapshot.CritIncreased += value;
                        break;
                    case ScalarOpType.Empowered:
                        snapshot.CritEmpowered += value;
                        break;
                }
            }
            else
            {
                switch (operation)
                {
                    case ScalarOpType.Additive:
                        snapshot.ScaleAdditive += value;
                        break;
                    case ScalarOpType.Increased:
                        snapshot.ScaleIncreased += value;
                        break;
                    case ScalarOpType.Empowered:
                        snapshot.ScaleEmpowered += value;
                        break;
                }
            }
        }

        public static DamageSnapshot AddModifiers(this DamageSnapshot snapshot, List<IModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                var action = modifiers[i];

                switch (action)
                {
                    case SkillModifier mod when mod.SkillId == mod.SkillId:
                        ApplyScalarOperation(snapshot, mod.Operation, mod.Value);
                        break;
                    case DamageModifier mod when (snapshot.DamageType & mod.DamageType) != 0:
                        ApplyScalarOperation(snapshot, mod.Operation, mod.Value);
                        break;
                    case AttackModifier mod when mod.AttackType == snapshot.AttackType:
                        ApplyScalarOperation(snapshot, mod.Operation, mod.Value);
                        break;
                    case WeaponModifier mod when mod.WeaponType == snapshot.WeaponType:
                        ApplyScalarOperation(snapshot, mod.Operation, mod.Value);
                        break;
                    case GlobalModifier mod:
                        if (mod.GlobalStat == GlobalStat.Crit)
                        {
                            ApplyScalarOperation(snapshot, mod.Operation, mod.Value, true);
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