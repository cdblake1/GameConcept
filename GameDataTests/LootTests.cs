using GameData;
using GameData.Mobs;
using static CraftingMaterialTemplates;

namespace GameDataTests
{
    public class LootTests
    {
        public LootTests()
        {
        }

        private Player CreateTestCharacter() => new("TestCharacter");

        [Fact]
        public void LootTableReturnsItemBasedOnWeight()
        {
            // Arrange
            var sword = ItemTemplates.SwordOfMight;
            var helm = ItemTemplates.HelmOfValor;
            var legs = ItemTemplates.LegsOfMight;
            var chest = ItemTemplates.ChestOfMight;

            var lootTable = new LootTable(new List<LootTable.LootTableEntry>
            {
                new(sword, 50),
                new(helm, 30),
                new(legs, 20),
                new(chest, 10)
            });

            var lootCount = new Dictionary<string, int>
            {
                { sword.Name, 0 },
                { helm.Name, 0 },
                { legs.Name, 0 },
                { chest.Name, 0 },
                { "no loot", 0 }
            };

            // Act
            for (int i = 0; i < 10000; i++)
            {
                var loot = lootTable.GetRandomLootEntry();
                if (loot is LootTable.LootTableEntry entry)
                    lootCount[entry.Item.Name]++;
                else
                    lootCount["no loot"]++;
            }

            // Assert
            Assert.True(lootCount[sword.Name] > lootCount[helm.Name]);
            Assert.True(lootCount[helm.Name] > lootCount[legs.Name]);
            Assert.True(lootCount[legs.Name] > lootCount[chest.Name]);
            Assert.True(lootCount["no loot"] > 0);
            Assert.All(new[] { sword.Name, helm.Name, legs.Name, chest.Name }, name => Assert.True(lootCount[name] > 0));
        }

        [Fact]
        public void LootTableReturnsItemBasedOnWeightTest()
        {
            LootTableReturnsItemBasedOnWeight();
        }

        [Fact]
        public void AddLootToCharacterTest()
        {
            var character = CreateTestCharacter();
            var sword = ItemTemplates.SwordOfMight;

            character.Inventory.AddItem(sword);

            Assert.Contains(sword, character.Inventory.Equipment);
            Assert.Single(character.Inventory.Equipment);
        }

        [Fact]
        public void AddCraftingMaterialToCharacterTest()
        {
            var character = CreateTestCharacter();
            var ironScrap = IronScrap.FromRange(1, 2);

            character.Inventory.AddItem(ironScrap);

            Assert.Contains(ironScrap, character.Inventory.CraftingMaterials);
        }

        [Fact]
        public void AddCraftingMaterialThatDropsToCharacterTest()
        {
            var character = CreateTestCharacter();
            var mob = TestMob.Create();

            var loot = mob.DropLoot();
            Assert.NotNull(loot);
            Assert.IsAssignableFrom<CraftingMaterial>(loot);

            character.Inventory.AddItem(loot);

            Assert.Contains(loot, character.Inventory.CraftingMaterials);
        }

        [Fact]
        public void EquipmentIteratorAlwaysReturnsSlot()
        {
            var character = CreateTestCharacter();

            var sword = ItemTemplates.SwordOfMight;
            var helm = ItemTemplates.HelmOfValor;

            character.Inventory.AddItem(sword);
            character.Inventory.AddItem(helm);

            character.EquipItem(sword);
            character.EquipItem(helm);

            Assert.Equal(sword, character.Equipment[EquipmentKind.Weapon]);
            Assert.Equal(helm, character.Equipment[EquipmentKind.HeadArmor]);
        }

        [Fact]
        public void DropLootTest()
        {
            var mob = TestMob.Create();

            var player = CreateTestCharacter();

            var loot = mob.DropLoot();
            Assert.NotNull(loot);

            player.Inventory.AddItem(loot);

            Assert.True(player.Inventory.RemoveItem(loot));
        }
    }

    public class TestMob : MobBase
    {
        public TestMob(IActor actor, LootTable LootTable, IReadOnlyList<Skill> skills, int level) : base(actor, LootTable, skills, level)
        {
        }

        public static MobBase Create()
        {
            return new TestMob(new MobDto("test mob", new()
            {
                AttackPower = 10,
                Defense = 5,
                Health = 100,
                Speed = 1
            }), new LootTable([
                new LootTable.LootTableEntry(IronScrap.FromAmount(2), 100)
            ], true), [new DefaultMobAttack()], 1);
        }
    }
}
