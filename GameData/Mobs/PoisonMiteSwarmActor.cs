
namespace GameData.Mobs;

public class PoisonMiteSwarmActor : MobBase
{
    public const string NameIdentifier = "Poison Mite Swarm";

    public static readonly IReadOnlyList<Skill> Skills = [
    new DefaultMobAttack(),
    ];

    public PoisonMiteSwarmActor(MobDto frogDto, LootTable lootTable, IReadOnlyList<Skill> skills)
         : base(frogDto, lootTable, skills, level: 1)
    {
    }
}