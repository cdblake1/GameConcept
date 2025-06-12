using GameData.src.Shared;

namespace GameData.src.Item
{
    public sealed record GoldCoin(
        string Id,
        PresentationDefinition Presentation
    ) : IItem;
}