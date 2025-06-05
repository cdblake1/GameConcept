using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Class
{
    public record TalentNodeDto
    {
        [JsonProperty(nameof(talent_tier), Required = Required.Always)]
        public required Tier talent_tier { get; init; }

        [JsonProperty(nameof(id), Required = Required.Always)]
        public required string id { get; init; }

        [JsonProperty(nameof(prerequisites))]
        public required string[] prerequisites { get; init; }

        public enum Tier
        {
            tier_05,
            tier_10,
            tier_15,
            tier_20,
            tier_25,
            tier_30,
            tier_35,
            tier_40,
            tier_45,
            tier_50
        }
    }
}