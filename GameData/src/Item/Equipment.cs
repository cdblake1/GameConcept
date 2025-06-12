using GameData.src.Shared;

namespace GameData.src.Item.Equipment
{
    public sealed record EquipmentDefinition(
        string Id,
        PresentationDefinition Presentation,
        EquipmentKind Kind
    ) : IItem;

    public enum EquipmentKind
    {
        Helmet,
        Necklace,
        Ring,
        Body,
        Legs,
        Gloves,
        Boots
    }
}