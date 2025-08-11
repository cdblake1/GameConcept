using Infrastructure.Json.Dto.CraftingRecipe;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class CraftingRecipeDtoTests
    {
        public static readonly string RecipeFilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "test_recipe.crafting_recipe.json");

        [Fact]
        public void CanDeserializeCraftingRecipeDto()
        {
            var json = File.ReadAllText(RecipeFilePath);
            var dto = JsonConvert.DeserializeObject<CraftingRecipeDto>(json);

            Assert.NotNull(dto);
            Assert.Equal("test_crafting_recipe", dto.id);
            Assert.Equal("test_item", dto.crafted_item_id);
            Assert.Single(dto.materials);
            Assert.Equal("test_item", dto.materials[0].item_id);
            Assert.Equal(1, dto.materials[0].count);
        }
    }
}