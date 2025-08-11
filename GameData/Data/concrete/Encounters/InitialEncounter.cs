// using GameData.src.Mob;

// public class InitialEncounter : Encounter
// {
//     public InitialEncounter(EncounterConfig config) : base(config)
//     {
//     }

//     public override string Name => "Demo Island";
//     public override string Description => "The first encounter on the demo island.";
//     public override WeightedMobSelector MobSelector => new WeightedMobSelector(new List<MobSpawnConfig>
//     {
//         // new MobSpawnConfig(() => MobFactory.Instance.Create(typeof(FrogActor)), 25),
//         // new MobSpawnConfig(() => MobFactory.Instance.Create(typeof(PoisonMiteSwarmActor)), 75)
//     });

//     public override MobDefinition AdvanceEncounter()
//     {
//         if (currentDuration > Config.Duration)
//         {
//             throw new InvalidOperationException("Encounter duration exceeded.");
//         }

//         return new MobDefinition([]);
//     }

//     public override EncounterReward EncounterReward()
//     {
//         throw new NotImplementedException();
//     }
// }