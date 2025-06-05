using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Json.Dto.Common;

[JsonConverter(typeof(DurationPolyConverter))]
public abstract record DurationBaseDto;

public sealed record TurnsDurationDto : DurationBaseDto
{
    [JsonProperty(nameof(turns), Required = Required.Always)]
    public required int turns { get; init; }
}

public sealed record PermanentDurationDto : DurationBaseDto
{
    [JsonProperty(nameof(permanent), Required = Required.Always)]
    public required bool permanent { get; init; }
}

public sealed record ExpiresWithDurationDto : DurationBaseDto
{
    [JsonProperty(nameof(expires_with), Required = Required.Always)]
    public required ExpiresWithDto expires_with { get; init; }
}

internal sealed class DurationPolyConverter : JsonConverter<DurationBaseDto>
{
    public override DurationBaseDto? ReadJson(
        JsonReader reader, Type objectType, DurationBaseDto? existing, bool _, JsonSerializer serializer)
    {
        var tok = JToken.Load(reader);

        return tok.Type switch
        {
            JTokenType.Integer =>
            new TurnsDurationDto
            {
                turns = tok.Value<int>()
            },

            JTokenType.String when tok.Value<string>() == "permanent" =>
            new PermanentDurationDto
            {
                permanent = true
            },

            JTokenType.Object => new ExpiresWithDurationDto
            {
                expires_with = (tok as JObject)?.ToObject<ExpiresWithDto>(serializer) ?? throw new JsonSerializationException("Failed to deserialize ExpiresWithDto from object token.")
            },

            _ => throw new JsonSerializationException(
             $"Unsupported duration token type: {tok.Type}")
        };
    }

    public override void WriteJson(JsonWriter writer, DurationBaseDto? value, JsonSerializer serializer)
    {
        switch (value)
        {
            case TurnsDurationDto t: writer.WriteValue(t.turns); break;
            case PermanentDurationDto: writer.WriteValue("permanent"); break;
            case ExpiresWithDurationDto e: serializer.Serialize(writer, e.expires_with); break;
            default:
                throw new JsonSerializationException($"Unknown DurationDto: {value?.GetType()}");
        }
    }
}



