using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers.Operations;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Modifiers;
using Infrastructure.Json.Dto.Common.Operations;
using Infrastructure.Json.Mappers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class CommonMapperTests
    {
        [Fact]
        public void CanMapDurationDto()
        {
            var turnsDurationDto = new TurnsDurationDto()
            {
                turns = 1
            };

            var permDurationDto = new PermanentDurationDto()
            {
                permanent = true
            };

            var expireswithDurationDto = new ExpiresWithDurationDto()
            {
                expires_with = new()
                {
                    expires_with = "test_effect",
                    source = ExpiresWithDto.ExpiresWithSourceEnum.skill
                }
            };

            var td = turnsDurationDto.ToDomain();
            var pd = permDurationDto.ToDomain();
            var ed = expireswithDurationDto.ToDomain();

            Assert.True(td.Turns == turnsDurationDto.turns);
            Assert.Equal(DurationKind.Permanent, pd.Kind);
            Assert.True(ed.Expiry.Id == expireswithDurationDto.expires_with.expires_with);
            Assert.True(ed.Expiry.Source == Duration.ExpiresWith.Category.Skill);
        }

        [Fact]
        public void CanMapDurationOperationDto()
        {
            var turnsDto = new DurationOperationDto()
            {
                turns = new(1, 1, 1, 1)
            };
            var permDto = new DurationOperationDto()
            {
                permanent = true
            };
            var expireswithDto = new DurationOperationDto()
            {
                expires_with = new() { expires_with = "test_effect", source = ExpiresWithDto.ExpiresWithSourceEnum.effect }
            };

            var tDurOp = turnsDto.ToDomain();
            var pDurOp = permDto.ToDomain();
            var eDurOp = expireswithDto.ToDomain();

            Assert.NotNull(turnsDto.turns);
            Assert.NotNull(permDto.permanent);
            Assert.NotNull(expireswithDto.expires_with);

            Assert.NotNull(tDurOp.Turns);
            Assert.NotNull(pDurOp.Permanent);
            Assert.NotNull(eDurOp.ExpiresWith);

            Assert.Equal(DurationKind.Turns, tDurOp.Kind);
            Assert.Equal(DurationKind.Permanent, pDurOp.Kind);
            Assert.Equal(DurationKind.ExpiresWith, eDurOp.Kind);

            Assert.Equal(turnsDto.turns.scale_added, tDurOp.Turns.ScaleAdded);
            Assert.Equal(true, pDurOp.Permanent);
            Assert.Equal(Duration.ExpiresWith.Category.Effect, eDurOp.ExpiresWith.Value.Source);
            Assert.Equal(expireswithDto.expires_with.expires_with, eDurOp.ExpiresWith.Value.Id);

        }

        [Fact]
        public void CanMapModiferDto()
        {
            var statModDto = new StatModifierDto(StatDto.armor_rating_added, 1);
            var skillModDto = new SkillModifierDto("test_skill", new(1, 0, 0, 0));

            var statMod = statModDto.ToDomain();
            var skillMod = skillModDto.ToDomain();

            Assert.Equal(StatKind.ArmorRatingAdded, statMod.StatKind);
            Assert.Equal(statModDto.value, statMod.Value);

            Assert.Equal("test_skill", skillMod.SkillId);
            Assert.Equal(1, skillMod.Operation.ScaleAdded);
            Assert.Equal(0, skillMod.Operation.ScaleIncreased);
            Assert.Equal(0, skillMod.Operation.ScaleEmpowered);
            Assert.Equal(0, skillMod.Operation.OverrideValue);
        }

        [Fact]
        public void CanMapDamageTypeCollectionOperationDto()
        {
            var damageTypeCollDto = new DamageTypeCollectionOperationDto(CollectionOperationDto.add, [DamageTypeDto.bleed, DamageTypeDto.elemental]);

            var damageTypeCollMod = damageTypeCollDto.ToDomain();

            Assert.Equal(damageTypeCollDto.items.Length, damageTypeCollMod.Operation.Items.Count);
            Assert.Equal(CollectionOperationKind.Add, damageTypeCollMod.Operation.Operation);
        }

        [Fact]
        public void CanMapModifierCollectionOperationDto()
        {
            var collOpDto = new ModifierCollectionOperationDto(
                CollectionOperationDto.add,
                [new StatModifierDto(StatDto.physical_damage_added, 1)]);

            var collModifierOp = collOpDto.ToDomain();

            Assert.NotNull(collModifierOp);

            Assert.Equal(collOpDto.items.Length, collModifierOp.Operation.Items.Count);

            Assert.Equal(CollectionOperationKind.Add, collModifierOp.Operation.Operation);
        }

        [Fact]
        public void CanMapPresentationDefinition()
        {
            var presDto = new PresentationDto() { description = "test descr", name = "test name", icon = "test icon" };

            var presDef = presDto.ToDomain();

            Assert.Equal(presDto.name, presDef.Name);
            Assert.Equal(presDto.description, presDef.Description);
            Assert.Equal(presDto.icon, presDef.Icon);
        }
    }
}