using Infrastructure.Json.Dto.Common.Modifiers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.Common
{
    public sealed record StatTemplateDto(
        [JsonProperty] string Id,
        [JsonProperty] GlobalModifierDto[] global,
        [JsonProperty] DamageModifierDto[] damage,
        [JsonProperty] AttackModifierDto[] attack,
        [JsonProperty] WeaponModifierDto[] weapon
    );
}