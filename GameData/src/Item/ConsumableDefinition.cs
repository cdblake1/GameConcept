using GameData.src.Shared;
using GameData.src.Shared.Modifiers;

namespace GameData.src.Item;

public sealed record ConsumableDefinition(
    string Id,
    PresentationDefinition Presentation,
    ItemRarity Rarity,
    IReadOnlyList<IModifier> Modifiers
) : IItemDefinition;