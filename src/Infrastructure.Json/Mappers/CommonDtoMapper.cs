using GameData.src.Shared;
using GameData.src.Shared.Enums;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Operations;
using Infrastructure.Json.Dto.Common.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Shared.Modifiers;

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

        public static IModifier ToDomain(this IModifierDto dto)
       => dto switch
       {
           SkillModifierDto sm => sm.ToDomain(),
           StatModifierDto sm2 => sm2.ToDomain(),
           _ => throw new NotImplementedException($"modifier not implemented: {dto.GetType().Name}")
       };

        public static SkillModifier ToDomain(this SkillModifierDto dto)
        => new SkillModifier(dto.skill_id, dto.operation.ToDomain());

        public static StatModifier ToDomain(this StatModifierDto dto)
        => new StatModifier(dto.stat.ToDomain(), dto.value);


        public static StackDefaultOperation ToDomain(this StackDefaultModifierDto dto)
        => new()
        {
            Maxstacks = dto.max_stacks?.ToDomain(),
            StacksPerApplication = dto.stacks_per_application?.ToDomain()
        };

        public static WeaponType ToDomain(this WeaponTypeDto dto)
        => dto switch
        {
            WeaponTypeDto.melee => WeaponType.Melee,
            WeaponTypeDto.range => WeaponType.Range,
            WeaponTypeDto.spell => WeaponType.Spell,
            _ => throw new InvalidOperationException($"Weapon type not implemented: {dto}")
        };

        public static ScalarOperation ToDomain(this ScalarOperationDto dto)
        => new ScalarOperation()
        {
            ScaleAdded = dto.scale_added,
            ScaleEmpowered = dto.scale_empowered,
            ScaleIncreased = dto.scale_increased,
            OverrideValue = dto.override_value
        };

        public static StatusCollectionModifier ToDomain(this StatusCollectionOperationDto dto)
        => new(new([.. dto.items.Select(i => i.ToDomain())], dto.operation.ToDomain()));

        public static DamageTypeCollectionModifier ToDomain(this DamageTypeCollectionOperationDto dto)
        => new(new([.. dto.items.Select(i => i.ToDomain())], dto.operation.ToDomain()));

        public static ModifierCollectionModifier ToDomain(this ModifierCollectionOperationDto dto)
        => new(new([.. dto.items.Select(i => i.ToDomain())], dto.operation.ToDomain()));
        public static DamageType ToDomain(this DamageTypeDto damageType)
        {
            return damageType switch
            {
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

        public static AttackType ToDomain(this AttackTypeDto attackKind)
        {
            return attackKind switch
            {
                AttackTypeDto.dot => AttackType.Dot,
                AttackTypeDto.hit => AttackType.Hit,
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
                StatDto.physical_damage_added => StatKind.PhysicalDamageAdded,
                StatDto.physical_damage_increased => StatKind.PhysicalDamageIncreased,
                StatDto.physical_damage_empowered => StatKind.PhysicalDamageEmpowered,

                StatDto.elemental_damage_added => StatKind.ElementalDamageAdded,
                StatDto.elemental_damage_increased => StatKind.ElementalDamageIncreased,
                StatDto.elemental_damage_empowered => StatKind.ElementalDamageEmpowered,

                StatDto.nature_damage_added => StatKind.NatureDamageAdded,
                StatDto.nature_damage_increased => StatKind.NatureDamageIncreased,
                StatDto.nature_damage_empowered => StatKind.NatureDamageEmpowered,

                StatDto.bleed_damage_added => StatKind.BleedDamageAdded,
                StatDto.bleed_damage_increased => StatKind.BleedDamageIncreased,
                StatDto.bleed_damage_empowered => StatKind.BleedDamageEmpowered,

                StatDto.poison_damage_added => StatKind.PoisonDamageAdded,
                StatDto.poison_damage_increased => StatKind.PoisonDamageIncreased,
                StatDto.poison_damage_empowered => StatKind.PoisonDamageEmpowered,

                StatDto.burn_damage_added => StatKind.BurnDamageAdded,
                StatDto.burn_damage_increased => StatKind.BurnDamageIncreased,
                StatDto.burn_damage_empowered => StatKind.BurnDamageEmpowered,

                StatDto.hit_damage_added => StatKind.HitDamageAdded,
                StatDto.hit_damage_increased => StatKind.HitDamageIncreased,
                StatDto.hit_damage_empowered => StatKind.HitDamageEmpowered,

                StatDto.dot_damage_added => StatKind.DotDamageAdded,
                StatDto.dot_damage_increased => StatKind.DotDamageIncreased,
                StatDto.dot_damage_empowered => StatKind.DotDamageEmpowered,

                StatDto.melee_damage_added => StatKind.MeleeDamageAdded,
                StatDto.melee_damage_increased => StatKind.MeleeDamageIncreased,
                StatDto.melee_damage_empowered => StatKind.MeleeDamageEmpowered,

                StatDto.ranged_damage_added => StatKind.RangedDamageAdded,
                StatDto.ranged_damage_increased => StatKind.RangedDamageIncreased,
                StatDto.ranged_damage_empowered => StatKind.RangedDamageEmpowered,

                StatDto.spell_damage_added => StatKind.SpellDamageAdded,
                StatDto.spell_damage_increased => StatKind.SpellDamageIncreased,
                StatDto.spell_damage_empowered => StatKind.SpellDamageEmpowered,

                StatDto.armor_rating_added => StatKind.ArmorRatingAdded,
                StatDto.armor_rating_increased => StatKind.ArmorRatingIncreased,
                StatDto.armor_rating_empowered => StatKind.ArmorRatingEmpowered,

                StatDto.avoidance_rating_added => StatKind.AvoidanceRatingAdded,
                StatDto.avoidance_rating_increased => StatKind.AvoidanceRatingIncreased,
                StatDto.avoidance_rating_empowered => StatKind.AvoidanceRatingEmpowered,

                StatDto.ward_rating_added => StatKind.WardRatingAdded,
                StatDto.ward_rating_increased => StatKind.WardRatingIncreased,
                StatDto.ward_rating_empowered => StatKind.WardRatingEmpowered,

                StatDto.nature_resistance_added => StatKind.NatureResistanceAdded,
                StatDto.nature_resistance_increased => StatKind.NatureResistanceIncreased,

                StatDto.elemental_resistance_added => StatKind.ElementalResistanceAdded,
                StatDto.elemental_resistance_increased => StatKind.ElementalResistanceIncreased,

                StatDto.speed_rating_added => StatKind.SpeedRatingAdded,
                StatDto.speed_rating_increased => StatKind.SpeedRatingIncreased,
                StatDto.speed_rating_empowered => StatKind.SpeedRatingEmpowered,

                StatDto.health_rating_added => StatKind.HealthRatingAdded,
                StatDto.health_rating_increased => StatKind.HealthRatingIncreased,
                StatDto.health_rating_empowered => StatKind.HealthRatingEmpowered,

                StatDto.melee_leech_added => StatKind.MeleeLeechAdded,
                StatDto.melee_leech_increased => StatKind.MeleeLeechIncreased,

                StatDto.range_leech_added => StatKind.RangeLeechAdded,
                StatDto.range_leech_increased => StatKind.RangeLeechIncreased,

                StatDto.spell_leech_added => StatKind.SpellLeechAdded,
                StatDto.spell_leech_increased => StatKind.SpellLeechIncreased,

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
    }
}