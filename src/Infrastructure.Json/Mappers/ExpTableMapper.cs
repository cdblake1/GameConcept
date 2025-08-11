using GameData.src.ExpTable;
using Infrastructure.Json.Dto.ExpTable;

namespace Infrastructure.Json.Mappers
{
    public static class ExpTableMapper
    {
        public static ExpTableDefinition ToDomain(this ExpTableDto dto)
        {
            return new ExpTableDefinition(dto.id, [.. dto.table.Select(ToDomain)]);
        }

        public static ExpTableEntryDefinition ToDomain(this ExpTableEntryDto dto)
        {
            return new ExpTableEntryDefinition(dto.level, dto.experience);
        }
    }
}