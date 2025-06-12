using GameData.src.Effect;
using GameData.src.Effect.Talent;
using GameData.src.Talent;
using GameLogic.Ports;
using GameLogicTests.Helpers.Effects;

namespace GameLogicTests.Helpers.Setup
{
    public class TestTalentRepository : ITalentRepository
    {
        private static readonly Dictionary<string, TalentDefinition> cache = [];
        public TestTalentRepository()
        {
        }

        public TalentDefinition Get(string id)
        {
            return cache[id];
        }

        public IReadOnlyList<TalentDefinition> GetAll()
        {
            return [.. cache.Values];
        }
    }
}