
using GameData;
using GameData.Mobs;

namespace GameDataTests
{
    public class DamageTests
    {
        [Fact]
        public void DamageIsAppliedToTarget()
        {

            var one = MobFactory.Instance.Create(typeof(FrogActor));

            var two = MobFactory.Instance.Create(typeof(FrogActor));

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
            var one = new Player("One");
            var two = MobFactory.Instance.Create(typeof(FrogActor));

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

