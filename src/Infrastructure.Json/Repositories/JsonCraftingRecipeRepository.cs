using GameData.src.CraftingRecipe;
using GameLogic.Ports;
using Infrastructure.Json.Dto.CraftingRecipe;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Repositories
{
    public class JsonCraftingRecipeRepository : ICraftingRecipeRepository
    {
        private const string searchPattern = "*.crafting_recipe.json";
        private readonly string contentDirectory;
        private readonly Lazy<IReadOnlyDictionary<string, CraftingRecipeDefinition>> cache;

        public JsonCraftingRecipeRepository(string contentDirectory)
        {
            this.contentDirectory = contentDirectory;
            this.cache = new Lazy<IReadOnlyDictionary<string, CraftingRecipeDefinition>>(this.LoadAll);
        }

        public CraftingRecipeDefinition Get(string id)
        {
            return this.cache.Value[id];
        }

        public IReadOnlyList<CraftingRecipeDefinition> GetAll()
        {
            return [.. this.cache.Value.Values];
        }

        private IReadOnlyDictionary<string, CraftingRecipeDefinition> LoadAll()
        {
            Dictionary<string, CraftingRecipeDefinition>? dict = null;

            foreach (var file in Directory.EnumerateFiles(this.contentDirectory, searchPattern, SearchOption.AllDirectories))
            {
                dict ??= [];
                var json = File.ReadAllText(file);
                var dto = JsonConvert.DeserializeObject<CraftingRecipeDto>(json) ?? throw new InvalidOperationException($"Could not convert to CraftingRecipeDto from file: {file}");
                var craftingRecipe = dto.ToDomain() ?? throw new InvalidOperationException($"could not convert from dto to CraftingRecipeDefinition for file: {file}");

                dict.Add(craftingRecipe.Id, craftingRecipe);
            }

            return dict ?? [];
        }
    }
}