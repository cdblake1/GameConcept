using GameData.src.Class;

namespace GameLogic.Ports
{
    public interface IClassRepository
    {
        ClassDefinition Get(string id);

        IReadOnlyList<ClassDefinition> GetAll();
    }
}