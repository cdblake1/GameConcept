using GameData.src.Shared;

namespace GameData.src.Item
{
    public sealed record CraftingMaterial(
        string Id,
        PresentationDefinition Presentation
    ) : IItem;
}