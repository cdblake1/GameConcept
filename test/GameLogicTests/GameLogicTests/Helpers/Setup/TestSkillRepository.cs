using GameData.src.Effect;
using GameData.src.Skill;
using GameLogic.Ports;

namespace GameLogicTests.Helpers.Setup
{
    public class TestSkillRepository : ISkillRepository
    {
        private static readonly Dictionary<string, SkillDefinition> cache = [];

        public TestSkillRepository()
        {

        }

        public SkillDefinition Get(string id)
        {
            return cache[id];
        }

        public IReadOnlyList<SkillDefinition> GetAll()
        {
            return [.. cache.Values];
        }
    }
}