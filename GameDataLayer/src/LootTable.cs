using GameDataLayer;

public class LootTable
{
    public List<LootTableEntry> LootTableEntries { get; }
    public bool AlwaysDropLoot { get; }

    public LootTable(List<LootTableEntry> lootTableEntries, bool alwaysDropLoot = false)
    {
        LootTableEntries = lootTableEntries;
        AlwaysDropLoot = alwaysDropLoot;
    }

    public LootTableEntry? GetRandomLootEntry()
    {
        if (LootTableEntries.Count == 0) return null;

        int totalWeight = 0;
        foreach (var entry in LootTableEntries)
        {
            totalWeight += entry.Weight;
        }

        // Check if AlwaysDropLoot is true, skip adding "no loot" weight
        if (!AlwaysDropLoot)
        {
            // Add a "no loot" weight equal to 30% of the total weight
            int noLootWeight = (int)(totalWeight * (30.0 / 70.0));
            totalWeight += noLootWeight;
        }

        // Generate a random value within the total weight
        Random random = new Random();
        int randomValue = random.Next(0, totalWeight); // Random value between 0 and totalWeight

        foreach (var entry in LootTableEntries)
        {
            if (randomValue < entry.Weight)
            {
                return entry;
            }
            randomValue -= entry.Weight;
        }

        // If the random value falls into the "no loot" range, return null
        return null;
    }

    public readonly record struct LootTableEntry
    {
        public Equipment Item { get; init; }
        public int Weight { get; init; }

        public LootTableEntry(Equipment item, int weight)
        {
            Item = item;
            Weight = weight;
        }
    }
}