
using GameData.src.Item;

namespace GameLogic.Ports
{
    public interface IItemRepository
    {
        IItemDefinition Get(string id);

        IReadOnlyList<IItemDefinition> GetAll();
    }
}