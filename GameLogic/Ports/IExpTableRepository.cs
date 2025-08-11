using GameData.src.ExpTable;

namespace GameLogic.Ports
{
    public interface IExpTableRepository
    {
        ExpTableDefinition Get(string id);

        IReadOnlyList<ExpTableDefinition> GetAll();
    }
}