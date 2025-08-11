using Infrastructure.Json.Dto.Encounter;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class EncounterDtoTests
    {
        public static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "test_encounter.encounter.json");

        [Fact]
        public void CanDeserializeEncounterDto()
        {
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<EncounterDto>(json);

            Assert.NotNull(dto);
            Assert.Equal("test_encounter", dto.id);
            Assert.Equal(1, dto.min_level);
            Assert.Equal(5, dto.duration.max);
            Assert.Equal("test_loot_table", dto.loot_table);
            Assert.Equal("test_mob", dto.mob_weights[0].mob_id);
            Assert.Equal(100, dto.mob_weights[0].weight);
            Assert.Equal("Test Name", dto.presentation.name);
            Assert.Equal("Test Description", dto.presentation.description);
            Assert.False(dto.boss_encounter);
        }
    }
}