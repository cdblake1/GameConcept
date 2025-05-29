using static CraftingMaterialTemplates;

namespace GameData.CraftingItemTemplates
{
    public static class CraftingRecipesTemplates
    {
        public class IronHelmetRecipe : ICraftingRecipe
        {
            public IItem CraftedItem { get; } = new CraftedEquipmentTemplates.IronHelmet();
            public List<IItem> RequiredMaterials { get; } = new()
            {
                IronScrap.FromAmount(5),
                TatteredCloth.FromAmount(2)
            };

            public int CraftingTime { get; } = 2;

            public IronHelmetRecipe()
            {

            }
        }

        public class IronChestRecipe : ICraftingRecipe
        {
            public IItem CraftedItem { get; } = new CraftedEquipmentTemplates.IronChest();
            public List<IItem> RequiredMaterials { get; } = new()
            {
                IronScrap.FromAmount(10),
                TatteredCloth.FromAmount(5)
            };

            public int CraftingTime { get; } = 2;

            public IronChestRecipe()
            {

            }
        }

        public class IronSwordRecipe : ICraftingRecipe
        {
            public IItem CraftedItem { get; } = new CraftedEquipmentTemplates.IronSword();

            public List<IItem> RequiredMaterials { get; } = new()
            {
                IronScrap.FromAmount(7),
                WoodenShoot.FromAmount(3)
            };

            public int CraftingTime { get; } = 2;

            public IronSwordRecipe()
            {

            }
        }

        public class IronLegsRecipe : ICraftingRecipe
        {
            public IItem CraftedItem { get; } = new CraftedEquipmentTemplates.IronLegs();
            public List<IItem> RequiredMaterials { get; } = new()
            {
                IronScrap.FromAmount(7),
                TatteredCloth.FromAmount(3)
            };

            public int CraftingTime { get; } = 2;

            public IronLegsRecipe()
            {

            }
        }
    }
}
