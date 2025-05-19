
using GameDataLayer;

public interface ICraftingRecipe
{
    public IItem CraftedItem { get; }
    public List<IItem> RequiredMaterials { get; }
    public int CraftingTime { get; }
}