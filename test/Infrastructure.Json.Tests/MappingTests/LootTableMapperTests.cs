using Infrastructure.Json.Dto.LootTable;
using Infrastructure.Json.Mappers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class LootTableMapperTests
    {

        [Fact]
        public void Map_LootTable_To_LootTableDto()
        {
            // Arrange
            var lootTable = new LootTableDto(
                "test",
                [
                    new(
                        [
                            new("test", 100, false)
                        ]
                    )
                ]
            );

            // Act
            var result = lootTable.ToDomain();

            Assert.NotNull(result);
            Assert.Equal("test", result.Id);
            Assert.NotNull(result.Groups);
            Assert.Single(result.Groups);
            Assert.Single(result.Groups[0].Entries);
            Assert.Equal("test", result.Groups[0].Entries[0].ItemId);
            Assert.Equal(100, result.Groups[0].Entries[0].Weight);
            Assert.False(result.Groups[0].Entries[0].Always);
        }
    }
}