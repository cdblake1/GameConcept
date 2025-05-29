using GameData;
using GameData.Mobs;
using static LootTable;

public abstract class Encounter
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public virtual int CurrentDuration => currentDuration;
    public virtual bool EncounterIsActive => !encounterEndedEarly && CurrentDuration < Config.Duration;
    public virtual bool EncounterEndedEarly => encounterEndedEarly;
    protected bool encounterEndedEarly = false;
    protected int currentDuration = 0;
    public abstract WeightedMobSelector MobSelector { get; }
    public EncounterConfig Config { get; }

    public abstract MobBase AdvanceEncounter();
    public abstract EncounterReward EncounterReward();

    public virtual void EndEncounter()
    {
        if (EncounterIsActive)
        {
            encounterEndedEarly = true;
        }
    }

    public Encounter(EncounterConfig config)
    {
        this.Config = config ?? throw new ArgumentNullException(nameof(config));
    }
}

public readonly struct EncounterReward
{
    public GoldCoin GoldCoin { get; init; }
    public List<IItem> Loot { get; init; }
}

public class EncounterConfig
{
    public int PlayerLevel { get; set; }
    public int Duration { get; set; }
    public bool IsBossEncounter { get; set; }
    public EncounterConfig(int currentPlayerLevel, int duration, bool isBossEncounter = false)
    {
        this.PlayerLevel = currentPlayerLevel;
        this.Duration = duration;
        this.IsBossEncounter = isBossEncounter;
    }
}

public static class EncounterTemplates
{
    [EncounterMetadata(typeof(GoblinEncampment), MinLevel, MaxLevel, hasBoss: true)]
    public class GoblinEncampment : Encounter
    {
        public const string NameIdentifier = "Goblin Encampment";
        public const int MinLevel = 1;
        public const int MaxLevel = 10;
        public override string Name { get; } = NameIdentifier;
        public override string Description => "Battle with the goblins of the forest.\nDefeat the Goblin King to claim your reward.";

        public override WeightedMobSelector MobSelector { get; }
        public MobBase BossEncounter { get; }

        public GoblinEncampment(EncounterConfig config) : base(config)
        {
            MobSelector = new WeightedMobSelector(new List<MobSpawnConfig>
            {
                new (() => MobFactory.Instance.Create(typeof(GoblinGruntActor)), 50),
                new (() => MobFactory.Instance.Create(typeof(GoblinRangerActor)), 30),
                new (() => MobFactory.Instance.Create(typeof(GoblinWarriorActor)), 20),
            });

            BossEncounter = MobFactory.Instance.Create(typeof(GoblinCaptainActor));
        }

        // static GoblinEncampment()
        // {
        //     EncounterFactory.RegisterMetadata(new(typeof(GoblinEncampment), MinLevel, MaxLevel, hasBoss: true));
        //     EncounterFactory.Register<GoblinEncampment>((config) => new GoblinEncampment(config));
        // }

        public override EncounterReward EncounterReward()
        {
            var table = new LootTable(new List<LootTableEntry>
            {
            new LootTableEntry(CraftingMaterialTemplates.IronScrap.FromRange(2, 4), 50),
            new LootTableEntry(CraftingMaterialTemplates.WoodenShoot.FromRange(1, 3), 50),
            }, true);

            var loot = new List<IItem>();
            for (int i = 0; i < CurrentDuration; i++)
            {
                var lootItem = table.GetRandomLootEntry();
                if (lootItem.HasValue)
                {
                    loot.Add(lootItem.Value.Item);
                }
            }

            return new EncounterReward
            {
                GoldCoin = GoldCoin.FromRange(10 * CurrentDuration, 20 * CurrentDuration),
                Loot = loot
            };
        }

        public override MobBase AdvanceEncounter()
        {
            if (!EncounterIsActive)
                throw new InvalidOperationException("Encounter has already ended.");

            currentDuration++;
            if (Config.IsBossEncounter && currentDuration == Config.Duration)
            {
                return this.BossEncounter;
            }

            return MobSelector.SelectMob(this);
        }

        public static GoblinEncampment FromDuration(int currentPlayerLevel, int duration, bool isBossEncounter = false)
        {
            return new GoblinEncampment(new(currentPlayerLevel, duration, isBossEncounter));
        }

        public static GoblinEncampment FromDurationRange(int currentPlayerLevel, int minDuration, int maxDuration, bool isBossEncounter = false)
        {
            int duration = Random.Shared.Next(minDuration, maxDuration + 1);
            return new GoblinEncampment(new(currentPlayerLevel, duration, isBossEncounter));
        }
    }

    [EncounterMetadata(typeof(SpringLandsEncounter), MinLevel, MaxLevel, true)]
    public class SpringLandsEncounter : Encounter
    {
        public const string NameIdentifier = "Spring Lands";
        public const int MinLevel = 1;
        public const int MaxLevel = 10;

        public override string Name => NameIdentifier;
        public override string Description => "Battle with the beasts of the Spring Lands.\nDefeat the Den Mother to claim your reward.";
        public override WeightedMobSelector MobSelector { get; }
        public MobBase BossEncounter { get; }
        public override EncounterReward EncounterReward()
        {
            var table = new LootTable(new List<LootTableEntry>
                {
                    new LootTableEntry(CraftingMaterialTemplates.TatteredCloth.FromRange(3, 5), 66),
                    new LootTableEntry(CraftingMaterialTemplates.WoodenShoot.FromRange(1, 2), 33),
                }, true);
            var goldReward = GoldCoin.FromAmount(0);
            var loot = new List<IItem>();

            for (int i = 0; i < currentDuration; i++)
            {
                goldReward += GoldCoin.FromRange(5, 10);

                var lootItem = table.GetRandomLootEntry();
                if (lootItem.HasValue)
                {
                    loot.Add(lootItem.Value.Item);
                }
            }

            return new EncounterReward
            {
                GoldCoin = GoldCoin.FromRange(5 * CurrentDuration, 10 * CurrentDuration),
                Loot = loot
            };
        }

        public SpringLandsEncounter(EncounterConfig config) : base(config)
        {
            MobSelector = new WeightedMobSelector(new List<MobSpawnConfig>
            {
                new(() => MobFactory.Instance.Create(typeof(BoarActor)), 50),
                new(() => MobFactory.Instance.Create(typeof(WolfActor)), 30),
                new(() => MobFactory.Instance.Create(typeof(BearActor)), 20),
            });

            BossEncounter = MobFactory.Instance.Create(typeof(DenMotherActor));
        }

        // static SpringLandsEncounter()
        // {
        //     EncounterFactory.RegisterMetadata(new(typeof(SpringLandsEncounter), MinLevel, MaxLevel, hasBoss: true));
        //     EncounterFactory.Register<SpringLandsEncounter>((config) => new SpringLandsEncounter(config));
        // }

        public override MobBase AdvanceEncounter()
        {
            if (currentDuration > Config.Duration)
                throw new InvalidOperationException("Encounter has already ended.");

            currentDuration++;
            if (Config.IsBossEncounter && currentDuration == Config.Duration)
            {
                return this.BossEncounter;
            }

            return MobSelector.SelectMob(this);
        }

        public static SpringLandsEncounter FromDuration(int currentPlayerLevel, int duration, bool isBossEncounter = false)
        {
            return new SpringLandsEncounter(new(currentPlayerLevel, duration, isBossEncounter));
        }

        public static SpringLandsEncounter FromDurationRange(int currentPlayerLevel, int minDuration, int maxDuration, bool isBossEncounter = false)
        {
            int duration = Random.Shared.Next(minDuration, maxDuration + 1);
            return new SpringLandsEncounter(new(currentPlayerLevel, duration, isBossEncounter));
        }
    }
}

public record struct MobSpawnConfig
{
    public Func<MobBase> MobFactory { get; }
    public int SpawnWeight { get; set; }

    public MobSpawnConfig(Func<MobBase> mob, int spawnRate)
    {
        MobFactory = mob;
        SpawnWeight = spawnRate;
    }
}

public class WeightedMobSelector
{
    public readonly IReadOnlyList<MobSpawnConfig> mobSpawnConfigs;

    public WeightedMobSelector(IReadOnlyList<MobSpawnConfig> mobSpawnConfigs)
    {
        // Sort by SpawnWeight ascending
        this.mobSpawnConfigs = mobSpawnConfigs.OrderBy(cfg => cfg.SpawnWeight).ToList();
    }

    /// <summary>
    /// Selects a mob based on weights, adjusting for encounter duration.
    /// </summary>
    public MobBase SelectMob(Encounter encounter)
    {
        // Adjust weights based on encounter duration
        var weightedMobs = mobSpawnConfigs
            .Select(cfg => (
                mob: cfg.MobFactory,
                weight: AdjustWeightForEncounter(cfg.SpawnWeight, encounter)
            ))
            .Where(entry => entry.weight > 0)
            .ToList();

        int totalWeight = weightedMobs.Sum(x => x.weight);
        if (totalWeight == 0)
            throw new InvalidOperationException("No mobs available to select (all weights are zero or less).");

        int roll = Random.Shared.Next(0, totalWeight);
        int cumulative = 0;
        foreach (var entry in weightedMobs)
        {
            cumulative += entry.weight;
            if (roll < cumulative)
                return entry.mob();
        }
        throw new InvalidOperationException("Failed to select a mob due to an internal error.");
    }

    /// <summary>
    /// Adjusts the weight for a mob based on the encounter's duration.
    /// Override this logic as needed.
    /// </summary>
    private int AdjustWeightForEncounter(int baseWeight, Encounter encounter)
    {
        // Define min and max weights in your mobSpawnConfigs
        int minWeight = mobSpawnConfigs.Min(cfg => cfg.SpawnWeight);
        int maxWeight = mobSpawnConfigs.Max(cfg => cfg.SpawnWeight);

        // Normalize baseWeight to [0,1]
        double t = (double)(baseWeight - minWeight) / (maxWeight - minWeight + 1e-6);

        // As duration increases, boost low weights and reduce high weights
        double durationFactor = Math.Min(encounter.Config.Duration / 10.0, 1.0); // scale up to duration 10
        double lowWeightBoost = 1.0 + durationFactor;    // e.g. up to 2x for low weights
        double highWeightPenalty = 1.0 - 0.5 * durationFactor; // e.g. down to 0.5x for high weights

        // Interpolate factor: low weights get more boost, high weights get more penalty
        double factor = lowWeightBoost * (1 - t) + highWeightPenalty * t;

        return Math.Max(1, (int)(baseWeight * factor));
    }
}

// public record EncounterMetadata
// {
//     public Type EncounterType { get; }
//     public int MinLevel { get; }
//     public int MaxLevel { get; }
//     public bool HasBoss { get; }

//     public EncounterMetadata(Type encounterType, int minLevel, int maxLevel, bool hasBoss)
//     {
//         EncounterType = encounterType;
//         MinLevel = minLevel;
//         MaxLevel = maxLevel;
//         HasBoss = hasBoss;
//     }
// }
