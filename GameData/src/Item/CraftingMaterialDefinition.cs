using GameData.src.Shared;

namespace GameData.src.Item
{
    public sealed record CraftingMaterialDefinition(
        string Id,
        PresentationDefinition Presentation,
        ItemRarity Rarity
    ) : IItemDefinition;
}