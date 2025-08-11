using GameData.src.Mob;
using Infrastructure.Json.Dto.Mob;

namespace Infrastructure.Json.Mappers
{
    public static class MobMapper
    {
        public static MobDefinition ToDomain(this MobDto dto)
        {
            return new MobDefinition(
                dto.id,
                dto.loot_table,
                dto.presentation.ToDomain(),
                dto.skills,
                dto.stats,
                dto.exp_table
            );
        }
    }
}