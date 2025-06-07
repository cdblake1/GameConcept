#nullable enable

using GameData;
using GameData.Mobs;

public class MobFactory
{
    private static readonly Dictionary<Type, Func<MobBase>> MobConstructors = new();

    public static MobFactory Instance => instance;
    private static MobFactory instance = new();

    public void Compose()
    {
        this.Register(typeof(BearActor), () => new BearActor(
            new(BearActor.NameIdentifier, new StatTemplateOld()
            {
                AttackPower = 10,
                Defense = 0,
                Health = 200,
                Speed = 3
            }),
            LootTableConcretes.LowLevelLootTable,
            BearActor.Skills));

        this.Register(typeof(BoarActor), () => new BoarActor(
                    new(BoarActor.NameIdentifier, new StatTemplateOld()
                    {
                        AttackPower = 10,
                        Defense = 0,
                        Health = 100,
                        Speed = 3
                    }),
                    LootTableConcretes.LowLevelLootTable,
                    BoarActor.Skills));

        this.Register(typeof(DenMotherActor), () => new DenMotherActor(
            new(DenMotherActor.NameIdentifier, new StatTemplateOld()
            {
                AttackPower = 25,
                Defense = 15,
                Health = 200,
                Speed = 3
            }),
            LootTableConcretes.LowLevelBossLootTable,
            DenMotherActor.Skills));

        this.Register(typeof(FlyActor),
            () => new FlyActor(
            new(FlyActor.NameIdentifier, new StatTemplateOld()
            {
                AttackPower = 10,
                Defense = 0,
                Health = 30,
                Speed = 3
            }),
            LootTableConcretes.LowLevelLootTable,
            FlyActor.Skills));

        this.Register(typeof(FrogActor), () => new FrogActor(
            new(FrogActor.NameIdentifier, new StatTemplateOld()
            {
                AttackPower = 5,
                Defense = 0,
                Health = 100,
                Speed = 2
            }),
            LootTableConcretes.LowLevelLootTable,
            FrogActor.Skills));

        this.Register(typeof(GoblinCaptainActor), () => new GoblinCaptainActor(
             new(GoblinCaptainActor.NameIdentifier, new StatTemplateOld()
             {
                 AttackPower = 25,
                 Defense = 15,
                 Health = 200,
                 Speed = 2
             }),
             LootTableConcretes.LowLevelBossLootTable,
             GoblinCaptainActor.Skills));

        this.Register(typeof(GoblinGruntActor), () => new GoblinGruntActor(
            new MobDto(GoblinGruntActor.NameIdentifier, GoblinGruntActor.DefaultStats),
            LootTableConcretes.LowLevelLootTable,
            GoblinGruntActor.Skills
        ));

        this.Register(typeof(GoblinRangerActor), () => new GoblinRangerActor(
            new(GoblinRangerActor.NameIdentifier, new StatTemplateOld()
            {
                AttackPower = 15,
                Defense = 0,
                Health = 120,
                Speed = 5
            }),
            LootTableConcretes.LowLevelLootTable,
            GoblinRangerActor.Skills));

        this.Register(typeof(GoblinWarriorActor), () => new GoblinWarriorActor(
             new(GoblinWarriorActor.NameIdentifier, new StatTemplateOld()
             {
                 AttackPower = 10,
                 Defense = 0,
                 Health = 150,
                 Speed = 1
             }),
             LootTableConcretes.LowLevelLootTable,
             GoblinWarriorActor.Skills));

        this.Register(typeof(OrcActor), () => new OrcActor(
              new(OrcActor.NameIdentifier, new StatTemplateOld()
              {
                  AttackPower = 15,
                  Defense = 10,
                  Health = 150,
                  Speed = 1
              }),
              LootTableConcretes.LowLevelLootTable,
              OrcActor.Skills));

        this.Register(typeof(PoisonMiteSwarmActor),
            () => new PoisonMiteSwarmActor(
            new(PoisonMiteSwarmActor.NameIdentifier, new StatTemplateOld()
            {
                AttackPower = 1,
                Defense = 0,
                Health = 50,
                Speed = 4
            }),
            LootTableConcretes.LowLevelLootTable,
            PoisonMiteSwarmActor.Skills));

        this.Register(typeof(TrollActor), () => new TrollActor(
            new(TrollActor.NameIdentifier, new StatTemplateOld()
            {
                AttackPower = 20,
                Defense = 10,
                Health = 300,
                Speed = 1
            }),
            LootTableConcretes.LowLevelLootTable,
            TrollActor.Skills));

        this.Register(typeof(WolfActor), () => new WolfActor(
            new(WolfActor.NameIdentifier, new StatTemplateOld()
            {
                AttackPower = 15,
                Defense = 0,
                Health = 80,
                Speed = 5
            }),
            LootTableConcretes.LowLevelLootTable,
            WolfActor.Skills
        ));
    }

    public void Register(Type type, Func<MobBase> constructor)
    {
        if (MobConstructors.ContainsKey(type))
        {
            throw new ArgumentException($"Mob type '{type.Name}' is already registered.");
        }

        MobConstructors[type] = constructor;
    }

    public MobBase Create(Type type)
    {
        if (MobConstructors.TryGetValue(type, out var factory))
        {
            return factory();
        }

        throw new ArgumentException($"Mob type '{type.Name}' is not registered.");
    }
}