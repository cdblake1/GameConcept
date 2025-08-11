// public class EncounterSelector
// {
//     private readonly Random random;

//     private EncounterSelector()
//     {
//         random = new Random();
//     }

//     public virtual IReadOnlyList<Encounter> SelectEncounters(EncounterConfig config, int maxEncounters)
//     {
//         System.Diagnostics.Debugger.Launch();
//         var selectedEncounters = new List<Encounter>();
//         var eligibleEncounters = EncounterFactory.GetEligibleEncounters(config).ToList();
//         var encounterCount = Math.Min(maxEncounters, eligibleEncounters.Count);

//         for (int i = 0; i < encounterCount; i++)
//         {
//             var randomIndex = random.Next(eligibleEncounters.Count);
//             var encounterMetadata = eligibleEncounters[randomIndex];
//             var encounter = EncounterFactory.CreateEncounter(encounterMetadata.Type, config);
//             selectedEncounters.Add(encounter);
//             eligibleEncounters.RemoveAt(randomIndex);
//         }

//         return selectedEncounters;
//     }

//     public static EncounterSelector Create()
//     {
//         return new EncounterSelector();
//     }
// }