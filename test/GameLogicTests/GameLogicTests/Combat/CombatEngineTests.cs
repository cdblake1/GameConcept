using GameData.src.Player;
using GameLogic.Player;
using GameLogicTests.Helpers.Effects;
using GameLogicTests.Helpers.Setup;

namespace GameLogicTests.Combat
{
    public class CombatEngineTests
    {
        private readonly PlayerDefinition player = new PlayerDefinition(PlayerTestSetup.PlayerClassDefinition);

        private readonly PlayerInstance playerInstace;
        private readonly MobInstance mobInstance;

        public CombatEngineTests()
        {
            this.playerInstace = PlayerTestSetup.Create(this.player);
            this.mobInstance = new MobInstance(MobTestSetup.Create(), new StatCollection(), 10);
        }

        [Fact]
        public void CanLoadSkills()
        {
            var player = PlayerTestSetup.Create(this.player, 5);
            var mob = MobTestSetup.Create();

            Assert.Equal(2, player.GetSkills().Length);
            Assert.Equal(2, mob.Skills.Length);

            player = PlayerTestSetup.Create(this.player, 10);
            Assert.Equal(3, player.GetSkills().Length);
        }

        [Fact]
        public void DamagePayloadIsValid()
        {

        }

    }
}