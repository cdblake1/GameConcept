public class LevelManager
{
    private readonly int maxLevel;
    private readonly ExperienceTable experienceTable;

    public int CurrentLevel { get; private set; }
    public int NextLevel => CurrentLevel + 1;
    public int CurrentExperience { get; private set; }

    public int MaxLevel => maxLevel;

    public StatTemplate StatsPerLevel { get; private set; }

    public StatTemplate Stats => StatsPerLevel * (CurrentLevel - 1);

    public LevelManager(int maxLevel, ExperienceTable experienceTable, int startingLevel = 1)
    {
        if (maxLevel <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLevel), "Max level must be greater than 0.");
        }

        this.maxLevel = maxLevel;
        this.experienceTable = experienceTable;
        this.CurrentLevel = startingLevel;
        CurrentExperience = 0;
    }

    public bool AddExperience(int experience)
    {
        if (CurrentLevel >= maxLevel)
        {
            return false;
        }

        CurrentExperience = Math.Max(CurrentExperience, CurrentExperience + experience);
        int newLevel = experienceTable.GetLevelByExperience(CurrentExperience);

        if (newLevel > CurrentLevel)
        {
            CurrentLevel = Math.Min(newLevel, maxLevel);
            if (CurrentLevel == maxLevel)
            {
                CurrentExperience = experienceTable.GetCumulativeExperienceForLevel(maxLevel);
            }
            return true;
        }

        return false;
    }

    public int GetExperienceNeededForNextLevel()
    {
        return CurrentLevel >= maxLevel
            ? 0
            : experienceTable.GetCumulativeExperienceForLevel(CurrentLevel + 1);
    }
}