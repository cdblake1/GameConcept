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
            Assert.True(dto.apply_status is StatusBaseDto[]);
            Assert.True(dto.category is EffectBaseDto.Category.buff);
            Assert.True(dto.damage_types is not null && dto.damage_types is DamageTypeDto[]);
            Assert.True(dto.duration is DurationBaseDto);
            Assert.False(string.IsNullOrEmpty(dto.id));
            Assert.True(dto.leech is int);
            Assert.True(dto.modifiers is ModifierDto[]);
            Assert.True(dto.stacking is StackBaseDto);
        }
    }
}