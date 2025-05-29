public class EncounterMetadataAttribute : Attribute
{
    public Type Type { get; }
    public int MinLevel { get; }
    public int MaxLevel { get; }
    public bool HasBoss { get; }

    public EncounterMetadataAttribute(Type type, int minLevel, int maxLevel, bool hasBoss)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        MinLevel = minLevel;
        MaxLevel = maxLevel;
        HasBoss = hasBoss;

        EncounterFactory.RegisterMetadata(new(Type, MinLevel, MaxLevel, HasBoss));
    }
}