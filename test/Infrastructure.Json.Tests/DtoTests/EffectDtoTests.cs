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

            var leech = dto.modifiers[0] as GlobalModifierDto;
            Assert.NotNull(leech);
            Assert.Equal(GlobalStatDto.leech, leech.stat);
            Assert.Equal(1, leech.value);
            Assert.Equal(ScalarOpTypeDto.added, leech.scalar_op_type);

            var physDamageEmpowered = dto.modifiers[1] as DamageModifierDto;
            Assert.NotNull(physDamageEmpowered);
            Assert.Equal(DamageTypeDto.physical, physDamageEmpowered.stat);
            Assert.Equal(1, physDamageEmpowered.value);
            Assert.Equal(ScalarOpTypeDto.empowered, physDamageEmpowered.scalar_op_type);

            var hitDamageInc = dto.modifiers[2] as AttackModifierDto;
            Assert.NotNull(hitDamageInc);
            Assert.Equal(AttackTypeDto.hit, hitDamageInc.stat);
            Assert.Equal(1, hitDamageInc.value);
            Assert.Equal(ScalarOpTypeDto.empowered, hitDamageInc.scalar_op_type);
        }
    }
}