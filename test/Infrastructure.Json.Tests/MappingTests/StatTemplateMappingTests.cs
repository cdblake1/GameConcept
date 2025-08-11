using GameData.src.Shared.Enums;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Mappers;
using Infrastructure.Json.Dto.Common.Modifiers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class StatTemplateMappingTests
    {
        [Fact]
        public void CanMapStatTemplateDtoToDomain()
        {
            // Arrange
            var dto = new StatTemplateDto(
                Id: "test_stats",
                global: new[] { new GlobalModifierDto(GlobalStatDto.health, ScalarOpTypeDto.added, 100) },
                damage: new[] { new DamageModifierDto(DamageTypeDto.physical, ScalarOpTypeDto.added, 100) },
                attack: Array.Empty<AttackModifierDto>(),
                weapon: Array.Empty<WeaponModifierDto>()
            );

            // Act
            var domain = dto.ToDomain();

            // Assert
            Assert.NotNull(domain);
            Assert.Equal("test_stats", domain.Id);
            Assert.Single(domain.Global);
            Assert.Equal(GlobalStat.Health, domain.Global[0].GlobalStat);
            Assert.Single(domain.Damage);
            Assert.Equal(DamageType.Physical, domain.Damage[0].DamageType);
            Assert.Empty(domain.Attack);
            Assert.Empty(domain.Weapon);
        }

        [Fact]
        public void CanMapStatTemplateDtoWithAllStatsToDomain()
        {
            // Arrange
            var dto = new StatTemplateDto(
                Id: "test_stats_full",
                global: new[] {
                    new GlobalModifierDto(GlobalStatDto.health, ScalarOpTypeDto.added, 100),
                    new GlobalModifierDto(GlobalStatDto.armor, ScalarOpTypeDto.added, 100),
                    new GlobalModifierDto(GlobalStatDto.ward, ScalarOpTypeDto.added, 100),
                    new GlobalModifierDto(GlobalStatDto.avoidance, ScalarOpTypeDto.added, 100),
                    new GlobalModifierDto(GlobalStatDto.crit, ScalarOpTypeDto.added, 100),
                    new GlobalModifierDto(GlobalStatDto.speed, ScalarOpTypeDto.added, 100),
                    new GlobalModifierDto(GlobalStatDto.leech, ScalarOpTypeDto.added, 100)
                },
                damage: new[] {
                    new DamageModifierDto(DamageTypeDto.physical, ScalarOpTypeDto.added, 100),
                    new DamageModifierDto(DamageTypeDto.elemental, ScalarOpTypeDto.added, 100),
                    new DamageModifierDto(DamageTypeDto.nature, ScalarOpTypeDto.added, 100),
                    new DamageModifierDto(DamageTypeDto.bleed, ScalarOpTypeDto.added, 100),
                    new DamageModifierDto(DamageTypeDto.burn, ScalarOpTypeDto.added, 100),
                    new DamageModifierDto(DamageTypeDto.poison, ScalarOpTypeDto.added, 100),
                    new DamageModifierDto(DamageTypeDto.true_damage, ScalarOpTypeDto.added, 100)
                },
                attack: new[] {
                    new AttackModifierDto(AttackTypeDto.hit, ScalarOpTypeDto.added, 100),
                    new AttackModifierDto(AttackTypeDto.dot, ScalarOpTypeDto.added, 100)
                },
                weapon: new[] {
                    new WeaponModifierDto(WeaponTypeDto.melee, ScalarOpTypeDto.added, 100),
                    new WeaponModifierDto(WeaponTypeDto.range, ScalarOpTypeDto.added, 100),
                    new WeaponModifierDto(WeaponTypeDto.spell, ScalarOpTypeDto.added, 100)
                }
            );

            // Act
            var domain = dto.ToDomain();

            // Assert
            Assert.NotNull(domain);
            Assert.Equal("test_stats_full", domain.Id);
            Assert.Equal(7, domain.Global.Count);
            Assert.Equal(7, domain.Damage.Count);
            Assert.Equal(2, domain.Attack.Count);
            Assert.Equal(3, domain.Weapon.Count);

            // Verify specific mappings
            Assert.Equal(GlobalStat.Health, domain.Global[0].GlobalStat);
            Assert.Equal(GlobalStat.Armor, domain.Global[1].GlobalStat);
            Assert.Equal(DamageType.Physical, domain.Damage[0].DamageType);
            Assert.Equal(DamageType.Elemental, domain.Damage[1].DamageType);
            Assert.Equal(AttackType.Hit, domain.Attack[0].AttackType);
            Assert.Equal(AttackType.Dot, domain.Attack[1].AttackType);
            Assert.Equal(WeaponType.Melee, domain.Weapon[0].WeaponType);
            Assert.Equal(WeaponType.Range, domain.Weapon[1].WeaponType);
            Assert.Equal(WeaponType.Spell, domain.Weapon[2].WeaponType);
        }

        [Fact]
        public void ThrowsOnInvalidEnumValue()
        {
            // Arrange
            var dto = new StatTemplateDto(
                Id: "test_stats",
                global: new[] { new GlobalModifierDto((GlobalStatDto)999, ScalarOpTypeDto.added, 100) }, // Invalid enum value
                damage: new[] { new DamageModifierDto(DamageTypeDto.physical, ScalarOpTypeDto.added, 100) },
                attack: Array.Empty<AttackModifierDto>(),
                weapon: Array.Empty<WeaponModifierDto>()
            );

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dto.ToDomain());
        }
    }
}