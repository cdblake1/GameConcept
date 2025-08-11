namespace GameData.src.CraftingRecipe;

public sealed record CraftingRecipeDefinition(
    string Id,
    string CraftedItemId,
    IReadOnlyList<(string ItemId, int Count)> Materials
);