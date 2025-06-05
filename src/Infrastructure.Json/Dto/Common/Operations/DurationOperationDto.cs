using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common.Operations
{
    public sealed record DurationOperationDto : IValidatableEntity
    {
        [JsonProperty(nameof(turns))]
        public ScalarOperationDto? turns { get; }

        [JsonProperty(nameof(permanent))]
        public bool? permanent { get; }

        [JsonProperty(nameof(expires_with))]
        public ExpiresWithDto? expires_with { get; }

        public DurationOperationDto(ScalarOperationDto? turns, bool? permananent, ExpiresWithDto? expires_with)
        {
            this.turns = turns;
            this.permanent = permananent;
            this.expires_with = expires_with;
        }

        public bool IsTurns()
        {
            return turns is not null;
        }

        public bool IsPermanent()
        {
            return permanent == true;
        }

        public bool IsExpiresWith()
        {
            return expires_with is not null;
        }

        public bool Validate()
        {
            if (turns is not null && expires_with is null && permanent is null)
            {
                return true;
            }
            else if (permanent == true && turns is null && expires_with is null)
            {
                return true;
            }
            else if (expires_with is not null && permanent is null && turns is null)
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