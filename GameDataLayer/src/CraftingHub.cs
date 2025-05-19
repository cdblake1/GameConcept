class CraftingHub
{
    public List<CraftingRecipe> Recipes { get; set; }
    public CraftingHub()
    {
        Recipes = new List<CraftingRecipe>();
        Stations = new List<CraftingStation>();
    }

    public void AddRecipe(CraftingRecipe recipe)
    {
        Recipes.Add(recipe);
    }

    public void AddStation(CraftingStation station)
    {
        Stations.Add(station);
    }
}