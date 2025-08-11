using GameData.src.Encounter;
using Infrastructure.Json.Repositories;
using Xunit;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class EncounterRepositoryTests
    {
        private const string testEncounterId = "test_encounter";
        private readonly JsonEncounterRepository repository;
        public static readonly string EncounterDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets");

        public EncounterRepositoryTests()
        {
            this.repository = new JsonEncounterRepository(EncounterDirectoryPath);
        }

        [Fact]
        public void CanLoadEncountersIntoRepository()
        {
            var encounters = this.repository.GetAll();

            Assert.Single(encounters, encounter => encounter.Id == testEncounterId);

            var encounter = this.repository.Get(testEncounterId);

            Assert.NotNull(encounter);
            Assert.Equal(testEncounterId, encounter.Id);
            Assert.False(encounter.BossEncounter);
            Assert.Equal(1, encounter.Duration.Min);
            Assert.Equal(5, encounter.Duration.Max);
            Assert.Equal("test_loot_table", encounter.LootTable);
            Assert.Equal(1, encounter.MinLevel);
            Assert.Single(encounter.MobWeights);
            Assert.Equal("test_mob", encounter.MobWeights[0].MobId);
            Assert.Equal(100, encounter.MobWeights[0].Weight);
            Assert.Equal("Test Name", encounter.Presentation.Name);
            Assert.Equal("Test Description", encounter.Presentation.Description);
        }
    }
}