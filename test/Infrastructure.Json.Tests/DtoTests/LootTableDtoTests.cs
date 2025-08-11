using Infrastructure.Json.Dto.LootTable;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class LootTableDtoTests
    {
        public static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "test_loot_table.loot_table.json");

        [Fact]
        public void CanDeserializeLTDto()
        {
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<LootTableDto>(json);

            Assert.True(dto is not null);
            Assert.Equal("test_loot_table", dto.id);
            Assert.Single(dto.groups);

            var entry1 = dto.groups[0].entries[0];
            var entry2 = dto.groups[0].entries[1];

            Assert.Equal("test_item", entry1.item_id);
            Assert.Equal(500, entry1.weight);
            Assert.False(entry1.always);

            Assert.Equal("test_item_2", entry2.item_id);
            Assert.Equal(1000, entry2.weight);
            Assert.True(entry2.always);
        }
    }
}