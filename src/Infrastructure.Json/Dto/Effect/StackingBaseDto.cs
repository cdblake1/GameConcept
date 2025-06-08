using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Effect;

[Newtonsoft.Json.JsonConverter(typeof(StackBaseDtoConverter))]
[JsonDerivedType(typeof(StackDefaultDto), (int)StackKind.@default)]
[JsonDerivedType(typeof(StackFromEffectDto), (int)StackKind.from_effect)]
public abstract record StackStrategyBaseDto
{
    [JsonProperty(nameof(type), Required = Required.Always)]
    public abstract StackKind type { get; }

    public enum StackKind { @default = 1, from_effect = 2 }
}

public sealed record StackDefaultDto : StackStrategyBaseDto
{
    public override StackKind type => StackKind.@default;

    [JsonProperty(nameof(stacks_per_application), Required = Required.Always)]
    public required int stacks_per_application { get; init; }

    [JsonProperty(nameof(max_stacks), Required = Required.Always)]
    public required int max_stacks { get; init; }

    [JsonProperty(nameof(refresh_mode), Required = Required.Always)]
    public required StackStrategyRefreshMode refresh_mode { get; init; }

}

public sealed record StackFromEffectDto : StackStrategyBaseDto
{
    public override StackKind type => StackKind.from_effect;

    [JsonProperty(nameof(effect_id), Required = Required.Always)]
    public required string effect_id { get; init; }

    [JsonProperty(nameof(consume_stacks), Required = Required.Always)]
    public required bool consume_stacks { get; init; }
}

internal sealed class StackBaseDtoConverter : Newtonsoft.Json.JsonConverter<StackStrategyBaseDto>
{
    public override StackStrategyBaseDto? ReadJson(
        JsonReader reader,
        Type objectType,
        StackStrategyBaseDto? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);

        var typeToken = jo[nameof(StackStrategyBaseDto.type)]
                        ?? throw new JsonSerializationException($"Stack object missing '{nameof(StackStrategyBaseDto.type)}'.");

        var typeStr = typeToken.Value<string>();
        if (!Enum.TryParse<StackStrategyBaseDto.StackKind>(typeStr, out var stackKind))
        {
            throw new JsonSerializationException($"Unknown stack '{nameof(StackStrategyBaseDto.type)}' value '{typeStr}'.");
        }

        return stackKind switch
        {
            StackStrategyBaseDto.StackKind.@default => new StackDefaultDto
            {
                stacks_per_application = jo[nameof(StackDefaultDto.stacks_per_application)]?.Value<int>()
                ?? throw new JsonSerializationException($"Missing '{nameof(StackDefaultDto.stacks_per_application)}'."),
                max_stacks = jo[nameof(StackDefaultDto.max_stacks)]?.Value<int>()
                ?? throw new JsonSerializationException($"Missing '{nameof(StackDefaultDto.max_stacks)}'."),
                refresh_mode = Enum.TryParse<StackStrategyRefreshMode>(
                jo[nameof(StackDefaultDto.refresh_mode)]?.Value<string>(), out var refreshMode)
                ? refreshMode
                : throw new JsonSerializationException($"Invalid or missing '{nameof(StackDefaultDto.refresh_mode)}'.")
            },
            StackStrategyBaseDto.StackKind.from_effect => new StackFromEffectDto
            {
                effect_id = jo[nameof(StackFromEffectDto.effect_id)]?.Value<string>()
                ?? throw new JsonSerializationException($"Missing '{nameof(StackFromEffectDto.effect_id)}'."),
                consume_stacks = jo[nameof(StackFromEffectDto.consume_stacks)]?.Value<bool>()
                ?? throw new JsonSerializationException($"Missing '{nameof(StackFromEffectDto.consume_stacks)}'.")
            },
            _ => throw new JsonSerializationException($"Unknown stack '{nameof(StackStrategyBaseDto.type)}' value '{typeStr}'.")
        };
    }

    public override void WriteJson(JsonWriter writer,
                                   StackStrategyBaseDto? value,
                                   JsonSerializer serializer)
    {
        // Concrete record already contains the correct fields; delegate to default serialisation.
        serializer.Serialize(writer, value);
    }
}