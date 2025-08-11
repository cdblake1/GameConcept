using GameData.src.Effect;
using GameData.src.Effect.Stack;
using GameData.src.Mob;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill;
using GameData.src.Skill.SkillStep;

namespace GameLogicTests.Helpers.Setup
{
    public static class MobTestSetup
    {
        public const string mobSkill1Id = "mob_reduce_armor_skill";
        public const string mobSkill2Id = "mob_commune_nature_skill";
        public const string mobEffect1Id = "mob_apply_armor_reduction_stacking_effect";
        public const string mobEffect2Id = "mob_apply_scaling_effect";
        public static EffectDefinition MobEffectOne = new()
        {
            Category = EffectCategory.Debuff,
            Duration = Duration.FromTurns(3),
            Id = mobEffect1Id,
            Modifiers = [
                new GlobalModifier(GlobalStat.Armor, ScalarOpType.Additive, -10)
            ],
            StackStrategy = new StackDefault()
            {
                MaxStacks = 3,
                StacksPerApplication = 1,
                RefreshMode = StackRefreshMode.ResetTime
            }
        };

        public static EffectDefinition MobEffectTwo = new()
        {
            Category = EffectCategory.Buff,
            Duration = Duration.FromTurns(3),
            Id = mobEffect2Id,
            Modifiers = [
                new AttackModifier(AttackType.Hit, ScalarOpType.Additive, 30)
            ],
            StackStrategy = new StackDefault()
            {
                MaxStacks = 1,
                StacksPerApplication = 1,
                RefreshMode = StackRefreshMode.ResetTime
            }
        };

        public static SkillDefinition MobSkillOne = new()
        {
            Cooldown = 1,
            Cost = 1,
            Effects = [
                new HitDamageStep(AttackType: AttackType.Hit,
                    DamageType: DamageType.Nature,
                    WeaponType: WeaponType.Spell,
                    MinBaseDamage: 7,
                    MaxBaseDamage: 9,
                    Crit: true,
                    ScaleProperties: new SkillScaleProperties(
                        ScaleAdded: 100,
                        ScaleIncreased: 100,
                        ScaleSpeed: 50
                    )),
                new ApplyEffectStep(mobEffect1Id)
            ],
            Id = mobSkill1Id,
            Presentation = new()
            {
                Description = "Blast the target with a spell, and apply a stacking armor reduction effect",
                Name = "Spell Armor Penetrate",
                Icon = "icon"
            }
        };

        public static SkillDefinition MobSkillTwo = new()
        {
            Cooldown = 3,
            Cost = 1,
            Effects = [
                new ApplyEffectStep(mobEffect2Id)
            ],
            Id = mobSkill2Id,
            Presentation = new()
            {
                Name = "Commune Nature",
                Description = "Commune with nature, empowering nature damage by 30%",
                Icon = "icon"
            },
        };

        public static MobDefinition Create()
            => new(
                Id: "mob_test",
                LootTable: "mob_test_loot_table",
                Presentation: new()
                {
                    Name = "Test Mob",
                    Description = "Test Mob",
                    Icon = "icon"
                },
                Skills: [mobSkill1Id, mobSkill2Id],
                Stats: "mob_test_stats",
                ExpTable: "mob_test_exp_table"
            );
    }
}