using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common
{
    public record ExpiresWithDto
    {
        [JsonProperty(nameof(source), Required = Required.Always)]
        public ExpiresWithSourceEnum source { get; init; }

        [JsonProperty(nameof(expires_with), Required = Required.Always)]
        public required string expires_with { get; init; }

        public enum ExpiresWithSourceEnum { skill, effect }
    }
}