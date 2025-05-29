
public record EncounterMetadata
{
    public Type Type { get; init; }
    public int MinLevel { get; init; }
    public int MaxLevel { get; init; }
    public bool HasBoss { get; init; }

    public EncounterMetadata(Type type, int minLevel, int maxLevel, bool hasBoss)
    {
        Type = type;
        MinLevel = minLevel;
        MaxLevel = maxLevel;
        HasBoss = hasBoss;
    }
}