using GameData.src.Effect;
using GameData.src.Effect.Stack;
using GameData.src.Effect.Status;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
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
        public void CanMapStatusDto()
        {
            var stunDto = new StunStatusDto { duration = new TurnsDurationDto() { turns = 1 } };

            var stunStatus = stunDto.ToDomain() as StunStatus;

            Assert.NotNull(stunStatus);

            Assert.Equal(1, stunStatus.Duration.Turns);
        }

        [Fact]
        public void CanMapStackBaseDto()
        {
            var sdDto = new StackDefaultDto() { max_stacks = 1, refresh_mode = StackDefaultDto.RefreshMode.add_time, stacks_per_application = 1 };
            var sfeDto = new StackFromEffectDto() { consume_stacks = true, from = "test_effect" };

            var stackDefault = sdDto.ToDomain() as StackDefault;
            var stackFromEffect = sfeDto.ToDomain() as StackFromEffect;

            Assert.NotNull(stackDefault);
            Assert.NotNull(stackFromEffect);

            Assert.Equal(sdDto.max_stacks, stackDefault.MaxStacks);
            Assert.Equal(StackRefreshMode.AddTime, stackDefault.RefreshMode);
            Assert.Equal(sdDto.stacks_per_application, stackDefault.StacksPerApplication);

            Assert.Equal(sfeDto.consume_stacks, stackFromEffect.ConsumeStacks);
            Assert.Equal(sfeDto.from, stackFromEffect.FromEffect);
        }

        [Fact]
        public void CanMapEffectBaseDto()
        {
            var efDto = new EffectBaseDto()
            {
                apply_status = [new StunStatusDto() { duration = new TurnsDurationDto() { turns = 1 } }],
                category = EffectBaseDto.Category.buff,
                duration = new TurnsDurationDto() { turns = 1 },
                id = "test_effect",
                damage_types = [DamageTypeDto.bleed, DamageTypeDto.burn],
                leech = 1,
                modifiers = [new ModifierDto(ModifierDto.Kind.stat, new(Dto.Common.Operations.ScalarOperationDto.Operation.add, 1))],
                stacking = new StackDefaultDto() { max_stacks = 1, refresh_mode = StackDefaultDto.RefreshMode.add_time, stacks_per_application = 1 }
            };

            var effectDefinition = efDto.ToDomain();

            Assert.NotNull(effectDefinition);

            Assert.Equal(EffectDefinition.Kind.Buff, effectDefinition.Category);
            Assert.Equal(1, effectDefinition.Duration.Turns);
            Assert.Equal("test_effect", effectDefinition.Id);

            Assert.NotNull(effectDefinition.ApplyStatus);
            Assert.Single(effectDefinition.ApplyStatus);
            Assert.IsType<StunStatus>(effectDefinition.ApplyStatus[0]);
            Assert.Equal(1, ((StunStatus)effectDefinition.ApplyStatus[0]).Duration.Turns);

            Assert.NotNull(effectDefinition.DamageTypes);
            Assert.Equal(2, effectDefinition.DamageTypes.Count);
            Assert.Contains(DamageType.Bleed, effectDefinition.DamageTypes);
            Assert.Contains(DamageType.Burn, effectDefinition.DamageTypes);

            Assert.Equal(1, effectDefinition.Leech);

            Assert.NotNull(effectDefinition.Modifiers);
            Assert.Single(effectDefinition.Modifiers);
            Assert.True(effectDefinition.Modifiers[0] is StatModifier);
            Assert.Equal(ScalarOperation.OperationKind.Add, effectDefinition.Modifiers[0].Operation.ModifierOperation);
            Assert.Equal(1, effectDefinition.Modifiers[0].Operation.Value);

            Assert.NotNull(effectDefinition.Stacking);
            Assert.IsType<StackDefault>(effectDefinition.Stacking);
            Assert.Equal(1, ((StackDefault)effectDefinition.Stacking).MaxStacks);
            Assert.Equal(StackRefreshMode.AddTime, ((StackDefault)effectDefinition.Stacking).RefreshMode);
            Assert.Equal(1, ((StackDefault)effectDefinition.Stacking).StacksPerApplication);
        }
    }
}