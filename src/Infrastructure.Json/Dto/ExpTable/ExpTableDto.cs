using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.ExpTable
{
    public sealed record ExpTableDto(
        [JsonProperty] string id,
        [JsonProperty] ExpTableEntryDto[] table);

    public sealed record ExpTableEntryDto(
        [JsonProperty] int level,
        [JsonProperty] int experience);
}