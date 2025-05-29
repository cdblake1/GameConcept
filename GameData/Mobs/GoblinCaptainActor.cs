
namespace GameData.Mobs;

public class GoblinCaptainActor : MobBase
{
    public const string NameIdentifier = "GoblinCaptain";

    public static readonly IReadOnlyList<Skill> Skills = [
        new DefaultMobAttack(),
    ];

    public GoblinCaptainActor(MobDto goblinCaptainDto, LootTable lootTable, IReadOnlyList<Skill> skills)
        : base(goblinCaptainDto, lootTable, skills, level: 1)
    {
    }
}