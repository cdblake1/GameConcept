public record struct StatTemplateOld : IStateSerializable<StatTemplateDto, StatTemplateOld>
{
    public required double AttackPower { get; init; }
    public required double Defense { get; init; }
    public required int Health { get; init; }

    public required int Speed { get; init; }

    public StatTemplateOld()
    {
    }

    public StatTemplateOld(StatTemplateDto dto)
    {
        AttackPower = dto.AttackPower;
        Defense = dto.Defense;
        Health = dto.Health;
        Speed = dto.Speed;
    }

    public static StatTemplateOld operator +(StatTemplateOld a, StatTemplateOld b)
    {
        return new StatTemplateOld
        {
            AttackPower = a.AttackPower + b.AttackPower,
            Defense = a.Defense + b.Defense,
            Health = a.Health + b.Health,
            Speed = a.Speed + b.Speed
        };
    }

    public static StatTemplateOld operator -(StatTemplateOld a, StatTemplateOld b)
    {
        return new StatTemplateOld
        {
            AttackPower = a.AttackPower - b.AttackPower,
            Defense = a.Defense - b.Defense,
            Health = a.Health - b.Health,
            Speed = a.Speed + b.Speed
        };
    }

    public static StatTemplateOld operator *(StatTemplateOld a, int b)
    {
        return new StatTemplateOld
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

    public static StatTemplateOld Restore(StatTemplateDto dto)
    {
        return new StatTemplateOld
        {
            AttackPower = dto.AttackPower,
            Defense = dto.Defense,
            Health = dto.Health,
            Speed = dto.Speed
        };
    }
}
