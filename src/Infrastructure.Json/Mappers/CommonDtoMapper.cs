using System.Collections.ObjectModel;
using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
using GameData.src.Shared.modifier;
using GameData.src.Shared.modifier.Operations;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.modifier;
using Infrastructure.Json.Dto.Common.Operations;
using Infrastructure.Json.Dto.Effect;

namespace Infrastructure.Json.Mappers
{
    internal static class CommonDtoMapper
    {
        public static Duration ToDomain(this DurationBaseDto dto)
        {
            return dto switch
            {
                TurnsDurationDto t => Duration.FromTurns(t.turns),
                PermanentDurationDto => Duration.Permanent(),
                ExpiresWithDurationDto e => e.expires_with.source switch
                {
                    ExpiresWithDto.ExpiresWithSourceEnum.effect => Duration.FromExpiry(new(Duration.ExpiresWith.Category.Effect, e.expires_with.expires_with)),
                    ExpiresWithDto.ExpiresWithSourceEnum.skill => Duration.FromExpiry(new(Duration.ExpiresWith.Category.Skill, e.expires_with.expires_with)),
                    _ => throw new InvalidOperationException("expires with category is invalid")
                },
                _ => throw new ArgumentOutOfRangeException(nameof(dto))
            };
        }

        public static DurationOperation ToDomain(this DurationOperationDto dto)
        {
            if (!dto.Validate())
            {
                throw new InvalidOperationException("Duration is invalid");
            }

            if (dto.IsPermanent())
            {
                return DurationOperation.FromPermanent(dto.permanent ?? throw new InvalidOperationException("Permanent duration value is missing in DurationOperationDto."));
            }
            else if (dto.IsExpiresWith())
            {
                return DurationOperation.FromExpiry(dto.expires_with?.ToDomain() ?? throw new InvalidOperationException("ExpiresWith value is missing in DurationOperationDto."));
            }
            else if (dto.IsTurns())
            {
                return DurationOperation.FromTurns(dto.turns?.ToDomain() ?? throw new InvalidOperationException("Turns value is missing in DurationOperationDto."));
            }
            else
            {
                throw new InvalidOperationException("DurationOperationDto does not match any known duration type");
            }
        }

        public static Duration.ExpiresWith ToDomain(this ExpiresWithDto dto)
        {
            return dto.source switch
            {
                ExpiresWithDto.ExpiresWithSourceEnum.effect => new Duration.ExpiresWith(Duration.ExpiresWith.Category.Effect, dto.expires_with),
                ExpiresWithDto.ExpiresWithSourceEnum.skill => new Duration.ExpiresWith(Duration.ExpiresWith.Category.Skill, dto.expires_with),
                _ => throw new InvalidOperationException($"Expires With Source category not implemented: {dto.source}")
            };
        }

        public static StatModifier ToDomain(this StatModifierDto dto)
        => new StatModifier(dto.stat.ToDomain(), dto.operation.ToDomain());

        public static AttackModifer ToDomain(this AttackModifierDto dto)
        => new AttackModifer(dto.attack_type.ToDomain(), dto.operation.ToDomain());

        public static SkillModifier ToDomain(this SkillModifierDto dto)
        => new SkillModifier(dto.skill_id, dto.operation.ToDomain());

        public static DamageModifer ToDomain(this DamageModifierDto dto)
        => new DamageModifer(dto.damage_type.ToDomain(), dto.operation.ToDomain());

        public static ScalarOperation ToDomain(this ScalarOperationDto dto)
        {
            return dto.operation switch
            {
                ScalarOperationDto.Operation.add => ScalarOperation.Create(ScalarOperation.OperationKind.Add, dto.value),
                ScalarOperationDto.Operation.mult => ScalarOperation.Create(ScalarOperation.OperationKind.Mult, dto.value),
                ScalarOperationDto.Operation.set => ScalarOperation.Create(ScalarOperation.OperationKind.Set, dto.value),
                _ => throw new InvalidOperationException("Invalid scalar modifier operation")
            };
        }
        public static ScalarModifierBase ToDomain(this ModifierDto dto)
        {
            return dto switch
            {
                StatModifierDto stat => stat.ToDomain(),
                AttackModifierDto attack => attack.ToDomain(),
                SkillModifierDto skill => skill.ToDomain(),
                DamageModifierDto damage => damage.ToDomain(),
                _ => throw new InvalidOperationException($"Unknown modifier type: {dto.type}")
            };
        }

        public static StatusCollectionModifier ToDomain(this StatusCollectionOperationDto dto)
        => new(new([.. dto.items.Select(i => i.ToDomain())], dto.operation.ToDomain()));

        public static DamageType ToDomain(this DamageTypeDto damageType)
        {
            return damageType switch
            {
                DamageTypeDto.melee => DamageType.Melee,
                DamageTypeDto.spell => DamageType.Spell,
                DamageTypeDto.physical => DamageType.Physical,
                DamageTypeDto.elemental => DamageType.Elemental,
                DamageTypeDto.nature => DamageType.Nature,
                DamageTypeDto.bleed => DamageType.Bleed,
                DamageTypeDto.burn => DamageType.Burn,
                DamageTypeDto.poison => DamageType.Poison,
                DamageTypeDto.true_damage => DamageType.TrueDamage,
                _ => throw new InvalidOperationException($"DamageKind not implemented: {damageType}")
            };
        }

        public static AttackKind ToDomain(this AttackKindDto attackKind)
        {
            return attackKind switch
            {
                AttackKindDto.dot => AttackKind.Dot,
                AttackKindDto.hit => AttackKind.Hit,
                _ => throw new InvalidOperationException($"AttackKind not implemented: {attackKind}")
            };
        }

        public static CollectionOperationKind ToDomain(this CollectionOperationDto dto)
        => dto switch
        {
            CollectionOperationDto.add => CollectionOperationKind.Add,
            CollectionOperationDto.remove => CollectionOperationKind.Remove,
            CollectionOperationDto.clear => CollectionOperationKind.Clear,
            CollectionOperationDto.set => CollectionOperationKind.Set,
            _ => throw new InvalidOperationException($"CollectionOperationKind not implemented: {dto}")
        };

        public static StatKind ToDomain(this StatDto stat) =>
            stat switch
            {
                StatDto.melee_damage => StatKind.MeleeDamage,
                StatDto.spell_damage => StatKind.SpellDamage,
                StatDto.physical_damage => StatKind.PhysicalDamage,
                StatDto.elemental_damage => StatKind.ElementalDamage,
                StatDto.speed => StatKind.Speed,
                StatDto.defense => StatKind.Defense,
                StatDto.health => StatKind.Health,
                StatDto.avoidance => StatKind.Avoidance,
                StatDto.ward => StatKind.Ward,
                _ => throw new InvalidOperationException($"StatKind not implemented: {stat}")
            };

        public static PresentationDefinition ToDomain(this PresentationDto dto)
        {
            return new PresentationDefinition()
            {
                Description = dto.description,
                Name = dto.name,
                Icon = dto.icon
            };
        }

        public static ScaleCoefficient ToDomain(this ScaleCoefficientDto dto)
        => new(dto.stat.ToDomain(), dto.scalar_operation.ToDomain());
    }
}