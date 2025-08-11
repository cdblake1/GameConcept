using GameData.src.Encounter;

namespace GameLogic.Ports
{
    public interface IEncounterRepository
    {
        EncounterDefinition Get(string id);

        IReadOnlyList<EncounterDefinition> GetAll();
    }
}