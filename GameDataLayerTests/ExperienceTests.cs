using GameDataLayer;

namespace GameDataLayerTests;

public class ExperienceTests
{
    [Fact]
    public void ExperienceGainsCorrectly()
    {
        // Arrange
        var character = new CharacterBase("TestCharacter", new StatTemplate
        {
            AttackPower = 10,
            Defense = 5,
            Health = 100
        }, new LevelManager(15, ExperienceTable.Default, new StatTemplate
        {
            AttackPower = 0,
            Defense = 0,
            Health = 0
        }));

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
        var character = new CharacterBase("TestCharacter", new StatTemplate
        {
            AttackPower = 10,
            Defense = 5,
            Health = 100
        }, new LevelManager(15, ExperienceTable.Default, new StatTemplate
        {
            AttackPower = 0,
            Defense = 0,
            Health = 0
        }));

        var initialExperience = character.LevelManager.CurrentExperience;

        Assert.Equal(0, initialExperience);
        Assert.Equal(1, character.LevelManager.CurrentLevel);
        var experienceGained = ExperienceTable.Default.GetCumulativeExperienceForLevel(2) - initialExperience;

        // Act
        character.LevelManager.AddExperience(experienceGained);

        // Assert
        Assert.Equal(initialExperience + experienceGained, character.LevelManager.CurrentExperience);
        Assert.Equal(2, character.LevelManager.CurrentLevel);

        var additionalExperience = ExperienceTable.Default.GetCumulativeExperienceForLevel(3) - character.LevelManager.CurrentExperience;
        character.LevelManager.AddExperience(additionalExperience);
        Assert.Equal(ExperienceTable.Default.GetCumulativeExperienceForLevel(3), character.LevelManager.CurrentExperience);

        Assert.Equal(3, character.LevelManager.CurrentLevel);
    }

    [Fact]
    public void ExperienceStopsAtMaxLevel()
    {
        // Arrange
        var character = new CharacterBase("TestCharacter", new StatTemplate
        {
            AttackPower = 10,
            Defense = 5,
            Health = 100
        }, new LevelManager(15, ExperienceTable.Default, new StatTemplate
        {
            AttackPower = 0,
            Defense = 0,
            Health = 0
        }));

        var initialExperience = character.LevelManager.CurrentExperience;

        Assert.Equal(0, initialExperience);
        Assert.Equal(1, character.LevelManager.CurrentLevel);
        var experienceGained = 1000000;

        // Act
        character.LevelManager.AddExperience(experienceGained);

        // Assert
        Assert.Equal(ExperienceTable.Default.GetCumulativeExperienceForLevel(character.LevelManager.MaxLevel), character.LevelManager.CurrentExperience);
    }

    [Fact]
    public void ExperienceCannotGoBelowMinLevelOrNegative()
    {
        // Arrange
        var character = new CharacterBase("TestCharacter", new StatTemplate
        {
            AttackPower = 10,
            Defense = 5,
            Health = 100
        }, new LevelManager(15, ExperienceTable.Default, new StatTemplate
        {
            AttackPower = 0,
            Defense = 0,
            Health = 0
        }));

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
        var character = new CharacterBase("TestCharacter", new StatTemplate
        {
            AttackPower = 10,
            Defense = 5,
            Health = 100
        }, new LevelManager(15, ExperienceTable.Default, new StatTemplate
        {
            AttackPower = 0,
            Defense = 0,
            Health = 0
        }));

        var initialExperience = character.LevelManager.CurrentExperience;

        Assert.Equal(0, initialExperience);
        Assert.Equal(1, character.LevelManager.CurrentLevel);

        // Act
        var totalExperienceToNextLevel = character.LevelManager.GetExperienceNeededForNextLevel();

        // Assert
        Assert.Equal(ExperienceTable.Default.Table[character.LevelManager.CurrentLevel + 1], totalExperienceToNextLevel);
    }
}