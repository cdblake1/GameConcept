using GameDataLayer;
using Xunit;

namespace GameDataLayerTests;


public class LootTests
{
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
            new LootTable.LootTableEntry(sword, 50), // 50% chance
            new LootTable.LootTableEntry(helm, 30), // 30% chance
            new LootTable.LootTableEntry(legs, 20), // 20% chance
            new LootTable.LootTableEntry(chest, 10) // 10% chance
        });

        // Act
        var lootCount = new Dictionary<string, int>
        {
            { sword.Name, 0 },
            { helm.Name, 0 },
            { legs.Name, 0 },
            { chest.Name, 0 },
            { "no loot", 0 }
        };

        for (int i = 0; i < 10000; i++)
        {
            var loot = lootTable.GetRandomLootEntry();
            if (loot is LootTable.LootTableEntry entry)
            {
                lootCount[entry.Item.Name]++;
            }
            else
            {
                lootCount["no loot"]++;
            }
        }

        // Assert
        Assert.True(lootCount[sword.Name] > lootCount[helm.Name]);
        Assert.True(lootCount[helm.Name] > lootCount[legs.Name]);
        Assert.True(lootCount[legs.Name] > lootCount[chest.Name]);
        Assert.True(lootCount["no loot"] > 0);
        Assert.True(lootCount[sword.Name] > 0);
        Assert.True(lootCount[helm.Name] > 0);
        Assert.True(lootCount[legs.Name] > 0);
        Assert.True(lootCount[chest.Name] > 0);
    }

    [Fact]
    public void LootTableReturnsItemBasedOnWeightTest()
    {
        // Arrange
        var lootTests = new LootTests();

        // Act & Assert
        lootTests.LootTableReturnsItemBasedOnWeight();
    }

    [Fact]
    public void AddLootToCharacterTest()
    {
        // Arrange
        var character = new CharacterBase("TestCharacter", new StatTemplate
        {
            AttackPower = 10,
            Defense = 5,
            Health = 100
        }, new LevelManager(1, ExperienceTable.Default, new StatTemplate
        {
            AttackPower = 0,
            Defense = 0,
            Health = 0
        }));

        var sword = ItemTemplates.SwordOfMight;

        // Act
        character.Inventory.AddItem(sword);

        // Assert
        Assert.Contains(sword, character.Inventory);
        Assert.Single(character.Inventory);
    }

    [Fact]
    public void EquipmentIteratorAlwaysReturnsSlot()
    {
        // Arrange
        var character = new CharacterBase("TestCharacter", new StatTemplate
        {
            AttackPower = 10,
            Defense = 5,
            Health = 100
        }, new LevelManager(1, ExperienceTable.Default, new StatTemplate
        {
            AttackPower = 0,
            Defense = 0,
            Health = 0
        }));

        var sword = ItemTemplates.SwordOfMight;
        var helm = ItemTemplates.HelmOfValor;

        // Act
        character.Inventory.AddItem(sword);
        character.Inventory.AddItem(helm);

        character.EquipItem(sword);
        character.EquipItem(helm);

        // Assert
        Assert.Equal(sword, character.EquipmentManager[EquipmentKind.Weapon]);
        Assert.Equal(helm, character.EquipmentManager[EquipmentKind.HeadArmor]);
    }

    [Fact]
    public void DropLootTest()
    {
        // Arrange
        var sword = ItemTemplates.SwordOfMight;
        var helm = ItemTemplates.HelmOfValor;

        var lootTable = new LootTable(new List<LootTable.LootTableEntry>
        {
            new LootTable.LootTableEntry(sword, 50),
            new LootTable.LootTableEntry(helm, 30)
        }, alwaysDropLoot: true);

        var mob = new MobBase("TestMob", new StatTemplate
        {
            AttackPower = 10,
            Defense = 5,
            Health = 100
        }, lootTable, new LevelManager(1, ExperienceTable.Default, new StatTemplate
        {
            AttackPower = 0,
            Defense = 0,
            Health = 0
        }), 100);

        var player = new CharacterBase("TestPlayer", new StatTemplate
        {
            AttackPower = 10,
            Defense = 5,
            Health = 100
        }, new LevelManager(1, ExperienceTable.Default, new StatTemplate
        {
            AttackPower = 0,
            Defense = 0,
            Health = 0
        }));

        // Act
        var loot = mob.DropLoot();
        Assert.NotNull(loot);

        if (loot is Equipment item)
        {
            player.Inventory.AddItem(item);
        }

        // Assert
    }
}