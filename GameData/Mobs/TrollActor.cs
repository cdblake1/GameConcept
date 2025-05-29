namespace GameData.Mobs;

public class TrollActor : MobBase
{
    public const string NameIdentifier = "Troll";

    public static readonly IReadOnlyList<Skill> Skills = [
        new DefaultMobAttack(),
    ];

    public TrollActor(MobDto trollDto, LootTable lootTable, IReadOnlyList<Skill> skills)
        : base(trollDto, lootTable, skills, level: 1)
    {
    }
}