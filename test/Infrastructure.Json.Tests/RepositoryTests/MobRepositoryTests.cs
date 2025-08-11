using GameData.src.Mob;
using Infrastructure.Json.Repositories;
using Xunit;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class MobRepositoryTests
    {
        private const string testMobId = "test_mob";
        private readonly JsonMobRepository repository;
        public static readonly string MobDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets");

        public MobRepositoryTests()
        {
            this.repository = new JsonMobRepository(MobDirectoryPath);
        }

        [Fact]
        public void CanLoadMobsIntoRepository()
        {
            var mobs = this.repository.GetAll();

            Assert.Single(mobs, mob => mob.Id == testMobId);

            var mob = this.repository.Get(testMobId);

            Assert.NotNull(mob);
            Assert.Equal(testMobId, mob.Id);
            Assert.Equal("test_loot_table", mob.LootTable);
            Assert.Equal("test_stats", mob.Stats);
            Assert.Equal("test_xp_table", mob.ExpTable);
            Assert.Equal("Test Mob", mob.Presentation.Name);
            Assert.Equal("test mob description", mob.Presentation.Description);
            Assert.Equal("./test_mob.icon", mob.Presentation.Icon);
            Assert.Single(mob.Skills);
            Assert.Equal("hit_damage_test", mob.Skills[0]);
        }
    }
}