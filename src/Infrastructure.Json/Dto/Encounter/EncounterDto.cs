using Infrastructure.Json.Dto.Common;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Encounter
{
    public sealed record EncounterDto(
        [JsonProperty] string id,
        [JsonProperty] EncounterDurationDto duration,
        [JsonProperty] EncounterMobWeightDto[] mob_weights,
        [JsonProperty] string loot_table,
        [JsonProperty] int min_level,
        [JsonProperty] PresentationDto presentation,
        [JsonProperty] bool boss_encounter
    );

    public sealed record EncounterDurationDto(
        [JsonProperty] int min,
        [JsonProperty] int max
    );

    public sealed record EncounterMobWeightDto(
        [JsonProperty] string mob_id,
        [JsonProperty] int weight
    );
}