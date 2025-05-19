public record struct StatTemplate
{
    public required double AttackPower { get; init; }
    public required double Defense { get; init; }
    public required int Health { get; init; }

    public StatTemplate()
    {

    }

    public static StatTemplate operator +(StatTemplate a, StatTemplate b)
    {
        return new StatTemplate
        {
            AttackPower = a.AttackPower + b.AttackPower,
            Defense = a.Defense + b.Defense,
            Health = a.Health + b.Health
        };
    }

    public static StatTemplate operator -(StatTemplate a, StatTemplate b)
    {
        return new StatTemplate
        {
            AttackPower = a.AttackPower - b.AttackPower,
            Defense = a.Defense - b.Defense,
            Health = a.Health - b.Health
        };
    }

    public static StatTemplate operator *(StatTemplate a, int b)
    {
        return new StatTemplate
        {
            AttackPower = a.AttackPower * b,
            Defense = a.Defense * b,
            Health = a.Health * b
        };
    }
}
