using GameDataLayer;
using static CraftingMaterialTemplates;

namespace GameDataLayerTests
{
    public class InventoryTests
    {
        private PlayerTemplate.Player CreateTestCharacter() => new("TestCharacter");

        [Fact]
        public void AddItemToCharacter_AddsEquipment()
        {
            var character = CreateTestCharacter();
            var sword = ItemTemplates.SwordOfMight;

            character.Inventory.AddItem(sword);

            Assert.Contains(sword, character.Inventory.Equipment);
            Assert.Single(character.Inventory.Equipment);
        }

        [Fact]
        public void AddCraftingMaterialToCharacter_AddsMaterial()
        {
            var character = CreateTestCharacter();
            var ironScrap = IronScrap.FromRange(1, 2);

            character.Inventory.AddItem(ironScrap);

            Assert.Contains(ironScrap, character.Inventory.CraftingMaterials);
        }

        [Fact]
        public void RemoveItemFromCharacter_RemovesEquipment()
        {
            var character = CreateTestCharacter();
            var sword = ItemTemplates.SwordOfMight;
            character.Inventory.AddItem(sword);

            character.Inventory.RemoveItem(sword);

            Assert.DoesNotContain(sword, character.Inventory.Equipment);
        }

        [Fact]
        public void RemoveMaterialFromCharacter_RemovesMaterial()
        {
            var character = CreateTestCharacter();
            var ironScrap = IronScrap.FromRange(1, 2);
            Assert.True(character.Inventory.AddItem(ironScrap));

            Assert.True(character.Inventory.RemoveItem(ironScrap));

            Assert.DoesNotContain(ironScrap, character.Inventory.CraftingMaterials);
        }

        [Fact]
        public void RemoveMaterialThatDoesNotExist_ReturnsFalse()
        {
            var character = CreateTestCharacter();
            var ironScrap = IronScrap.FromRange(1, 2);

            Assert.False(character.Inventory.RemoveItem(ironScrap));
        }

        [Fact]
        public void RemoveMaterialWithDifferentType_ReturnsFalse()
        {
            var character = CreateTestCharacter();
            var ironScrap = IronScrap.FromRange(1, 2);
            var woodenShoot = WoodenShoot.FromRange(1, 2);
            character.Inventory.AddItem(ironScrap);

            Assert.False(character.Inventory.RemoveItem(woodenShoot));
        }
    }
}
