using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.ExpTable;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Mob
{
    public sealed record MobDto(
        [JsonProperty] string id,
        [JsonProperty] string loot_table,
        [JsonProperty] PresentationDto presentation,
        [JsonProperty] string[] skills,
        [JsonProperty] string stats,
        [JsonProperty] string exp_table);
}