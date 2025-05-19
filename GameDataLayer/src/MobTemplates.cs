using GameDataLayer;

public static class MobTemplates
{
    public static MobBase GetGoblin(int startingLevel) => new MobBase(
        "Goblin",
        new StatTemplate
        {
            AttackPower = 5,
            Defense = 2,
            Health = 20
        },
        new LootTable(new List<LootTable.LootTableEntry>
        {
            new(ItemTemplates.HelmOfValor, 30),
            new(ItemTemplates.SwordOfMight, 30),
            new(ItemTemplates.LegsOfMight, 30)
        }),
        new LevelManager(int.MaxValue, ExperienceTable.Default, new()
        {
            AttackPower = 1,
            Defense = 1,
            Health = 5
        },
        startingLevel),
        30
    );

    public static MobBase GetOrc(int startingLevel) => new MobBase(
        "Orc",
        new StatTemplate
        {
            AttackPower = 10,
            Defense = 5,
            Health = 50
        },
        new LootTable(new List<LootTable.LootTableEntry>
        {
            new(ItemTemplates.HelmOfValor, 30),
            new(ItemTemplates.SwordOfMight, 30),
            new(ItemTemplates.LegsOfMight, 30)
        }),
        new LevelManager(int.MaxValue, ExperienceTable.Default, new()
        {
            AttackPower = 2,
            Defense = 2,
            Health = 10
        },
        startingLevel),
        60
    );

    public static MobBase GetTroll(int startingLevel) => new MobBase(
        "Troll",
        new StatTemplate
        {
            AttackPower = 15,
            Defense = 10,
            Health = 100
        },
        new LootTable(new List<LootTable.LootTableEntry>
        {
            new(ItemTemplates.HelmOfValor, 30),
            new(ItemTemplates.SwordOfMight, 30),
            new(ItemTemplates.LegsOfMight, 30)
        }),
        new LevelManager(int.MaxValue, ExperienceTable.Default, new()
        {
            AttackPower = 3,
            Defense = 3,
            Health = 20
        },
        startingLevel),
        90
    );

    public static MobBase GetBoar(int startingLevel) => new MobBase(
        "Boar",
        new StatTemplate
        {
            AttackPower = 8,
            Defense = 3,
            Health = 30
        },
        new LootTable(new List<LootTable.LootTableEntry>
        {
            new(ItemTemplates.HelmOfValor, 30),
            new(ItemTemplates.SwordOfMight, 30),
            new(ItemTemplates.LegsOfMight, 30)
        }),
        new LevelManager(int.MaxValue, ExperienceTable.Default, new()
        {
            AttackPower = 1,
            Defense = 1,
            Health = 5
        },
        startingLevel),
        40
    );
    public static MobBase GetBear(int startingLevel) => new MobBase(
        "Bear",
        new StatTemplate
        {
            AttackPower = 12,
            Defense = 6,
            Health = 60
        },
        new LootTable(new List<LootTable.LootTableEntry>
        {
            new(ItemTemplates.HelmOfValor, 30),
            new(ItemTemplates.SwordOfMight, 30),
            new(ItemTemplates.LegsOfMight, 30)
        }),
        new LevelManager(int.MaxValue, ExperienceTable.Default, new()
        {
            AttackPower = 2,
            Defense = 2,
            Health = 15
        },
        startingLevel),
        80
    );

    public static MobBase GetWolf(int startingLevel) => new MobBase(
        "Wolf",
        new StatTemplate
        {
            AttackPower = 7,
            Defense = 4,
            Health = 25
        },
        new LootTable(new List<LootTable.LootTableEntry>
        {
            new(ItemTemplates.HelmOfValor, 30),
            new(ItemTemplates.SwordOfMight, 30),
            new(ItemTemplates.LegsOfMight, 30)
        }),
        new LevelManager(int.MaxValue, ExperienceTable.Default, new()
        {
            AttackPower = 1,
            Defense = 1,
            Health = 5
        },
        startingLevel),
        70
    );
}