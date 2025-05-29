namespace GameData.Mobs;

public class FrogActor : MobBase
{
    public const string NameIdentifier = "Frog";

    public static readonly IReadOnlyList<Skill> Skills = [
        new DefaultMobAttack(),
    ];

    public FrogActor(MobDto frogDto, LootTable lootTable, IReadOnlyList<Skill> skills)
        : base(frogDto, lootTable, skills, level: 1)
    {
    }
}