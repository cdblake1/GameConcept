using GameData.src.Class;
using GameData.src.Effect;
using GameData.src.Effect.Stack;
using GameData.src.Player;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers;
using GameData.src.Skill;
using GameData.src.Skill.SkillStep;
using GameLogic.Player;

namespace GameLogicTests.Helpers.Effects
{
    public static class PlayerTestSetup
    {
        private const string playerEffect1Id = "player_effect_1";
        private const string playerEffect2Id = "player_effect_2";
        private const string playerEffect3Id = "player_effect_3";
        private const string playerSkill1Id = "player_skill_1";
        private const string playerSkill2Id = "player_apply_speed_slow";
        private const string playerSkill3Id = "player_add_burst_skill";

        public static EffectDefinition PlayerEffectOne = new EffectDefinition()
        {
            Category = EffectCategory.Buff,
            Duration = Duration.FromTurns(1),
            Id = playerEffect1Id,
            Modifiers = [
                new SkillModifier(playerSkill2Id, new() {ScaleEmpowered = 20}),
        ],
            StackStrategy = new StackDefault() { MaxStacks = 1, RefreshMode = StackRefreshMode.ResetTime, StacksPerApplication = 1 }
        };

        public static EffectDefinition PlayerEffectTwo = new EffectDefinition()
        {
            Category = EffectCategory.Buff,
            Duration = Duration.FromExpiry(new Duration.ExpiresWith(Duration.ExpiresWith.Category.Effect, playerSkill1Id)),
            Id = playerEffect2Id,
            Modifiers = [
                new DamageModifier(DamageType.Physical, new() {ScaleIncreased = 50}),
        ],
            StackStrategy = new StackDefault() { MaxStacks = 1, RefreshMode = StackRefreshMode.ResetTime, StacksPerApplication = 1 }
        };

        public static EffectDefinition PlayerEffectThree = new()
        {
            Category = EffectCategory.Debuff,
            Duration = Duration.Permanent(),
            Id = playerEffect3Id,
            Modifiers = [
                new GlobalModifier(GlobalStat.Speed, new () {ScaleIncreased = 100})
            ],
            StackStrategy = new StackDefault() { MaxStacks = 3, RefreshMode = StackRefreshMode.AddTime, StacksPerApplication = 1 }
        };

        public static SkillDefinition PlayerSkillOne = new()
        {
            Cooldown = 1,
            Cost = 1,
            Effects = [
                        new HitDamageStep(
                            AttackType: AttackType.Hit,
                            DamageType: DamageType.Physical,
                            WeaponType: WeaponType.Melee,
                            MinBaseDamage: 5,
                            MaxBaseDamage: 10,
                            Crit: true,
                            ScaleProperties: new SkillScaleProperties(100, 150, 50),
                            null),
                        new ApplyEffectStep(playerEffect1Id)],
            Id = playerSkill1Id,
            Presentation = new()
            {
                Description = "player skill description",
                Name = "Player Skill One",
                Icon = "icon"
            },
        };

        public static SkillDefinition PlayerSkillTwo = new()
        {
            Cooldown = 3,
            Cost = 1,
            Effects = [
                new ApplyEffectStep(playerEffect3Id)
            ],
            Id = playerSkill2Id,
            Presentation = new()
            {
                Description = "player applies stacking speed slow",
                Icon = "icon",
                Name = "Player Speed Slow"
            }
        };

        public static SkillDefinition PlayerSkillThree = new()
        {
            Cooldown = 1,
            Cost = 1,
            Effects = [
                new ApplyEffectStep(playerEffect2Id),
                new DotDamageStep(
                    AttackType: AttackType.Dot,
                    DamageType: DamageType.Bleed,
                    WeaponType: WeaponType.Melee,
                    MinBaseDamage: 3,
                    MaxBaseDamage: 5,
                    Crit: false,
                    Duration: Duration.FromTurns(2),
                    Frequency: 1,
                    StackStrategy: new StackDefault() {
                        StacksPerApplication = 1,
                        MaxStacks = 1,
                        RefreshMode = StackRefreshMode.ResetTime
                    },
                    ScaleProperties: new SkillScaleProperties(50, 50, 0)
                )
            ],
            Id = playerSkill3Id,
            Presentation = new()
            {
                Description = "Add a 2 turn dot, and store an effect that will be bursted by player skill one",
                Name = "Player Store Damage Skill",
                Icon = "icon"
            }
        };

        public static PlayerInstance Create(PlayerDefinition player, int level = 10)
        {
            return new PlayerInstance(
                player,
                new StatCollection(),
                level
            );
        }

        public static ClassDefinition PlayerClassDefinition = new(
            Id: "player_class_1",
            Talents: [],
            SkillEntries: [
                new SkillEntry(playerSkill1Id, 1),
                new SkillEntry(playerSkill2Id, 5),
                new SkillEntry(playerSkill3Id, 10)]
        );
    }
}