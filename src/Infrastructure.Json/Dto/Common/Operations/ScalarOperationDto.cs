using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common.Operations;


public sealed record ScalarOperationDto
{
    [JsonProperty(nameof(operation), Required = Required.Always)]
    public Operation operation { get; }

    [JsonProperty(nameof(value), Required = Required.Always)]
    public int value { get; }

    public ScalarOperationDto(Operation operation, int value)
    {
        this.operation = operation;
        this.value = value;
    }

    public enum Operation { add, mult, set }
}
