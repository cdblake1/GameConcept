namespace GameData.src.LootTable
{
    public sealed record LootTableDefinition(
        string Id,
        IReadOnlyList<LootGroupDefinition> Groups);
    public sealed record LootGroupDefinition(
        IReadOnlyList<LootEntryDefinition> Entries);
    public sealed record LootEntryDefinition(
        string ItemId,
        int Weight,
        bool Always);
}