using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Mob;
using Infrastructure.Json.Mappers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class MobMapperTests
    {
        [Fact]
        public void CanMapMobDto()
        {
            var mobDto = new MobDto(
                id: "test_mob",
                loot_table: "test_loot_table",
                exp_table: "test_exp_table",
                presentation: new PresentationDto() { name = "test_mob", description = "test_mob", icon = "test_mob" },
                skills: ["test_skill"],
                stats: "test_stats"
            );

            var mob = mobDto.ToDomain();

            Assert.Equal("test_mob", mob.Id);
            Assert.Equal("test_loot_table", mob.LootTable);
            Assert.Equal("test_exp_table", mob.ExpTable);
            Assert.Equal("test_mob", mob.Presentation.Name);
            Assert.Equal("test_mob", mob.Presentation.Description);
            Assert.Equal("test_mob", mob.Presentation.Icon);
            Assert.Equal("test_skill", mob.Skills[0]);
            Assert.Equal("test_stats", mob.Stats);
        }
    }
}