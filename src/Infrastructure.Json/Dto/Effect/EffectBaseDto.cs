using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Modifiers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Effect;

public record EffectBaseDto
{
    [JsonProperty(nameof(id), Required = Required.Always)]
    public required string id { get; init; }

    [JsonProperty(nameof(category), Required = Required.Always)]
    public required EffectCategoryDto category { get; init; }

    [JsonProperty(nameof(duration), Required = Required.Always)]
    public required DurationBaseDto duration { get; init; }

    [JsonProperty(nameof(stack_strategy))]
    public StackStrategyBaseDto? stack_strategy { get; init; }

    [JsonProperty(nameof(modifiers))]
    public IModifierDto[]? modifiers { get; init; }

    [JsonProperty(nameof(apply_status))]
    public StatusBaseDto[]? apply_status { get; init; }
}