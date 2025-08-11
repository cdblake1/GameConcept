using GameData.src.Shared;
using GameData.src.Shared.Enums;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Common.Operations;
using Infrastructure.Json.Dto.Common.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.StatTemplate;

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
           GlobalModifierDto gm => gm.ToDomain(),
           DamageModifierDto dm => dm.ToDomain(),
           AttackModifierDto am => am.ToDomain(),
           WeaponModifierDto wm => wm.ToDomain(),
           _ => throw new NotImplementedException($"modifier not implemented: {dto.GetType().Name}")
       };

        public static SkillModifier ToDomain(this SkillModifierDto dto)
        => new(dto.skill_id, dto.scalar_op_type.ToDomain(), dto.value);
        public static GlobalModifier ToDomain(this GlobalModifierDto dto)
        => new(dto.stat.ToDomain(), dto.scalar_op_type.ToDomain(), dto.value);
        public static DamageModifier ToDomain(this DamageModifierDto dto)
        => new(dto.stat.ToDomain(), dto.scalar_op_type.ToDomain(), dto.value);
        public static AttackModifier ToDomain(this AttackModifierDto dto)
        => new(dto.stat.ToDomain(), dto.scalar_op_type.ToDomain(), dto.value);
        public static WeaponModifier ToDomain(this WeaponModifierDto dto)
        => new(dto.stat.ToDomain(), dto.scalar_op_type.ToDomain(), dto.value);

        public static ScalarOpType ToDomain(this ScalarOpTypeDto dto)
        => dto switch
        {
            ScalarOpTypeDto.added => ScalarOpType.Additive,
            ScalarOpTypeDto.increased => ScalarOpType.Increased,
            ScalarOpTypeDto.empowered => ScalarOpType.Empowered,
            _ => throw new InvalidOperationException($"ScalarOpType not implemented: {dto}")
        };

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
        => new()
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
        public static GlobalStat ToDomain(this GlobalStatDto dto)
        => dto switch
        {
            GlobalStatDto.armor => GlobalStat.Armor,
            GlobalStatDto.avoidance => GlobalStat.Avoidance,
            GlobalStatDto.crit => GlobalStat.Crit,
            GlobalStatDto.health => GlobalStat.Health,
            GlobalStatDto.speed => GlobalStat.Speed,
            GlobalStatDto.ward => GlobalStat.Ward,
            GlobalStatDto.leech => GlobalStat.Leech,
            _ => throw new InvalidOperationException($"GlobalStat not implemented: {dto}")
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

        public static StatTemplateDefinition ToDomain(this StatTemplateDto dto)
        => new(dto.Id, dto.global.ToDomain(), dto.damage.ToDomain(), dto.attack.ToDomain(), dto.weapon.ToDomain());

        public static IReadOnlyList<GlobalModifier> ToDomain(this GlobalModifierDto[] dto)
        => [.. dto.Select(g => g.ToDomain())];

        public static IReadOnlyList<DamageModifier> ToDomain(this DamageModifierDto[] dto)
        => [.. dto.Select(d => d.ToDomain())];

        public static IReadOnlyList<AttackModifier> ToDomain(this AttackModifierDto[] dto)
        => [.. dto.Select(a => a.ToDomain())];

        public static IReadOnlyList<WeaponModifier> ToDomain(this WeaponModifierDto[] dto)
        => [.. dto.Select(w => w.ToDomain())];

        public static IReadOnlyList<SkillModifier> ToDomain(this SkillModifierDto[] dto)
        => [.. dto.Select(s => s.ToDomain())];
    }
}