using GameData.src.Shared;

namespace GameData.src.Item;

public sealed record CurrencyDefinition(
    string Id,
    PresentationDefinition Presentation,
    ItemRarity Rarity,
    int MinAmount,
    int MaxAmount
) : IItemDefinition;