using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Modifiers;
using Infrastructure.Json.Dto.Effect;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class EffectDtoTests
    {
        public static readonly string EffectFilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "test_effect.test.effect.json");

        [Fact]
        public void CanSerializeEffectDto()
        {
            var json = File.ReadAllText(EffectFilePath);
            var dto = JsonConvert.DeserializeObject<EffectBaseDto>(json);

            Assert.True(dto is not null);
            Assert.True(dto.category is EffectCategoryDto.buff);

            var duration = dto.duration as TurnsDurationDto;
            Assert.NotNull(duration);
            Assert.Equal(1, duration.turns);

            Assert.False(string.IsNullOrEmpty(dto.id));

            Assert.NotNull(dto.modifiers?.Length);
            Assert.Equal(3, dto.modifiers.Length);

            var leech = dto.modifiers[0] as StatModifierDto;
            Assert.NotNull(leech);
            Assert.Equal(StatDto.melee_leech_added, leech.stat);
            Assert.Equal(1, leech.value);

            var physDamageEmpowered = dto.modifiers[1] as StatModifierDto;
            Assert.NotNull(physDamageEmpowered);
            Assert.Equal(StatDto.physical_damage_empowered, physDamageEmpowered.stat);
            Assert.Equal(1, physDamageEmpowered.value);

            var hitDamageInc = dto.modifiers[2] as StatModifierDto;
            Assert.NotNull(hitDamageInc);
            Assert.Equal(StatDto.hit_damage_increased, hitDamageInc.stat);
            Assert.Equal(1, hitDamageInc.value);
        }
    }
}