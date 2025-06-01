public class ExperienceTable
{
    public static ExperienceTable PlayerExpTable = new ExperienceTable(new()
    {
        {1, 0},
        {2, 1_000},
        {3, 4_063},
        {4, 4_158},
        {5, 4_289},
        {6, 4_457},
        {7, 4_667},
        {8, 4_924},
        {9, 5_233},
        {10, 5_603},
        {11, 6_042},
        {12, 6_564},
        {13, 7_182},
        {14, 7_915},
        {15, 8_785},
        {16, 9_819},
        {17, 11_053},
        {18, 12_527},
        {19, 14_297},
        {20, 16_430},
        {21, 19_009},
        {22, 22_142},
        {23, 25_966},
        {24, 30_653},
        {25, 36_428},
        {26, 43_576},
        {27, 52_470},
        {28, 63_590},
        {29, 77_565},
        {30, 95_221},
        {31, 117_644},
        {32, 146_270},
        {33, 183_010},
        {34, 230_414},
        {35, 291_906},
        {36, 372_100},
        {37, 477_246},
        {38, 615_850},
        {39, 799_542},
        {40, 1_044_299},
        {41, 1_372_180},
        {42, 1_813_776},
        {43, 2_411_724},
        {44, 3_225_728},
        {45, 4_339_794},
        {46, 5_872_689},
        {47, 7_993_126},
        {48, 10_941_924},
        {49, 15_064_470},
        {50, 20_858_497}
    });

    public static ExperienceTable MonsterExpTable = new ExperienceTable(new()
    {
        {1, 437},
        {2, 442},
        {3, 446},
        {4, 451},
        {5, 455},
        {6, 460},
        {7, 464},
        {8, 469},
        {9, 474},
        {10, 478},
        {11, 483},
        {12, 488},
        {13, 493},
        {14, 498},
        {15, 503},
        {16, 591},
        {17, 694},
        {18, 816},
        {19, 959},
        {20, 1_126},
        {21, 1_323},
        {22, 1_555},
        {23, 1_827},
        {24, 2_147},
        {25, 2_523},
        {26, 2_964},
        {27, 3_483},
        {28, 4_092},
        {29, 4_808},
        {30, 5_650},
        {31, 6_639},
        {32, 7_800},
        {33, 9_166},
        {34, 10_770},
        {35, 12_654},
        {36, 14_869},
        {37, 17_471},
        {38, 20_528},
        {39, 24_120},
        {40, 28_342},
        {41, 33_301},
        {42, 39_129},
        {43, 45_977},
        {44, 54_023},
        {45, 63_477},
        {46, 74_585},
        {47, 87_637},
        {48, 102_974},
        {49, 120_994},
        {50, 142_168}
    });

    private Dictionary<int, int> Table { get; }

    private readonly int maxLevel;
    private readonly int minLevel;

    public int MaxLevel => maxLevel;
    public int MinLevel => minLevel;

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

        return (int)Table.Where(e => e.Key <= level).Sum(e => e.Value);
    }

    public int GetLevelByExperience(int experience)
    {
        for (int level = maxLevel; level >= minLevel; level--)
        {
            if (experience >= GetCumulativeExperienceForLevel(level))
            {
                return level;
            }
        }

        return minLevel;
    }

    public int this[int level]
    {
        get
        {
            if (!Table.ContainsKey(level))
                throw new ArgumentOutOfRangeException(nameof(level), "Level not found in experience table.");
            return Table[level];
        }
    }
}
