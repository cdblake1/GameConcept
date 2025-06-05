using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Effect;

[Newtonsoft.Json.JsonConverter(typeof(StackBaseDtoConverter))]
[JsonDerivedType(typeof(StackDefaultDto), (int)StackKind.stack_default)]
[JsonDerivedType(typeof(StackFromEffectDto), (int)StackKind.stack_from_effect)]
public abstract record StackBaseDto
{
    [JsonProperty(nameof(type), Required = Required.Always)]
    public abstract StackKind type { get; }

    public enum StackKind { stack_default = 1, stack_from_effect = 2 }
}

public sealed record StackDefaultDto : StackBaseDto
{
    public override StackKind type => StackKind.stack_default;

    [JsonProperty(nameof(stacks_per_application), Required = Required.Always)]
    public required int stacks_per_application { get; init; }

    [JsonProperty(nameof(max_stacks), Required = Required.Always)]
    public required int max_stacks { get; init; }

    [JsonProperty(nameof(refresh_mode), Required = Required.Always)]
    public required RefreshMode refresh_mode { get; init; }

    public enum RefreshMode { add_time, reset_time, no_refresh }
}

public sealed record StackFromEffectDto : StackBaseDto
{
    public override StackKind type => StackKind.stack_from_effect;

    [JsonProperty(nameof(from), Required = Required.Always)]
    public required string from { get; init; }

    [JsonProperty(nameof(consume_stacks), Required = Required.Always)]
    public required bool consume_stacks { get; init; }
}

internal sealed class StackBaseDtoConverter : Newtonsoft.Json.JsonConverter<StackBaseDto>
{
    public override StackBaseDto? ReadJson(
        JsonReader reader,
        Type objectType,
        StackBaseDto? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);

        var typeToken = jo["type"]
                        ?? throw new JsonSerializationException("Stack object missing 'type'.");

        var typeStr = typeToken.Value<string>();
        return typeStr switch
        {
            "stack_default" => new StackDefaultDto
            {
                stacks_per_application = jo["stacks_per_application"]?.Value<int>()
                ?? throw new JsonSerializationException("Missing 'stacks_per_application'."),
                max_stacks = jo["max_stacks"]?.Value<int>()
                ?? throw new JsonSerializationException("Missing 'max_stacks'."),
                refresh_mode = Enum.TryParse<StackDefaultDto.RefreshMode>(
                jo["refresh_mode"]?.Value<string>(), out var refreshMode)
                ? refreshMode
                : throw new JsonSerializationException("Invalid or missing 'refresh_mode'.")
            },
            "stack_from_effect" => new StackFromEffectDto
            {
                from = jo["from"]?.Value<string>()
                ?? throw new JsonSerializationException("Missing 'from'."),
                consume_stacks = jo["consume_stacks"]?.Value<bool>()
                ?? throw new JsonSerializationException("Missing 'consume_stacks'.")
            },
            _ => throw new JsonSerializationException($"Unknown stack 'type' value '{typeStr}'.")
        };
    }

    public override void WriteJson(JsonWriter writer,
                                   StackBaseDto? value,
                                   JsonSerializer serializer)
    {
        // Concrete record already contains the correct fields; delegate to default serialisation.
        serializer.Serialize(writer, value);
    }
}