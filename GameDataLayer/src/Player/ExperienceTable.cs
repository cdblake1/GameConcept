public class ExperienceTable
{
    public static ExperienceTable Default => new ExperienceTable(new Dictionary<int, int>
        {
            { 1, 0 },
            { 2, 100 },
            { 3, 300 },
            { 4, 600 },
            { 5, 1000 },
            { 6, 1500 },
            { 7, 2100 },
            { 8, 2800 },
            { 9, 3600 },
            { 10, 4500 },
            { 11, 5500 },
            { 12, 6600 },
            { 13, 7800 },
            { 14, 9100 },
            { 15, 10500 },
            { 16, int.MaxValue }
        });

    public Dictionary<int, int> Table { get; }

    private readonly int maxLevel;
    private readonly int minLevel;

    public ExperienceTable(Dictionary<int, int> experienceTable)
    {
        Table = experienceTable ?? throw new ArgumentNullException(nameof(experienceTable), "Experience table cannot be null.");
        maxLevel = experienceTable.Keys.Max();
        minLevel = experienceTable.Keys.Min();
    }

    public int GetCumulativeExperienceForLevel(int level)
    {
        if (level < minLevel || level > maxLevel)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be within the valid range.");
        }

        return Table.Where(entry => entry.Key <= level).Sum(entry => entry.Value);
    }

    public int GetLevelByExperience(int experience)
    {
        foreach (var entry in Table.OrderByDescending(e => e.Key))
        {
            if (experience >= entry.Value)
            {
                return entry.Key;
            }
        }

        return minLevel;
    }
}
