using GameData.src.Encounter;
using Infrastructure.Json.Dto.Encounter;

namespace Infrastructure.Json.Mappers
{
    public static class EncounterMapper
    {
        public static EncounterDefinition ToDomain(this EncounterDto dto)
        {
            return new EncounterDefinition(
                dto.id,
                dto.duration.ToDomain(),
                [.. dto.mob_weights.Select(m => m.ToDomain())],
                dto.loot_table,
                dto.min_level,
                dto.presentation.ToDomain(),
                dto.boss_encounter);
        }

        public static EncounterDuration ToDomain(this EncounterDurationDto dto)
        {
            return new EncounterDuration(dto.min, dto.max);
        }

        public static EncounterMobWeight ToDomain(this EncounterMobWeightDto dto)
        {
            return new EncounterMobWeight(dto.mob_id, dto.weight);
        }
    }
}