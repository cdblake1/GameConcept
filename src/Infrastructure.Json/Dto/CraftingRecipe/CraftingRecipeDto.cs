using Newtonsoft.Json;

namespace Infrastructure.Json.Dto.CraftingRecipe
{
    public sealed record CraftingRecipeDto(
        [JsonProperty] string id,
        [JsonProperty] string crafted_item_id,
        [JsonProperty] CraftingRecipeItemDto[] materials);

    public sealed record CraftingRecipeItemDto(
        [JsonProperty] string item_id,
        [JsonProperty] int count);
}