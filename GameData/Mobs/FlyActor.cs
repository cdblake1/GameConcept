namespace GameData.Mobs;

public class FlyActor : MobBase
{
    public const string NameIdentifier = "Poison Mite Swarm";

    public static readonly IReadOnlyList<Skill> Skills = [
    new DefaultMobAttack(),
    ];

    public FlyActor(MobDto frogDto, LootTable lootTable, IReadOnlyList<Skill> skills)
         : base(frogDto, lootTable, skills, level: 1)
    {
    }

}