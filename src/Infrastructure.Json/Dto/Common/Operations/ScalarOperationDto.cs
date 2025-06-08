using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common.Operations;

public sealed record ScalarOperationDto(
    [JsonProperty] float scale_added,
    [JsonProperty] float scale_increased,
    [JsonProperty] float scale_empowered,
    [JsonProperty] float override_value
);

