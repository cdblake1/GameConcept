using Infrastructure.Json.Dto.Common.Operations;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common
{
    public record ScaleCoefficientDto
    {
        [JsonProperty(nameof(stat), Required = Required.Always)]
        public required StatDto stat { get; init; }

        [JsonProperty(nameof(scalar_operation))]
        public required ScalarOperationDto scalar_operation { get; init; }
    }
}