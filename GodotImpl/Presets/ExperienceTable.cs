using System;

namespace TopDownGame.Presets;

internal static class ExperienceTable
{
		public static readonly int[] Levels = new int[]
		{
				0,
				100,
				300,
				600,
				1000,
				1500,
				2100,
				2800,
				3600,
				4500
		};

		public static int GetExperienceForLevel(int level)
		{
				if (level < 1 || level > Levels.Length)
				{
						throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and " + Levels.Length);
				}
				return Levels[level - 1];
		}

		public static int GetCumulativeExperienceUpToLevel(int level)
		{
				if (level < 1 || level > Levels.Length)
				{
						throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and " + Levels.Length);
				}

				int cumulativeExperience = 0;
				for (int i = 0; i < level; i++)
				{
						cumulativeExperience += Levels[i];
				}
				return cumulativeExperience;
		}

		public static int GetLevelForCumulativeExperience(double experience)
		{
				int cumulativeExperience = 0;
				for (int level = 1; level <= Levels.Length; level++)
				{
						cumulativeExperience += Levels[level - 1];
						if (experience < cumulativeExperience)
						{
								return level == 0 ? 0 : level - 1;
						}
				}

				return Levels.Length;
		}
}
