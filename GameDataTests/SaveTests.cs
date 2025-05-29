#nullable enable

using GameData;
using GameData.Inventory;
using GameData.Save;
using Newtonsoft.Json;
using static CraftingMaterialTemplates;
using static GameData.CraftingMaterial;

namespace GameDataTests;

public class SaveTests
{
    [Fact]
    public void CanLoadAllSaves()
    {
        var player = new Player("TestCharacter");
        var saveManager = new SaveManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Guid.NewGuid().ToString()));

        var saveNames = new[] { "TestSave", "TestSave2", "TestSave3", "TestSave4" };


        var filePaths = new List<string>();

        try
        {
            foreach (var saveName in saveNames)
            {
                filePaths.Add(saveManager.SaveGame(saveName, player));
            }

            var saves = saveManager.LoadAvailableSaveStates();

            Assert.NotNull(saves);
            Assert.NotEmpty(saves);
            Assert.Equal(saveNames.Length, saves.Count);

            foreach (var saveName in saveNames)
            {
                Assert.Contains(saves, save => save.GameName == saveName);
            }

            var loadedSave = saveManager.LoadGame(saves[2].Id);
            Assert.Equal(saves[2].GameName, loadedSave.GameName);
            Assert.Equal(saves[2].Id, loadedSave.Id);
            Assert.Equal(player.Name, loadedSave.Player.Name);
            Assert.Equal(player.MaxHealth, loadedSave.Player.MaxHealth);
            Assert.Equal(player.CurrentHealth, loadedSave.Player.CurrentHealth);
            Assert.Equal(player.Inventory.Equipment.Count, loadedSave.Player.Inventory.Equipment.Count);
            Assert.Equal(player.Inventory.CraftingMaterials.Count, loadedSave.Player.Inventory.CraftingMaterials.Count);
        }
        finally
        {
            foreach (var filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }


    [Fact]
    public void CanLoadSaveStateFromFile()
    {
        var saveManager = new SaveManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Guid.NewGuid().ToString()));
        var player = new Player("TestCharacter");
        var sword = ItemTemplates.SwordOfMight;
        var ironScrap = IronScrap.FromRange(1, 2);
        player.Inventory.AddItem(sword);
        player.Inventory.AddItem(ironScrap);

        var filePath = saveManager.GetSaveFilePath("TestSave");
        try
        {
            var id = saveManager.SaveGame("TestSave", player);
            var loadedState = saveManager.LoadGame(id);
            Assert.Equal("TestSave", loadedState.GameName);
            Assert.Equal(id, loadedState.Id);

            var restoredPlayer = Player.Restore(loadedState.Player);
            Assert.NotNull(restoredPlayer);
            Assert.Equal(player.Name, restoredPlayer.Name);
            Assert.Equal(player.MaxHealth, restoredPlayer.MaxHealth);
            Assert.Equal(player.CurrentHealth, restoredPlayer.CurrentHealth);
            Assert.Equal(player.Inventory.Equipment.Count, restoredPlayer.Inventory.Equipment.Count);
            Assert.Equal(player.Inventory.CraftingMaterials.Count, restoredPlayer.Inventory.CraftingMaterials.Count);

            Assert.Collection(restoredPlayer.Inventory.Equipment,
                item => Assert.Equal(player.Inventory.Equipment.First().Name, item.Name));
            Assert.Collection(restoredPlayer.Inventory.CraftingMaterials,
                material => Assert.Equal(player.Inventory.CraftingMaterials.First().Name, material.Name));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void CanSerializePlayer()
    {
        var player = new Player("TestCharacter");

        var sword = ItemTemplates.SwordOfMight;
        var helmet = ItemTemplates.HelmOfValor;
        var ironScrap = IronScrap.FromAmount(100);
        player.Inventory.AddItem(sword);
        player.Inventory.AddItem(ironScrap);
        player.Inventory.AddItem(helmet);
        player.EquipItem(helmet);
        player.Class = new BloodReaver();

        var serializedPlayer = JsonConvert.SerializeObject(player.Serialize());
        Assert.NotNull(serializedPlayer);

        var deserializedPlayerDto = JsonConvert.DeserializeObject<Player.PlayerDto>(serializedPlayer);

        var restoredPlayer = Player.Restore(deserializedPlayerDto);
        Assert.NotNull(restoredPlayer);

        Assert.Equal(player.Name, restoredPlayer.Name);
        Assert.Equal(player.MaxHealth, restoredPlayer.MaxHealth);
        Assert.Equal(player.CurrentHealth, restoredPlayer.CurrentHealth);
        Assert.Equal(player.Class?.Name, restoredPlayer.Class?.Name);
        Assert.Equal(player.Class?.Description, restoredPlayer.Class?.Description);
        Assert.Equal(player.Class?.GetType(), restoredPlayer.Class?.GetType());
        Assert.Equal(player.LevelManager.CurrentLevel, restoredPlayer.LevelManager.CurrentLevel);
        Assert.Equal(player.LevelManager.CurrentExperience, restoredPlayer.LevelManager.CurrentExperience);

        Assert.Equal(player.Inventory.Equipment.Count, restoredPlayer.Inventory.Equipment.Count);
        Assert.Equal(player.Inventory.CraftingMaterials.Count, restoredPlayer.Inventory.CraftingMaterials.Count);

        Assert.Equal(player.Inventory.Equipment.First().Name, restoredPlayer.Inventory.Equipment.First().Name);
        Assert.Equal(player.Inventory.Equipment.First().Description, restoredPlayer.Inventory.Equipment.First().Description);
        Assert.Equal(player.Inventory.Equipment.First().Amount.Amount, restoredPlayer.Inventory.Equipment.First().Amount.Amount);

        Assert.Equal(player.Inventory.CraftingMaterials.First().Name, restoredPlayer.Inventory.CraftingMaterials.First().Name);
        Assert.Equal(player.Inventory.CraftingMaterials.First().Description, restoredPlayer.Inventory.CraftingMaterials.First().Description);
        Assert.Equal(player.Inventory.CraftingMaterials.First().Amount.Amount, restoredPlayer.Inventory.CraftingMaterials.First().Amount.Amount);
    }

    [Fact]
    public void CanSerializeEquipmentManager()
    {
        var equipmentManager = new EquipmentManager();
        var sword = ItemTemplates.SwordOfMight;
        var shield = ItemTemplates.HelmOfValor;

        equipmentManager.Equip(sword);
        equipmentManager.Equals(shield);

        var serializer = JsonConvert.SerializeObject(equipmentManager.Serialize());
        Assert.NotNull(serializer);
        var deserialize = JsonConvert.DeserializeObject<EquipmentManager.EquipmentManagerDto>(serializer);

        var restoredEquipmentManager = EquipmentManager.Restore(deserialize);
        Assert.NotNull(restoredEquipmentManager);
        Assert.Equal(equipmentManager.GetAllEquipment().Count(), restoredEquipmentManager.GetAllEquipment().Count());
    }

    [Fact]
    public void CanSerializePlayerInventory()
    {
        var player = new Player("TestCharacter");
        var sword = ItemTemplates.SwordOfMight;
        var ironScrap = IronScrap.FromRange(1, 2);

        player.Inventory.AddItem(sword);
        player.Inventory.AddItem(ironScrap);

        var serializer = JsonConvert.SerializeObject(player.Inventory.Serialize());
        Assert.NotNull(serializer);
        var deserialize = JsonConvert.DeserializeObject<InventoryManager.InventoryManagerDto>(serializer);

        var restoredInventory = InventoryManager.Restore(deserialize);
        Assert.NotNull(restoredInventory);
        Assert.Equal(player.Inventory.Equipment.Count, restoredInventory.Equipment.Count);
        Assert.Equal(player.Inventory.CraftingMaterials.Count, restoredInventory.CraftingMaterials.Count);
        Assert.Equal(player.Inventory.Equipment.First().Name, restoredInventory.Equipment.First().Name);
        Assert.Equal(player.Inventory.CraftingMaterials.First().Name, restoredInventory.CraftingMaterials.First().Name);
        Assert.Equal(player.Inventory.CraftingMaterials.First().Amount.Amount, restoredInventory.CraftingMaterials.First().Amount.Amount);
        Assert.Equal(player.Inventory.Equipment.First().Description, restoredInventory.Equipment.First().Description);
        Assert.Equal(player.Inventory.CraftingMaterials.First().Description, restoredInventory.CraftingMaterials.First().Description);
        Assert.Equal(player.Inventory.Equipment.First().Amount.Amount, restoredInventory.Equipment.First().Amount.Amount);
    }

    [Fact]
    public void CanSerializeEquipment()
    {
        var equipment = new Equipment("Sword", "some description", GoldCoin.FromAmount(100), new()
        {
            AttackPower = 10,
            Defense = 5,
            Health = 0,
            Speed = 0,
        },
        EquipmentKind.Weapon, ItemRarity.Epic);

        var serializer = JsonConvert.SerializeObject(equipment.Serialize());

        Assert.NotNull(serializer);

        var deserialize = JsonConvert.DeserializeObject<EquipmentDto>(serializer);

        var restoredEquipment = Equipment.Restore(deserialize);
        Assert.NotNull(restoredEquipment);
        Assert.Equal(equipment.Name, restoredEquipment.Name);
        Assert.Equal(equipment.Description, restoredEquipment.Description);
        Assert.Equal(equipment.Kind, restoredEquipment.Kind);
        Assert.Equal(equipment.Stats.AttackPower, restoredEquipment.Stats.AttackPower);
        Assert.Equal(equipment.Stats.Defense, restoredEquipment.Stats.Defense);
        Assert.Equal(equipment.Stats.Health, restoredEquipment.Stats.Health);
        Assert.Equal(equipment.Stats.Speed, restoredEquipment.Stats.Speed);
        Assert.Equal(equipment.Amount.Amount, restoredEquipment.Amount.Amount);
        Assert.Equal(equipment.ToString(), restoredEquipment.ToString());
        Assert.Equal(equipment.GetType(), restoredEquipment.GetType());
    }

    [Fact]
    public void CanSerializeCraftingMaterial()
    {
        var woodenShoot = WoodenShoot.FromAmount(10);

        var serializer = JsonConvert.SerializeObject(woodenShoot.Serialize());
        Assert.NotNull(serializer);
        var deserialize = JsonConvert.DeserializeObject<CraftingMaterialDto>(serializer);

        var restoredMaterial = Restore(deserialize);
        Assert.NotNull(restoredMaterial);
        Assert.Equal(woodenShoot.Name, restoredMaterial.Name);
        Assert.Equal(woodenShoot.Description, restoredMaterial.Description);
        Assert.Equal(woodenShoot.Amount.Amount, restoredMaterial.Amount.Amount);
        Assert.Equal(woodenShoot.Count, restoredMaterial.Count);

        var ironScrap = IronScrap.FromAmount(1_000);
        var ironSerializer = JsonConvert.SerializeObject(ironScrap.Serialize());
        Assert.NotNull(ironSerializer);
        var ironDeserialize = JsonConvert.DeserializeObject<CraftingMaterialDto>(ironSerializer);
        var restoredIronMaterial = Restore(ironDeserialize);
        Assert.NotNull(restoredIronMaterial);
        Assert.Equal(ironScrap.Name, restoredIronMaterial.Name);
        Assert.Equal(ironScrap.Description, restoredIronMaterial.Description);
        Assert.Equal(ironScrap.Amount.Amount, restoredIronMaterial.Amount.Amount);
        Assert.Equal(ironScrap.Count, restoredIronMaterial.Count);
    }
}