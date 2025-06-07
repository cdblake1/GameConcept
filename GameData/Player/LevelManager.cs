namespace GameData;

public class LevelManager : IStateSerializable<LevelManager.LevelManagerDto, LevelManager>
{
    private readonly int maxLevel;
    private readonly ExperienceTable experienceTable;

    public int CurrentLevel { get; private set; }

    public int CurrentExperience { get; private set; }

    public int MaxLevel => maxLevel;

    public StatTemplateOld StatsPerLevel { get; private set; }

    public StatTemplateOld Stats => StatsPerLevel * (CurrentLevel - 1);

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

    public readonly struct LevelManagerDto
    {
        public int CurrentLevel { get; init; }
        public int CurrentExperience { get; init; }
        public int MaxLevel { get; init; }
        public StatTemplateOld StatsPerLevel { get; init; }
    }

    public LevelManagerDto Serialize()
    {
        return new LevelManagerDto
        {
            CurrentLevel = CurrentLevel,
            CurrentExperience = CurrentExperience,
            MaxLevel = maxLevel,
            StatsPerLevel = StatsPerLevel
        };
    }

    public static LevelManager Restore(LevelManagerDto dto)
    {
        if (dto.MaxLevel <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.MaxLevel), "Max level must be greater than 0.");
        }

        var experienceTable = ExperienceTable.PlayerExpTable;
        var levelManager = new LevelManager(dto.MaxLevel, experienceTable, dto.CurrentLevel)
        {
            CurrentExperience = dto.CurrentExperience,
            StatsPerLevel = dto.StatsPerLevel
        };

        return levelManager;
    }
}