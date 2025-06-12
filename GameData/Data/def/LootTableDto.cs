using GameData;
using GameData.src.Item;

public record LootTableDto
{
    public required List<LootTableEntryDto> LootTableEntries { get; init; }
    public required bool AlwaysDropLoot { get; init; }

    public record LootTableEntryDto
    {
        public IItem Item { get; init; }
        public int Weight { get; init; }

        public LootTableEntryDto(IItem item, int weight)
        {
            Item = item;
            Weight = weight;
        }
    }
}