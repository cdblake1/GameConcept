namespace GameDataTests;

public class StatTests
{

    [Fact]
    public void StatScaling()
    {
        var baseStats = new StatTemplateOld
        {
            AttackPower = 20,
            Defense = 10,
            Health = 200,
            Speed = 2
        };

        for (int lvl = 1; lvl <= 100; lvl += 20)
        {
            var mob = StatScaler.Scale(baseStats, lvl, 100, GrowthModel.Smoothed, growthRate: 1.1, variancePercent: 0.1);
            var elite = StatScaler.Scale(baseStats, lvl, 100, GrowthModel.Smoothed, growthRate: 1.1, variancePercent: 0.1, isElite: true);

            Console.WriteLine($"[Lv {lvl}] Mob:    AP={mob.AttackPower:F1}, DEF={mob.Defense:F1}, HP={mob.Health}");
            Console.WriteLine($"[Lv {lvl}] Elite:  AP={elite.AttackPower:F1}, DEF={elite.Defense:F1}, HP={elite.Health}");
            Console.WriteLine();
        }
    }
}