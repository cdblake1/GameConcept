using GameData.src.Shared;

namespace GameData.src.Encounter
{
    public sealed record EncounterDefinition(
        string Id,
        EncounterDuration Duration,
        EncounterMobWeight[] MobWeights,
        string LootTable,
        int MinLevel,
        PresentationDefinition Presentation,
        bool BossEncounter
    );

    public sealed record EncounterDuration(
        int Min,
        int Max
    );

    public sealed record EncounterMobWeight(
        string MobId,
        int Weight
    );
}