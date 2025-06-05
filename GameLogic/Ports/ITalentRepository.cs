using GameData.src.Effect.Talent;

namespace GameLogic.Ports
{
    public interface ITalentRepository
    {
        TalentDefinition Get(string id);

        IReadOnlyList<TalentDefinition> GetAll();
    }
}