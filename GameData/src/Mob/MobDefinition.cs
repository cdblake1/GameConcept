using GameData.src.Shared;

namespace GameData.src.Mob
{
    public sealed record MobDefinition(
        string Id,
        string LootTable,
        PresentationDefinition Presentation,
        string[] Skills,
        string Stats,
        string ExpTable
    );
}