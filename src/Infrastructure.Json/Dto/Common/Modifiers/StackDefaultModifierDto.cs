using Infrastructure.Json.Dto.Common.Operations;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common.Modifiers
{
    public sealed record StackDefaultModifierDto(
        [JsonProperty] ScalarOperationDto? stacks_per_application,
        [JsonProperty] ScalarOperationDto? max_stacks
    ) : IValidatableEntity
    {
        public bool Validate()
        {
            return this.stacks_per_application is not null || this.max_stacks is not null;
        }
    }
}