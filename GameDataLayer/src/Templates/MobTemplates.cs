using GameDataLayer;
using static CraftingMaterialTemplates;

public static class MobTemplates
{
    public static class GoblinMobs
    {
        public class Goblin : MobBase
        {
            public Goblin() : base(
                "Goblin",
                "Goblin0",
                100,
                new StatTemplate
                {
                    AttackPower = 8,
                    Defense = 0,
                    Health = 30
                },
                new LootTable(new List<LootTable.LootTableEntry>
                {
                    new(TatteredCloth.FromRange(1, 2), 50),
                    new(ItemTemplates.SwordOfMight, 5)
                }))
            { 
            }
        }

        public class GoblinArcher : MobBase
        {
            public GoblinArcher() : base(
                "Goblin Archer",
                "GoblinArcher0",
                100,
                new StatTemplate
                {
                    AttackPower = 10,
                    Defense = 0,
                    Health = 40
                },
                new LootTable(new List<LootTable.LootTableEntry>
                {
                    new(WoodenShoot.FromRange(1, 2), 50),
                    new(ItemTemplates.SwordOfMight, 5)
                }))
            { }
        }

        public class GoblinWarrior : MobBase
        {
            public GoblinWarrior() : base(
                "Goblin Warrior",
                "GoblinWarrior0",
                100,
                new StatTemplate
                {
                    AttackPower = 12,
                    Defense = 1,
                    Health = 50
                },
                new LootTable(new List<LootTable.LootTableEntry>
                {
                    new(IronScrap.FromRange(1,2), 50),
                    new(ItemTemplates.SwordOfMight, 5)
                }))
            { }
        }
    }

    public static class OrcMobs
    {
        public class Orc : MobBase
        {
            public Orc() : base(
                "Orc",
                "Orc0",
                100,
                new StatTemplate
                {
                    AttackPower = 12,
                    Defense = 5,
                    Health = 50
                },
                new LootTable(new List<LootTable.LootTableEntry>
                {
                    new(IronScrap.FromRange(2, 4), 50),
                    new(ItemTemplates.SwordOfMight, 10)
                }))
            { }
        }
    }

    public static class TrollMobs
    {
        public class Troll : MobBase
        {
            public Troll() : base(
                "Troll",
                "Troll0",
                100,
                new StatTemplate
                {
                    AttackPower = 15,
                    Defense = 10,
                    Health = 100
                },
                new LootTable(new List<LootTable.LootTableEntry>
                {
                    new(IronScrap.FromRange(1, 2), 50),
                    new(ItemTemplates.SwordOfMight, 5)
                }))
            { }
        }
    }

    public static class AnimalMobs
    {
        public class Boar : MobBase
        {
            public Boar() : base(
                "Boar",
                "Boar0",
                100,
                new StatTemplate
                {
                    AttackPower = 8,
                    Defense = 1,
                    Health = 30
                },
                new LootTable(new List<LootTable.LootTableEntry>
                {
                    new(TatteredCloth.FromRange(1, 2), 50),
                    new(ItemTemplates.SwordOfMight, 5)
                }))
            { }
        }

        public class Bear : MobBase
        {
            public Bear() : base(
                "Bear",
                "Bear0",
                100,
                new StatTemplate
                {
                    AttackPower = 13,
                    Defense = 2,
                    Health = 50
                },
                new LootTable(new List<LootTable.LootTableEntry>
                {
                    new(TatteredCloth.FromRange(2, 3), 50),
                    new(ItemTemplates.SwordOfMight, 5)
                }))
            { }
        }

        public class Wolf : MobBase
        {
            public Wolf() : base(
                "Wolf",
                "Wolf0",
                100,
                new StatTemplate
                {
                    AttackPower = 11,
                    Defense = 0,
                    Health = 40
                },
                new LootTable(new List<LootTable.LootTableEntry>
                {
                    new(TatteredCloth.FromRange(1, 2), 50),
                    new(ItemTemplates.SwordOfMight, 5)
                }))
            { }
        }
    }

    public static class Bosses
    {
        public class DenMother : MobBase
        {
            public DenMother() : base(
                "Den Mother",
                "DenMother0",
                100,
                new StatTemplate
                {
                    AttackPower = 15,
                    Defense = 4,
                    Health = 90
                },
                new LootTable(new List<LootTable.LootTableEntry>
                {
                    new(IronScrap.FromRange(3, 5), 50),
                    new(ItemTemplates.SwordOfMight, 25)
                }))
            { }
        }

        public class GoblinCaptain : MobBase
        {
            public GoblinCaptain() : base(
                "Goblin Captain",
                "GoblinCaptain0",
                100,
                new StatTemplate
                {
                    AttackPower = 15,
                    Defense = 4,
                    Health = 100
                },
                new LootTable(new List<LootTable.LootTableEntry>
                {
                    new(IronScrap.FromRange(5, 10), 50),
                    new(ItemTemplates.SwordOfMight, 30)
                }))
            { }
        }
    }
}