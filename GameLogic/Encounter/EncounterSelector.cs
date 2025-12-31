using GameData.src.Encounter;

public class EncounterSelector
{
    public EncounterSelector(List<EncounterDefinition> encounterDefinitions)
    {
        EncounterDefinitions = encounterDefinitions ?? throw new ArgumentNullException(nameof(encounterDefinitions));
    }

    public List<EncounterDefinition> EncounterDefinitions { get; }

    public EncounterDefinition? SelectEncounter(int playerLevel, bool hasBoss)
    {
        int count = 0;
        EncounterDefinition? selected = null;
        var random = new Random();

        foreach (var e in EncounterDefinitions)
        {
            if (e.MinLevel <= playerLevel && e.BossEncounter == hasBoss)
            {
                count++;
                if (random.Next(count) == 0)
                {
                    selected = e;
                }
            }
        }

        return selected;
    }
}
