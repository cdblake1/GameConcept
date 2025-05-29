using GameData;
using GameData.Mobs;

public class EncounterSelectorTests
{
    private readonly EncounterSelector _encounterSelector;

    public EncounterSelectorTests()
    {
        _encounterSelector = EncounterSelector.Create();
    }

    [Fact]
    public void SelectEncounter_ValidInput_ReturnsExpectedEncounter()
    {
        // Arrange
        var expectedEncounter = new MockEncounter(new EncounterConfig(1, 2, false));
        EncounterFactory.Register<MockEncounter>((config) => new MockEncounter(config));
        EncounterFactory.RegisterMetadata(new(typeof(MockEncounter), 1, 3, false));
        // Act
        var result = _encounterSelector.SelectEncounters(new(1, 2, false), 1).FirstOrDefault();

        // Assert
        Assert.Equal(expectedEncounter.Name, result?.Name);
    }

    [EncounterMetadata(typeof(MockEncounter), 2, 3, false)]
    public class MockEncounter : Encounter
    {
        public MockEncounter(EncounterConfig config) : base(config)
        {
        }

        public override string Name => "MockEncounter";

        public override string Description => "Mock description";

        public override WeightedMobSelector MobSelector => new WeightedMobSelector(new List<MobSpawnConfig>()
        {
            new MobSpawnConfig(() => MobFactory.Instance.Create(typeof(GoblinGruntActor)), 1)
        });

        public override MobBase AdvanceEncounter()
        {
            return MobSelector.SelectMob(this);
        }

        public override EncounterReward EncounterReward()
        {
            return new EncounterReward()
            {
                GoldCoin = GoldCoin.FromAmount(100),
                Loot = []
            };
        }
    }
}