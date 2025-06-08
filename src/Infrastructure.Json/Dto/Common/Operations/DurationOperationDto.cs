using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common.Operations
{
    public sealed record DurationOperationDto : IValidatableEntity
    {
        [JsonProperty(nameof(turns))]
        public ScalarOperationDto? turns { get; init; }

        [JsonProperty(nameof(permanent))]
        public bool? permanent { get; init; }

        [JsonProperty(nameof(expires_with))]
        public ExpiresWithDto? expires_with { get; init; }

        public bool IsTurns()
        {
            return this.turns is not null;
        }

        public bool IsPermanent()
        {
            return this.permanent == true;
        }

        public bool IsExpiresWith()
        {
            return this.expires_with is not null;
        }

        public bool Validate()
        {
            if (this.turns is not null && this.expires_with is null && this.permanent is null)
            {
                return true;
            }
            else if (this.permanent == true && this.turns is null && this.expires_with is null)
            {
                return true;
            }
            else if (this.expires_with is not null && this.permanent is null && this.turns is null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}