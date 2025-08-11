using GameData.src.Shared.Enums;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class StatTemplateRepositoryTests
    {
        private const string testStatsId = "test_stats";
        private readonly JsonStatTemplateRepository repository;
        public static readonly string StatTemplateDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets");

        public StatTemplateRepositoryTests()
        {
            this.repository = new JsonStatTemplateRepository(StatTemplateDirectoryPath);
        }

        [Fact]
        public void CanLoadStatTemplatesIntoRepository()
        {
            var statTemplates = this.repository.GetAll();

            Assert.Single(statTemplates, stats => stats.Id == testStatsId);

            var statTemplate = this.repository.Get(testStatsId);

            Assert.NotNull(statTemplate);
            Assert.Equal(testStatsId, statTemplate.Id);
            Assert.Empty(statTemplate.Attack);
            Assert.Single(statTemplate.Damage);
            Assert.Equal(DamageType.Physical, statTemplate.Damage[0].DamageType);
            Assert.Single(statTemplate.Global);
            Assert.Equal(GlobalStat.Health, statTemplate.Global[0].GlobalStat);
            Assert.Empty(statTemplate.Weapon);
        }
    }
}