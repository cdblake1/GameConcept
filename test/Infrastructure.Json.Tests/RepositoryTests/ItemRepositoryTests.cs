using GameData.src.Item;
using GameData.src.Item.Equipment;
using Infrastructure.Json.Repositories;
using Xunit;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class ItemRepositoryTests
    {
        private const string testItemId = "test_item";
        private const string testEquipItemId = "test_equip_item";
        private readonly JsonItemRepository repository;
        public static readonly string ItemDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets");

        public ItemRepositoryTests()
        {
            this.repository = new JsonItemRepository(ItemDirectoryPath);
        }

        [Fact]
        public void CanLoadItemsIntoRepository()
        {
            var items = this.repository.GetAll();

            Assert.Contains(items, item => item.Id == testItemId);
            Assert.Contains(items, item => item.Id == testEquipItemId);

            var craftingItem = this.repository.Get(testItemId) as CraftingMaterialDefinition;
            var equipItem = this.repository.Get(testEquipItemId) as EquipmentDefinition;

            // Test crafting material item
            Assert.NotNull(craftingItem);
            Assert.Equal(testItemId, craftingItem.Id);
            Assert.Equal(ItemRarity.Uncommon, craftingItem.Rarity);
            Assert.Equal("Test Item", craftingItem.Presentation.Name);
            Assert.Equal("Test Description", craftingItem.Presentation.Description);

            // Test equipment item
            Assert.NotNull(equipItem);
            Assert.Equal(testEquipItemId, equipItem.Id);
            Assert.Equal(EquipmentKind.Body, equipItem.Kind);
            Assert.Equal(ItemRarity.Epic, equipItem.Rarity);
            Assert.Equal("Test Equip", equipItem.Presentation.Name);
            Assert.Equal("Test Equip Description", equipItem.Presentation.Description);
        }
    }
}