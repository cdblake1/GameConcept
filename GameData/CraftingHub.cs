using GameData.CraftingItemTemplates;

public class CraftingHub
{
    public List<ICraftingRecipe> Recipes { get; set; }
    public CraftingHub()
    {
        Recipes = new List<ICraftingRecipe>()
        {
            new CraftingRecipesTemplates.IronChestRecipe(),
            new CraftingRecipesTemplates.IronHelmetRecipe(),
            new CraftingRecipesTemplates.IronLegsRecipe(),
            new CraftingRecipesTemplates.IronSwordRecipe(),
        };
    }
}