using GameData.src.Class;
using Infrastructure.Json.Dto.Class;

namespace Infrastructure.Json.Mappers
{
    public static class ClassMappers
    {
        public static ClassDefinition ToDomain(this ClassDto dto)
        {
            return new ClassDefinition(dto.id, dto.talents.ToDomain(), dto.skills.ToDomain());
        }

        public static SkillEntry ToDomain(this ClassDto.SkillEntryDto dto)
        {
            return new SkillEntry(dto.skill, dto.level);
        }

        public static TalentNode[] ToDomain(this TalentNodeDto[] nodeDtos)
        {
            TalentNode[]? nodes = null;
            for (var i = 0; i < nodeDtos.Length; i++)
            {
                nodes ??= new TalentNode[nodeDtos.Length];
                nodes[i] = nodeDtos[i].ToDomain();
            }

            return nodes ?? Array.Empty<TalentNode>();
        }

        public static SkillEntry[] ToDomain(this ClassDto.SkillEntryDto[] entryDtos)
        {
            SkillEntry[]? entries = null;
            for (var i = 0; i < entryDtos.Length; i++)
            {
                entries ??= new SkillEntry[entryDtos.Length];
                entries[i] = entryDtos[i].ToDomain();
            }

            return entries ?? Array.Empty<SkillEntry>();
        }

        public static TalentNode ToDomain(this TalentNodeDto dto)
        {
            return new TalentNode(dto.id, dto.talent_tier.ToDomain(), dto.prerequisites);
        }

        public static TalentTier ToDomain(this TalentNodeDto.Tier dto)
        {
            return dto switch
            {
                TalentNodeDto.Tier.tier_05 => TalentTier.Tier05,
                TalentNodeDto.Tier.tier_10 => TalentTier.Tier10,
                TalentNodeDto.Tier.tier_15 => TalentTier.Tier15,
                TalentNodeDto.Tier.tier_20 => TalentTier.Tier20,
                TalentNodeDto.Tier.tier_25 => TalentTier.Tier25,
                TalentNodeDto.Tier.tier_30 => TalentTier.Tier30,
                TalentNodeDto.Tier.tier_35 => TalentTier.Tier35,
                TalentNodeDto.Tier.tier_40 => TalentTier.Tier40,
                TalentNodeDto.Tier.tier_45 => TalentTier.Tier45,
                TalentNodeDto.Tier.tier_50 => TalentTier.Tier50,
                _ => throw new ArgumentOutOfRangeException(nameof(dto), dto, null)
            };
        }
    }
}