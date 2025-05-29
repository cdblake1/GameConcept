using GameData;

public class EncounterFactory
{
    private static readonly Dictionary<Type, Func<EncounterConfig, Encounter>> encounterCreators = new();
    private static readonly Dictionary<Type, EncounterMetadata> encounterMetadata = new();
    public static void Register<TEncounter>(Func<EncounterConfig, Encounter> factoryFunction) where TEncounter : Encounter
    {
        encounterCreators[typeof(TEncounter)] = config => factoryFunction(config);
    }

    public static void RegisterMetadata(EncounterMetadata metadata)
    {
        if (encounterMetadata.ContainsKey(metadata.Type))
        {
            throw new ArgumentException($"Encounter type {metadata.Type} is already registered.");
        }

        encounterMetadata[metadata.Type] = metadata;
    }

    public static IEnumerable<EncounterMetadata> GetEligibleEncounters(EncounterConfig config)
    {
        var eligibleEncounters = encounterMetadata.Values
            .Where(metadata => metadata.MinLevel <= config.PlayerLevel && metadata.MaxLevel >= config.PlayerLevel)
            .ToList();

        return eligibleEncounters;
    }

    public static Encounter CreateEncounter<TEncounter>(EncounterConfig config) where TEncounter : Encounter
    {
        return encounterCreators[typeof(TEncounter)](config);
    }

    public static Encounter CreateEncounter(Type type, EncounterConfig config)
    {
        if (!encounterCreators.ContainsKey(type))
        {
            throw new ArgumentException($"Encounter type {type} is not registered.");
        }

        return encounterCreators[type](config);
    }
}