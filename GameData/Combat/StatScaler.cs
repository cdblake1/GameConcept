public static class StatScaler
{
    private static readonly Random _rand = new();

    public static StatTemplateOld Scale(
        StatTemplateOld baseStats,
        int level,
        int maxLevel = 100,
        GrowthModel model = GrowthModel.Linear,
        double growthRate = 1.0,
        double variancePercent = 0.0,
        bool isElite = false,
        double eliteMultiplier = 1.5
    )
    {
        double scaleFactor = model switch
        {
            GrowthModel.Linear => level,
            GrowthModel.Quadratic => Math.Pow(level, 2),
            GrowthModel.Exponential => Math.Pow(growthRate, level - 1),
            GrowthModel.Logarithmic => Math.Log(level + 1) * growthRate,
            GrowthModel.Smoothed => SmoothedScaling(level, maxLevel),
            _ => level
        };

        // Use base as level 1, grow from there
        double effectiveFactor = scaleFactor - 1;

        double attack = baseStats.AttackPower + (baseStats.AttackPower * effectiveFactor);
        double defense = baseStats.Defense + (baseStats.Defense * effectiveFactor);
        double health = baseStats.Health + (baseStats.Health * effectiveFactor);

        // Apply variance
        attack *= ApplyVariance(variancePercent);
        defense *= ApplyVariance(variancePercent);
        health *= ApplyVariance(variancePercent);

        // Elite multiplier
        if (isElite)
        {
            attack *= eliteMultiplier;
            defense *= eliteMultiplier;
            health *= eliteMultiplier;
        }

        return new StatTemplateOld
        {
            AttackPower = attack,
            Defense = defense,
            Health = (int)Math.Round(health),
            Speed = baseStats.Speed
        };
    }

    private static double ApplyVariance(double percent)
    {
        if (percent <= 0) return 1.0;
        double range = percent * 2;
        return 1.0 - percent + (_rand.NextDouble() * range);
    }

    private static double SmoothedScaling(int level, int maxLevel)
    {
        double x = (double)level / maxLevel;
        double smooth = 1 / (1 + Math.Exp(-12 * (x - 0.5)));
        return 0.5 + smooth * 0.5;
    }
}
