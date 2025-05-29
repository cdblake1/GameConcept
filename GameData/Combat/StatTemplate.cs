public record struct StatTemplate : IStateSerializable<StatTemplateDto, StatTemplate>
{
    public required double AttackPower { get; init; }
    public required double Defense { get; init; }
    public required int Health { get; init; }

    public required int Speed { get; init; }

    public StatTemplate()
    {
    }

    public StatTemplate(StatTemplateDto dto)
    {
        AttackPower = dto.AttackPower;
        Defense = dto.Defense;
        Health = dto.Health;
        Speed = dto.Speed;
    }

    public static StatTemplate operator +(StatTemplate a, StatTemplate b)
    {
        return new StatTemplate
        {
            AttackPower = a.AttackPower + b.AttackPower,
            Defense = a.Defense + b.Defense,
            Health = a.Health + b.Health,
            Speed = a.Speed + b.Speed
        };
    }

    public static StatTemplate operator -(StatTemplate a, StatTemplate b)
    {
        return new StatTemplate
        {
            AttackPower = a.AttackPower - b.AttackPower,
            Defense = a.Defense - b.Defense,
            Health = a.Health - b.Health,
            Speed = a.Speed + b.Speed
        };
    }

    public static StatTemplate operator *(StatTemplate a, int b)
    {
        return new StatTemplate
        {
            AttackPower = a.AttackPower * b,
            Defense = a.Defense * b,
            Health = a.Health * b,
            Speed = a.Speed * b
        };
    }

    public StatTemplateDto Serialize()
    {
        return new StatTemplateDto
        {
            AttackPower = AttackPower,
            Defense = Defense,
            Health = Health,
            Speed = Speed
        };
    }

    public static StatTemplate Restore(StatTemplateDto dto)
    {
        return new StatTemplate
        {
            AttackPower = dto.AttackPower,
            Defense = dto.Defense,
            Health = dto.Health,
            Speed = dto.Speed
        };
    }
}
