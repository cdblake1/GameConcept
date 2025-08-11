using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.LootTable
{
    public sealed record LootTableDto(
        [JsonProperty] string id,
        [JsonProperty] LootGroupDto[] groups
    );

    public sealed record LootGroupDto(
        [JsonProperty] LootEntryDto[] entries);

    public sealed record LootEntryDto(
        [JsonProperty] string item_id,
        [JsonProperty] int weight,
        [JsonProperty] bool? always);
}