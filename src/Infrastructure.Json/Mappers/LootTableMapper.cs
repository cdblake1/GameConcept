using GameData.src.LootTable;
using Infrastructure.Json.Dto.LootTable;

namespace Infrastructure.Json.Mappers
{
    public static class TableMappers
    {
        public static LootTableDefinition ToDomain(this LootTableDto dto)
        {
            return new LootTableDefinition(dto.id, dto.groups.ToDomain());
        }

        public static LootGroupDefinition ToLootGroupDefinition(this LootGroupDto lootGroupDto)
        {
            return new LootGroupDefinition([.. lootGroupDto.entries.Select(ToLootEntryDefinition)]);
        }

        public static List<LootGroupDefinition> ToDomain(this LootGroupDto[] dto)
        {
            return [.. dto.Select(ToLootGroupDefinition)];
        }

        public static LootEntryDefinition ToLootEntryDefinition(this LootEntryDto lootEntryDto)
        {
            return new LootEntryDefinition(lootEntryDto.item_id, lootEntryDto.weight, lootEntryDto.always ?? false);
        }
    }
}