using GameDataLayer;

namespace GameDataLayerTests
{
    public class ExperienceTests
    {
        [Fact]
        public void ExperienceGainsCorrectly()
        {
            // Arrange
            var character = new PlayerTemplate.Player("TestCharacter");

            var initialExperience = character.Level.CurrentExperience;

            Assert.Equal(0, initialExperience);
            Assert.Equal(1, character.Level.CurrentLevel);
            var experienceGained = 50;

            // Act
            character.Level.AddExperience(experienceGained);

            // Assert
            Assert.Equal(initialExperience + experienceGained, character.Level.CurrentExperience);
        }

        [Fact]
        public void ExperienceGainsLevelUp()
        {
            // Arrange
            var character = new PlayerTemplate.Player("TestCharacter");

            var initialExperience = character.Level.CurrentExperience;

            Assert.Equal(0, initialExperience);
            Assert.Equal(1, character.Level.CurrentLevel);
            var experienceGained = ExperienceTable.Default.GetCumulativeExperienceForLevel(2) - initialExperience;

            // Act
            character.Level.AddExperience(experienceGained);

            // Assert
            Assert.Equal(initialExperience + experienceGained, character.Level.CurrentExperience);
            Assert.Equal(2, character.Level.CurrentLevel);

            var additionalExperience = ExperienceTable.Default.GetCumulativeExperienceForLevel(3) - character.Level.CurrentExperience;
            character.Level.AddExperience(additionalExperience);
            Assert.Equal(ExperienceTable.Default.GetCumulativeExperienceForLevel(3), character.Level.CurrentExperience);

            Assert.Equal(3, character.Level.CurrentLevel);
        }

        [Fact]
        public void ExperienceStopsAtMaxLevel()
        {
            // Arrange
            var character = new PlayerTemplate.Player("TestCharacter");

            var initialExperience = character.Level.CurrentExperience;

            Assert.Equal(0, initialExperience);
            Assert.Equal(1, character.Level.CurrentLevel);
            var experienceGained = 1000000;

            // Act
            character.Level.AddExperience(experienceGained);

            // Assert
            Assert.Equal(ExperienceTable.Default.GetCumulativeExperienceForLevel(character.Level.MaxLevel), character.Level.CurrentExperience);
        }

        [Fact]
        public void ExperienceCannotGoBelowMinLevelOrNegative()
        {
            // Arrange
            var character = new PlayerTemplate.Player("TestCharacter");

            var initialExperience = character.Level.CurrentExperience;

            Assert.Equal(0, initialExperience);
            Assert.Equal(1, character.Level.CurrentLevel);
            var experienceGained = -100;

            // Act
            character.Level.AddExperience(experienceGained);

            // Assert
            Assert.Equal(initialExperience, character.Level.CurrentExperience);
            Assert.Equal(1, character.Level.CurrentLevel);
            Assert.Equal(0, character.Level.CurrentExperience);
        }

        [Fact]
        public void TotalExperienceToNextLevelWorks()
        {
            // Arrange
            var character = new PlayerTemplate.Player("TestCharacter");

            var initialExperience = character.Level.CurrentExperience;

            Assert.Equal(0, initialExperience);
            Assert.Equal(1, character.Level.CurrentLevel);

            // Act
            var totalExperienceToNextLevel = character.Level.GetExperienceNeededForNextLevel();

            // Assert
            Assert.Equal(ExperienceTable.Default.Table[character.Level.CurrentLevel + 1], totalExperienceToNextLevel);
        }
    }

}