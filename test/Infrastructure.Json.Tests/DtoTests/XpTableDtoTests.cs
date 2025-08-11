using Infrastructure.Json.Dto.ExpTable;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class XpTableDtoTests
    {
        public static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "text_xp_table.exp_table.json");

        [Fact]
        public void CanDeserializeXpTableDto()
        {
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<ExpTableDto>(json);

            Assert.True(dto is not null);
            Assert.Equal("test_xp_table", dto.id);
            Assert.Equal(50, dto.table.Length);
            var entry = dto.table[0];
            Assert.NotNull(entry);

            Assert.Equal(1, entry.level);
            Assert.Equal(437, entry.experience);
        }
    }
}