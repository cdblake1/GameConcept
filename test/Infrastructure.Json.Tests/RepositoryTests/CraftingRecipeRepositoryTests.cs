using Infrastructure.Json.Repositories;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class CraftingRecipeRepositoryTests
    {
        private const string testRecipeId = "test_crafting_recipe";
        private readonly JsonCraftingRecipeRepository repository;
        public static readonly string RecipeDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets");

        public CraftingRecipeRepositoryTests()
        {
            this.repository = new JsonCraftingRecipeRepository(RecipeDirectoryPath);
        }

        [Fact]
        public void CanLoadCraftingRecipesIntoRepository()
        {
            var recipes = this.repository.GetAll();

            Assert.Single(recipes, recipe => recipe.Id == testRecipeId);

            var recipe = this.repository.Get(testRecipeId);

            Assert.NotNull(recipe);
            Assert.Equal(testRecipeId, recipe.Id);
            Assert.Equal("test_item", recipe.CraftedItemId);
            Assert.Single(recipe.Materials);
            Assert.Equal("test_item", recipe.Materials[0].ItemId);
            Assert.Equal(1, recipe.Materials[0].Count);
        }
    }
}