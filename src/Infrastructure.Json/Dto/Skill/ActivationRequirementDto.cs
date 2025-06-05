using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Skill
{
    public record ActivationRequirementDto
    {
        [JsonProperty(nameof(type), Required = Required.Always)]
        public required ActivationKind type { get; init; }

        [JsonProperty(nameof(count), Required = Required.Always)]
        public required int count { get; init; }

        [JsonProperty(nameof(id))]
        public string? id;

        public enum ActivationKind { effect_present, hp_pct_below }
    }
}