using GameData;

namespace GameDataTests
{
    public class ExperienceTests
    {
        [Fact]
        public void ExperienceGainsCorrectly()
        {
            // Arrange
            var character = new PlayerOld("TestCharacter");

            var initialExperience = character.LevelManager.CurrentExperience;

            Assert.Equal(0, initialExperience);
            Assert.Equal(1, character.LevelManager.CurrentLevel);
            var experienceGained = 50;

            // Act
            character.LevelManager.AddExperience(experienceGained);

            // Assert
            Assert.Equal(initialExperience + experienceGained, character.LevelManager.CurrentExperience);
        }

        [Fact]
        public void ExperienceGainsLevelUp()
        {
            // Arrange
            var character = new PlayerOld("TestCharacter");

            var initialExperience = character.LevelManager.CurrentExperience;

            Assert.Equal(0, initialExperience);
            Assert.Equal(1, character.LevelManager.CurrentLevel);
            var experienceGained = ExperienceTable.PlayerExpTable.GetCumulativeExperienceForLevel(2) - initialExperience;

            // Act
            character.LevelManager.AddExperience(experienceGained);

            // Assert
            Assert.Equal(initialExperience + experienceGained, character.LevelManager.CurrentExperience);
            Assert.Equal(2, character.LevelManager.CurrentLevel);

            var additionalExperience = ExperienceTable.PlayerExpTable.GetCumulativeExperienceForLevel(3) - character.LevelManager.CurrentExperience;
            character.LevelManager.AddExperience(additionalExperience);
            Assert.Equal(ExperienceTable.PlayerExpTable.GetCumulativeExperienceForLevel(3), character.LevelManager.CurrentExperience);

            Assert.Equal(3, character.LevelManager.CurrentLevel);
        }

        [Fact]
        public void ExperienceStopsAtMaxLevel()
        {
            // Arrange
            var character = new PlayerOld("TestCharacter");

            var initialExperience = character.LevelManager.CurrentExperience;

            Assert.Equal(0, initialExperience);
            Assert.Equal(1, character.LevelManager.CurrentLevel);
            var experienceGained = 1000000;

            // Act
            character.LevelManager.AddExperience(experienceGained);

            // Assert
            Assert.Equal(ExperienceTable.PlayerExpTable.GetCumulativeExperienceForLevel(character.LevelManager.MaxLevel), character.LevelManager.CurrentExperience);
        }

        [Fact]
        public void ExperienceCannotGoBelowMinLevelOrNegative()
        {
            // Arrange
            var character = new PlayerOld("TestCharacter");

            var initialExperience = character.LevelManager.CurrentExperience;

            Assert.Equal(0, initialExperience);
            Assert.Equal(1, character.LevelManager.CurrentLevel);
            var experienceGained = -100;

            // Act
            character.LevelManager.AddExperience(experienceGained);

            // Assert
            Assert.Equal(initialExperience, character.LevelManager.CurrentExperience);
            Assert.Equal(1, character.LevelManager.CurrentLevel);
            Assert.Equal(0, character.LevelManager.CurrentExperience);
        }

        [Fact]
        public void TotalExperienceToNextLevelWorks()
        {
            // Arrange
            var character = new PlayerOld("TestCharacter");

            var initialExperience = character.LevelManager.CurrentExperience;

            Assert.Equal(0, initialExperience);
            Assert.Equal(1, character.LevelManager.CurrentLevel);

            // Act
            var totalExperienceToNextLevel = character.LevelManager.GetExperienceNeededForNextLevel();

            // Assert
            Assert.Equal(ExperienceTable.PlayerExpTable[character.LevelManager.CurrentLevel + 1], totalExperienceToNextLevel);
        }
    }

    public class MockClass : IClass
    {
        public string Name { get; }
        public string Description { get; }
        public StatTemplateOld BaseStats { get; }

        public List<Talent> Talents => throw new NotImplementedException();

        public List<(int requiredLevel, Skill Skill)> SkillList => throw new NotImplementedException();

        public MockClass(string name, string description, StatTemplateOld baseStats)
        {
            Name = name;
            Description = description;
            BaseStats = baseStats;
        }

        public static MockClass Create()
        {
            return new MockClass("MockClass", "This is a mock class for testing.", new StatTemplateOld
            {
                Health = 100,
                AttackPower = 10,
                Defense = 5,
                Speed = 2
            });
        }
    }
}