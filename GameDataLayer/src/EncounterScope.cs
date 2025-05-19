using GameDataLayer;
using static LootTable;

public abstract class Encounter
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Duration { get; }
    public abstract int CurrentDuration { get; }
    public abstract bool EncounterIsActive { get; }
    public abstract bool EncounterEndedEarly { get; }
    public abstract WeightedMobSelector MobSelector { get; }

    public abstract MobBase AdvanceEncounter();
    public abstract EncounterReward EncounterReward();
}

public readonly struct EncounterReward
{
    public GoldCoin GoldCoin { get; init; }
    public List<IItem> Loot { get; init; }
}

public static class EncounterTemplates
{
    public class GoblinEncampment : Encounter
    {
        public override string Name { get; } = "Goblin Encampment";
        public override string Description => "Battle with the goblins of the forest. Defeat the Goblin King to claim your reward.";
        public override int Duration => duration;
        public override int CurrentDuration => currentDuration;
        public override bool EncounterIsActive => !encounterEndedEarly && currentDuration < duration;
        public override bool EncounterEndedEarly => encounterEndedEarly;

        private readonly int duration;
        private int currentDuration = 0;
        public override WeightedMobSelector MobSelector { get; }
        public MobBase BossEncounter { get; }
        private readonly bool isBossEncounter;
        private bool encounterEndedEarly = false;

        public void EndEncounterEarly()
        {
            encounterEndedEarly = true;
        }

        public GoblinEncampment(int duration, int currentPlayerLevel, bool isBossEncounter = false)
        {
            this.duration = duration;
            this.isBossEncounter = isBossEncounter;

            if (currentPlayerLevel < 1)
                throw new ArgumentOutOfRangeException(nameof(currentPlayerLevel), "currentPlayerLevel must be greater than or equal to 1.");

            MobSelector = new WeightedMobSelector(new List<MobSpawnConfig>
            {
                new(() => new MobTemplates.GoblinMobs.Goblin() { Level = currentPlayerLevel }, 50),
                new(() => new MobTemplates.GoblinMobs.GoblinArcher() { Level = currentPlayerLevel }, 30),
                new(() => new MobTemplates.GoblinMobs.GoblinWarrior() { Level = currentPlayerLevel }, 20),
            });

            BossEncounter = new MobTemplates.Bosses.GoblinCaptain() { Level = currentPlayerLevel + 3 };
        }

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
            if (currentDuration > duration)
                throw new InvalidOperationException("Encounter has already ended.");

            currentDuration++;
            if (isBossEncounter && currentDuration == duration)
            {
                return this.BossEncounter;
            }

            return MobSelector.SelectMob(this);
        }

        public static GoblinEncampment FromDuration(int currentPlayerLevel, int duration)
        {
            return new GoblinEncampment(duration, currentPlayerLevel);
        }

        public static GoblinEncampment FromDurationRange(int currentPlayerLevel, int minDuration, int maxDuration)
        {
            int duration = Random.Shared.Next(minDuration, maxDuration + 1);
            return new GoblinEncampment(duration, currentPlayerLevel);
        }
    }

    public class SpringLandsEncounter : Encounter
    {
        public override string Name { get; } = "Spring Lands";
        public override string Description => "Battle with the beasts of the Spring Lands. Defeat the Den Mother to claim your reward.";
        public override int Duration => duration;
        public override int CurrentDuration => currentDuration;
        public override bool EncounterIsActive => !encounterEndedEarly && currentDuration < duration;
        public override bool EncounterEndedEarly => encounterEndedEarly;

        private readonly int duration;
        private int currentDuration = 0;
        public override WeightedMobSelector MobSelector { get; }
        public MobBase BossEncounter { get; }
        private readonly bool isBossEncounter;
        private bool encounterEndedEarly = false;
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

        public void EndEncounterEarly()
        {
            encounterEndedEarly = true;
        }

        public SpringLandsEncounter(int duration, int currentPlayerLevel, bool isBossEncounter = false)
        {
            this.duration = duration;
            this.isBossEncounter = isBossEncounter;

            if (currentPlayerLevel < 1)
                throw new ArgumentOutOfRangeException(nameof(currentPlayerLevel), "currentPlayerLevel must be greater than or equal to 1.");

            MobSelector = new WeightedMobSelector(new List<MobSpawnConfig>
                {
                    new(() => new MobTemplates.AnimalMobs.Boar() { Level = currentPlayerLevel }, 50),
                    new(() => new MobTemplates.AnimalMobs.Wolf() { Level = currentPlayerLevel }, 30),
                    new(() => new MobTemplates.AnimalMobs.Bear() { Level = currentPlayerLevel }, 20),
                });

            BossEncounter = new MobTemplates.Bosses.DenMother() { Level = currentPlayerLevel };
        }

        public override MobBase AdvanceEncounter()
        {
            if (currentDuration > duration)
                throw new InvalidOperationException("Encounter has already ended.");

            currentDuration++;
            if (isBossEncounter && currentDuration == duration)
            {
                return this.BossEncounter;
            }

            return MobSelector.SelectMob(this);
        }

        public static SpringLandsEncounter FromDuration(int currentPlayerLevel, int duration)
        {
            return new SpringLandsEncounter(duration, currentPlayerLevel);
        }

        public static SpringLandsEncounter FromDurationRange(int currentPlayerLevel, int minDuration, int maxDuration)
        {
            int duration = Random.Shared.Next(minDuration, maxDuration + 1);
            return new SpringLandsEncounter(duration, currentPlayerLevel);
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
        double durationFactor = Math.Min(encounter.Duration / 10.0, 1.0); // scale up to duration 10
        double lowWeightBoost = 1.0 + durationFactor;    // e.g. up to 2x for low weights
        double highWeightPenalty = 1.0 - 0.5 * durationFactor; // e.g. down to 0.5x for high weights

        // Interpolate factor: low weights get more boost, high weights get more penalty
        double factor = lowWeightBoost * (1 - t) + highWeightPenalty * t;

        return Math.Max(1, (int)(baseWeight * factor));
    }
}