using GameData;
using GameData.Mobs;

public class InitialEncounter : Encounter
{
    public InitialEncounter(EncounterConfig config) : base(config)
    {
    }

    public override string Name => "Demo Island";
    public override string Description => "The first encounter on the demo island.";
    public override WeightedMobSelector MobSelector => new WeightedMobSelector(new List<MobSpawnConfig>
    {
        new MobSpawnConfig(() => MobFactory.Instance.Create(typeof(FrogActor)), 25),
        new MobSpawnConfig(() => MobFactory.Instance.Create(typeof(PoisonMiteSwarmActor)), 75)
    });

    public override MobBase AdvanceEncounter()
    {
        if (currentDuration > Config.Duration)
        {
            throw new InvalidOperationException("Encounter duration exceeded.");
        }

        return MobSelector.SelectMob(this);
    }

    public override EncounterReward EncounterReward()
    {
        throw new NotImplementedException();
    }
}