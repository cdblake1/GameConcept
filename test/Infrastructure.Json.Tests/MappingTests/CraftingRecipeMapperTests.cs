using GameData.src.CraftingRecipe;
using Infrastructure.Json.Dto.CraftingRecipe;
using Infrastructure.Json.Mappers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class CraftingRecipeMapperTests
    {
        [Fact]
        public void CanMapCraftingRecipeDto()
        {
            // Arrange
            var dto = new CraftingRecipeDto(
                id: "test_recipe",
                crafted_item_id: "test_item",
                materials:
                [
                    new CraftingRecipeItemDto("iron_scrap", 10),
                    new CraftingRecipeItemDto("leather", 5)
                ]
            );

            // Act
            var result = dto.ToDomain();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.id, result.Id);
            Assert.Equal(dto.crafted_item_id, result.CraftedItemId);
            Assert.Equal(2, result.Materials.Count);
            Assert.Equal("iron_scrap", result.Materials[0].ItemId);
            Assert.Equal(10, result.Materials[0].Count);
            Assert.Equal("leather", result.Materials[1].ItemId);
            Assert.Equal(5, result.Materials[1].Count);
        }
    }
}