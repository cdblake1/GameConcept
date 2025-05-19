
using GameDataLayer;

namespace GameDataLayerTests
{
    public class DamageTests
    {
        [Fact]
        public void DamageIsAppliedToTarget()
        {

            var one = new PlayerTemplate.Player("One");

            var two = new PlayerTemplate.Player("Two");

            var damage = one.Attack(two);
            Assert.Equal(10, damage);
            Assert.Equal(90, two.CurrentHealth);
            Assert.Equal(100, one.CurrentHealth);
        }

        [Fact]
        public void DamageIsReducedByDefense()
        {
            var one = new PlayerTemplate.Player("One");
            var two = new TestMob("Two", new StatTemplate
            {
                Health = 100,
                AttackPower = 5,
                Defense = 5,
            }, new LootTable(new List<LootTable.LootTableEntry>()));

            var damage = one.Attack(two);
            Assert.Equal(5, damage);
            Assert.Equal(95, two.CurrentHealth);
            Assert.Equal(100, one.CurrentHealth);
        }
    }
}

