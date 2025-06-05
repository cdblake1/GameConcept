using Infrastructure.Json.Dto.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Effect;

[JsonConverter(typeof(StatusBaseDtoConverter))]
public abstract record StatusBaseDto
{
    [JsonProperty(nameof(id), Required = Required.Always)]
    public abstract StatusKind id { get; }

    public enum StatusKind { stun }
}

public sealed record StunStatusDto : StatusBaseDto
{
    public override StatusKind id => StatusKind.stun;

    [JsonProperty(nameof(duration), Required = Required.Always)]
    public required DurationBaseDto duration;
}

internal sealed class StatusBaseDtoConverter : JsonConverter<StatusBaseDto>
{
    public override StatusBaseDto? ReadJson(
        JsonReader reader, Type objectType, StatusBaseDto? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);

        var idToken = jo["id"] ?? throw new JsonSerializationException("Status object missing 'id'.");

        // We assume 'id' is a string in JSON. If you emit integers, convert accordingly.
        var id = idToken.Value<string>();

        return id switch
        {
            "stun" => new StunStatusDto()
            {
                duration = jo["duration"]?.ToObject<DurationBaseDto>()
                    ?? throw new JsonSerializationException("Failed to deserialize 'duration' for stun status."),
            },
            _ => throw new JsonSerializationException($"Unknown status id '{id}'.")
        };
    }

    public override void WriteJson(JsonWriter writer, StatusBaseDto? value, JsonSerializer serializer)
    {
        // Concrete record already has correct fields; default serialisation is fine.
        serializer.Serialize(writer, value);
    }
}