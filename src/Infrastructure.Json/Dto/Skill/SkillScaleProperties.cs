using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Skill
{
    public sealed record SkillScalePropertiesDto(
      [JsonProperty] int scale_added,
      [JsonProperty] float scale_increased,
      [JsonProperty] float scale_speed
    );
}