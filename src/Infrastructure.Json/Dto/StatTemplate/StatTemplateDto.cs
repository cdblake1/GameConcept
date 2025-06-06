using Infrastructure.Json.Dto.Common;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.StatTemplate
{
    public sealed record StatTemplateDto
    {
        [JsonProperty(nameof(Stats))]
        public required StatValueDto[] Stats;
    }

    public sealed record StatValueDto(StatDto stat, int value);
}