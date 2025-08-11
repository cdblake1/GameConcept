using GameData.src.LootTable;

namespace GameLogic.Ports
{
    public interface ILootTableRepository
    {
        LootTableDefinition Get(string id);

        IReadOnlyList<LootTableDefinition> GetAll();
    }
}