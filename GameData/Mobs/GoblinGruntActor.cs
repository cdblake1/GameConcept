namespace GameData.Mobs;

public class GoblinGruntActor : MobBase
{
    public const string NameIdentifier = "GoblinGrunt";
    public static readonly IReadOnlyList<Skill> Skills = [
        new DefaultMobAttack(),
    ];

    public static StatTemplate DefaultStats => new StatTemplate
    {
        AttackPower = 5,
        Defense = 0,
        Health = 100,
        Speed = 2
    };

    public GoblinGruntActor(MobDto goblinDto, LootTable lootTable, IReadOnlyList<Skill> skills)
        : base(goblinDto, lootTable, skills, level: 1)
    {
    }
}