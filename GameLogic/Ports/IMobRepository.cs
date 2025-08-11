using GameData.src.Mob;

namespace GameLogic.Ports
{
    public interface IMobRepository
    {
        MobDefinition Get(string id);

        IReadOnlyList<MobDefinition> GetAll();
    }
}