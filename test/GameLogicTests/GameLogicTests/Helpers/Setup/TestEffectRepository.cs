using GameData.src.Effect;
using GameLogic.Ports;
using GameLogicTests.Helpers.Effects;

namespace GameLogicTests.Helpers.Setup
{
    public class TestEffectRepository : IEffectRepository
    {
        private static readonly Dictionary<string, EffectDefinition> cache = [];
        public TestEffectRepository()
        {
            cache[PlayerTestSetup.PlayerEffectOne.Id] = PlayerTestSetup.PlayerEffectOne;
            cache[PlayerTestSetup.PlayerEffectTwo.Id] = PlayerTestSetup.PlayerEffectTwo;
            cache[PlayerTestSetup.PlayerEffectThree.Id] = PlayerTestSetup.PlayerEffectThree;

            cache[MobTestSetup.MobEffectOne.Id] = MobTestSetup.MobEffectOne;
            cache[MobTestSetup.MobEffectTwo.Id] = MobTestSetup.MobEffectTwo;
        }

        public EffectDefinition Get(string id)
        {
            return cache[id];
        }

        public IReadOnlyList<EffectDefinition> GetAll()
        {
            return [.. cache.Values];
        }
    }
}