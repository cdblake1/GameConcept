using GameData.src.Player;
using GameLogic.Combat;
using GameLogic.Mob;
using GameLogic.Player;
using GameLogicTests.Helpers.Effects;
using GameLogicTests.Helpers.Setup;
using static GameLogic.Combat.CombatEngine;

namespace GameLogicTests.Combat
{
    public class CombatEngineTests
    {
        private readonly PlayerDefinition player = new PlayerDefinition(
            PlayerTestSetup.PlayerClassDefinition,
            new GameData.src.Shared.PresentationDefinition
            {
                Name = "Test Player",
                Description = "A test player for unit tests",
                Icon = null
            });
        private readonly PlayerInstance playerInstace;
        private readonly MobInstance mobInstance;
        private readonly TestEffectRepository effectRepo = new();
        private readonly TestSkillRepository skillRepo = new();
        private readonly TestTalentRepository talentRepo = new();
        internal readonly CombatEngine engine;

        public CombatEngineTests()
        {
            this.playerInstace = PlayerTestSetup.Create(this.player);
            this.mobInstance = new MobInstance(MobTestSetup.Create(), new StatCollection(), 10);

            this.engine = new CombatEngine(
                this.skillRepo,
                this.effectRepo,
                this.talentRepo,
                this.playerInstace,
                this.mobInstance
            );
        }

        [Fact]
        public void CanLoadSkills()
        {
            var player = PlayerTestSetup.Create(this.player, 5);
            var mob = MobTestSetup.Create();

            Assert.Equal(2, player.GetSelectedSkills().Count);
            Assert.Equal(2, mob.Skills.Length);

            player = PlayerTestSetup.Create(this.player, 10);
            Assert.Equal(3, player.GetSelectedSkills().Count);
        }

        [Fact]
        public void DamagePayloadIsValid()
        {
            var skillSnapshot = this.engine.player.Skills[0];
            var dmgStepSnapshot = skillSnapshot.DamageSteps[0];
            var dmgSnap = DamageSnapshotBuilder.BuildNew(skillSnapshot.skillDefinition, dmgStepSnapshot);
            (bool crit, float dmg) = CombatEngine.CalculateDamage(dmgSnap, this.engine.player, this.engine.mob);

            Assert.Equal(dmg, dmgSnap.Damage);
        }

        [Fact]
        public void ProcessAttack()
        {
            this.engine.ProcessCombat(
                new CombatEngine.UseSkillCommand(
                    this.player.ClassDefinition.SkillEntries[0].Id,
                    this.engine.mob.Identifier));

            var cevtCt = 0;
            var devtCt = 0;
            var aevtCt = 0;

            foreach (var eventBase in this.engine.events)
            {
                switch (eventBase)
                {
                    case CombatEngine.CombatStartEvent evt:
                        cevtCt++;
                        break;
                    case DamageAppliedEvent evt:
                        devtCt++;
                        break;
                    case EffectApplied evt:
                        aevtCt++;
                        break;
                }
            }

            Assert.Equal(1, cevtCt);
            Assert.True(devtCt >= 1);
            Assert.True(aevtCt >= 1);
        }
    }
}