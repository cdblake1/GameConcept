using GameData.src.Effect;
using GameData.src.Effect.Stack;
using GameData.src.Effect.Status;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Modifiers;
using Infrastructure.Json.Dto.Effect;
using Infrastructure.Json.Mappers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class EffectMapperTests
    {

        [Fact]
        public void CanMapStackBaseDto()
        {
            var sdDto = new StackDefaultDto() { max_stacks = 1, refresh_mode = StackStrategyRefreshMode.add_time, stacks_per_application = 1 };
            var sfeDto = new StackFromEffectDto() { consume_stacks = true, effect_id = "test_effect" };

            var stackDefault = sdDto.ToDomain() as StackDefault;
            var stackFromEffect = sfeDto.ToDomain() as StackFromEffect;

            Assert.NotNull(stackDefault);
            Assert.NotNull(stackFromEffect);

            Assert.Equal(sdDto.max_stacks, stackDefault.MaxStacks);
            Assert.Equal(StackRefreshMode.AddTime, stackDefault.RefreshMode);
            Assert.Equal(sdDto.stacks_per_application, stackDefault.StacksPerApplication);

            Assert.Equal(sfeDto.consume_stacks, stackFromEffect.ConsumeStacks);
            Assert.Equal(sfeDto.effect_id, stackFromEffect.EffectId);
        }

        [Fact]
        public void CanMapEffectBaseDto()
        {
            var efDto = new EffectBaseDto()
            {
                category = EffectCategoryDto.buff,
                duration = new TurnsDurationDto() { turns = 1 },
                id = "test_effect",
                modifiers = [new StatModifierDto(StatDto.physical_damage_added, 1)],
                stack_strategy = new StackDefaultDto()
                {
                    max_stacks = 1,
                    refresh_mode = StackStrategyRefreshMode.add_time,
                    stacks_per_application = 1
                }
            };

            var effectDefinition = efDto.ToDomain();

            Assert.NotNull(effectDefinition);

            Assert.Equal(EffectCategory.Buff, effectDefinition.Category);
            Assert.Equal(1, effectDefinition.Duration.Turns);
            Assert.Equal("test_effect", effectDefinition.Id);

            Assert.NotNull(effectDefinition.Modifiers);
            Assert.Single(effectDefinition.Modifiers);
            var statMod = effectDefinition.Modifiers[0] as StatModifier;
            Assert.NotNull(statMod);
            Assert.Equal(StatKind.PhysicalDamageAdded, statMod.StatKind);
            Assert.Equal(1, statMod.Value);

            Assert.NotNull(effectDefinition.StackStrategy);
            Assert.IsType<StackDefault>(effectDefinition.StackStrategy);
            Assert.Equal(1, ((StackDefault)effectDefinition.StackStrategy).MaxStacks);
            Assert.Equal(StackRefreshMode.AddTime, ((StackDefault)effectDefinition.StackStrategy).RefreshMode);
            Assert.Equal(1, ((StackDefault)effectDefinition.StackStrategy).StacksPerApplication);
        }
    }
}