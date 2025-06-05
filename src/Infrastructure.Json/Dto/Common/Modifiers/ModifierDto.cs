using Infrastructure.Json.Dto.Common.Operations;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common.Modifiers
{
    public sealed record ModifierDto
    {
        [JsonProperty(nameof(type), Required = Required.Always)]
        public Kind type { get; }

        [JsonProperty(nameof(scalar_operation))]
        public ScalarOperationDto scalar_operation { get; }

        public ModifierDto(Kind type, ScalarOperationDto scalar_operation)
        {
            this.type = type;
            this.scalar_operation = scalar_operation;
        }

        public enum Kind { stat, damage, skill, attack_kind }
    }
}
