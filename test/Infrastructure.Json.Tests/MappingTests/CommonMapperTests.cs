using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
using GameData.src.Shared.Modifiers;
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
            Assert.Equal(Duration.Kind.Permanent, pd.Type);
            Assert.True(ed.Expiry.Id == expireswithDurationDto.expires_with.expires_with);
            Assert.True(ed.Expiry.Source == Duration.ExpiresWith.Category.Skill);
        }

        [Fact]
        public void CanMapDurationOperationDto()
        {
            var turnsDto = new DurationOperationDto(new ScalarOperationDto(ScalarOperationDto.Operation.add, 1), null, null);
            var permDto = new DurationOperationDto(null, true, null);
            var expireswithDto = new DurationOperationDto(null, null, new() { expires_with = "test_effect", source = ExpiresWithDto.ExpiresWithSourceEnum.effect });

            var tDurOp = turnsDto.ToDomain();
            var pDurOp = permDto.ToDomain();
            var eDurOp = expireswithDto.ToDomain();

            Assert.NotNull(turnsDto.turns);
            Assert.NotNull(permDto.permanent);
            Assert.NotNull(expireswithDto.expires_with);

            Assert.NotNull(tDurOp.Turns);
            Assert.NotNull(pDurOp.Permanent);
            Assert.NotNull(eDurOp.ExpiresWith);

            Assert.Equal(Duration.Kind.Turns, tDurOp.Kind);
            Assert.Equal(Duration.Kind.Permanent, pDurOp.Kind);
            Assert.Equal(Duration.Kind.ExpiresWith, eDurOp.Kind);

            Assert.Equal(turnsDto.turns.value, tDurOp.Turns.Value);
            Assert.Equal(true, pDurOp.Permanent);
            Assert.Equal(Duration.ExpiresWith.Category.Effect, eDurOp.ExpiresWith.Value.Source);
            Assert.Equal(expireswithDto.expires_with.expires_with, eDurOp.ExpiresWith.Value.Id);

        }

        [Fact]
        public void CanMapModiferDto()
        {
            var statModDto = new ModifierDto(ModifierDto.ModifierKindDto.stat, new ScalarOperationDto(ScalarOperationDto.Operation.add, 1));
            var damageModDto = new ModifierDto(ModifierDto.ModifierKindDto.damage, new ScalarOperationDto(ScalarOperationDto.Operation.add, 1));
            var attackModDto = new ModifierDto(ModifierDto.ModifierKindDto.attack_type, new ScalarOperationDto(ScalarOperationDto.Operation.add, 1));
            var skillDto = new ModifierDto(ModifierDto.ModifierKindDto.skill, new ScalarOperationDto(ScalarOperationDto.Operation.add, 1));

            var statMod = statModDto.ToDomain();
            var damageMod = damageModDto.ToDomain();
            var attackMod = attackModDto.ToDomain();
            var skillMod = skillDto.ToDomain();

            Assert.Equal(statModDto.scalar_operation.value, statMod.Operation.Value);
            Assert.Equal(ScalarOperation.OperationKind.Add, statMod.Operation.ModifierOperation);

            Assert.Equal(damageModDto.scalar_operation.value, damageMod.Operation.Value);
            Assert.Equal(ScalarOperation.OperationKind.Add, damageMod.Operation.ModifierOperation);

            Assert.Equal(attackModDto.scalar_operation.value, attackMod.Operation.Value);
            Assert.Equal(ScalarOperation.OperationKind.Add, attackMod.Operation.ModifierOperation);

            Assert.Equal(skillDto.scalar_operation.value, skillMod.Operation.Value);
            Assert.Equal(ScalarOperation.OperationKind.Add, skillMod.Operation.ModifierOperation);
        }

        [Fact]
        public void CanMapStatCollectionOperationDto()
        {
            var statCollDto = new StatCollectionOperationDto(CollectionOperationDto.add, [StatDto.defense, StatDto.defense]);

            var statKindCollMod = statCollDto.ToDomain();

            Assert.Equal(statCollDto.items.Length, statKindCollMod.Operation.Items.Count);
            Assert.Equal(CollectionOperationKind.Add, statKindCollMod.Operation.Operation);
        }

        [Fact]
        public void CanMapAttackKindCollectionOperationDto()
        {
            var attackCollDto = new AttackCollectionOperationDto(CollectionOperationDto.add, [AttackKindDto.dot, AttackKindDto.hit]);

            var attackKindCollMod = attackCollDto.ToDomain();

            Assert.Equal(attackCollDto.items.Length, attackKindCollMod.Operation.Items.Count);
            Assert.Equal(CollectionOperationKind.Add, attackKindCollMod.Operation.Operation);
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
            var collOpDto = new ModifierCollectionOperationDto(CollectionOperationDto.add, [new ModifierDto(ModifierDto.ModifierKindDto.stat, new ScalarOperationDto(ScalarOperationDto.Operation.add, 1))]);

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