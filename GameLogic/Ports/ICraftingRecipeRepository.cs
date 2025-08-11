using GameData.src.CraftingRecipe;

namespace GameLogic.Ports
{
    public interface ICraftingRecipeRepository
    {
        CraftingRecipeDefinition Get(string id);
        IReadOnlyList<CraftingRecipeDefinition> GetAll();
    }
}