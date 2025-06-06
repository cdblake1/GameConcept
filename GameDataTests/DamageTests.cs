
using GameData;
using GameData.Mobs;

namespace GameDataTests
{
    public class DamageTests
    {
        [Fact]
        public void DamageIsAppliedToTarget()
        {

            var one = TestMob.Create();

            var two = TestMob.Create();

            var effect = one.AttackSkill[0].Apply(new()
            {
                AddedBaseDamage = two.Stats.AttackPower,
                AddedBaseDamageMultiplier = 1,
                Multiplier = 1,
            }).OfType<DamageEffect>().First();
            var damage = two.ApplyDamage(effect);

            Assert.Equal(two.MaxHealth - two.CurrentHealth, damage);
        }

        [Fact]
        public void DamageIsReducedByDefense()
        {
            var one = new PlayerOld("One");
            var two = TestMob.Create();

            var effect = two.AttackSkill[0].Apply(new()
            {
                AddedBaseDamage = one.Stats.AttackPower,
                AddedBaseDamageMultiplier = 1,
                Multiplier = 1,
            }).OfType<DamageEffect>().First();

            var damage = one.ApplyDamage(effect);

            Assert.Equal(one.MaxHealth - one.CurrentHealth, damage);
        }
    }
}

