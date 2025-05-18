
using Xunit;
using GameDataLayer;

namespace GameDataLayerTests;

public class UnitTest1
{
    [Fact]
    public void DamageIsAppliedToTarget()
    {

        var one = new CharacterBase("One", new StatTemplate
        {
            AttackPower = 10,
            Defense = 0,
            Health = 100
        });

        var two = new CharacterBase("Two", new StatTemplate
        {
            AttackPower = 5,
            Defense = 0,
            Health = 100
        });


        var damage = one.Attack(two);
        Assert.Equal(10, damage);
        Assert.Equal(90, two.CurrentHealth);
        Assert.Equal(100, one.CurrentHealth);
    }

    [Fact]
    public void DamageIsReducedByDefense()
    {
        var one = new CharacterBase("One", new StatTemplate
        {
            AttackPower = 10,
            Defense = 0,
            Health = 100
        });

        var two = new CharacterBase("Two", new StatTemplate
        {
            AttackPower = 5,
            Defense = 5,
            Health = 100
        });

        var damage = one.Attack(two);
        Assert.Equal(5, damage);
        Assert.Equal(95, two.CurrentHealth);
        Assert.Equal(100, one.CurrentHealth);
    }
}
