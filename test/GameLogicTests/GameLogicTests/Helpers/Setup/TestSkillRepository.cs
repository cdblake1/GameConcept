using GameData.src.Effect;
using GameData.src.Skill;
using GameLogic.Ports;
using GameLogicTests.Helpers.Effects;

namespace GameLogicTests.Helpers.Setup
{
    public class TestSkillRepository : ISkillRepository
    {
        private static readonly Dictionary<string, SkillDefinition> cache = [];

        public TestSkillRepository()
        {
            cache[PlayerTestSetup.PlayerSkillOne.Id] = PlayerTestSetup.PlayerSkillOne;
            cache[PlayerTestSetup.PlayerSkillTwo.Id] = PlayerTestSetup.PlayerSkillTwo;
            cache[PlayerTestSetup.PlayerSkillThree.Id] = PlayerTestSetup.PlayerSkillThree;
            cache[MobTestSetup.MobSkillOne.Id] = MobTestSetup.MobSkillOne;
            cache[MobTestSetup.MobSkillTwo.Id] = MobTestSetup.MobSkillTwo;
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