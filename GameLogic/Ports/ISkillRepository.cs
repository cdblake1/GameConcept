using GameData.src.Effect;
using GameData.src.Skill;

namespace GameLogic.Ports
{
    public interface ISkillRepository
    {
        SkillDefinition Get(string id);

        IReadOnlyList<SkillDefinition> GetAll();
    }
}