using GameData.src.Effect;

namespace GameLogic.Ports
{
    public interface IEffectRepository
    {
        EffectDefinition Get(string id);

        IReadOnlyList<EffectDefinition> GetAll();
    }
}