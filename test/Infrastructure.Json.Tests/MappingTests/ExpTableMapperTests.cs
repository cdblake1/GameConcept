using Infrastructure.Json.Dto.ExpTable;
using Infrastructure.Json.Mappers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class ExpTableMapperTests
    {
        [Fact]
        public void Map_ExpTableDto_To_ExpTable()
        {
            // Arrange
            var dto = new ExpTableDto("1", [new ExpTableEntryDto(1, 100)]);

            // Act
            var result = dto.ToDomain();

            Assert.Equal("1", result.Id);
            Assert.Equal(100, result.Entries.First().Exp);
        }
    }
}