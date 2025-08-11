using Infrastructure.Json.Dto.Mob;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class MobDtoTests
    {
        public static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "test_mob.mob.json");

        [Fact]
        public void CanDeserializeMobDto()
        {
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<MobDto>(json);

            Assert.NotNull(dto);

            Assert.Equal("test_mob", dto.id);
            Assert.Equal("test_loot_table", dto.loot_table);
            Assert.Equal("Test Mob", dto.presentation.name);
            Assert.Equal("test mob description", dto.presentation.description);
            Assert.Equal("./test_mob.icon", dto.presentation.icon);
            Assert.Single(dto.skills);
            Assert.Equal("hit_damage_test", dto.skills[0]);
            Assert.Equal("test_stats", dto.stats);
            Assert.Equal("test_xp_table", dto.exp_table);
        }
    }
}