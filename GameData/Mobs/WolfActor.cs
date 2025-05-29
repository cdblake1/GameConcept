namespace GameData.Mobs;

public class WolfActor : MobBase
{
    public const string NameIdentifier = "Wolf";

    public static readonly IReadOnlyList<Skill> Skills = [
        new DefaultMobAttack(),
    ];

    public WolfActor(MobDto wolfDto, LootTable lootTable, IReadOnlyList<Skill> skills)
        : base(wolfDto, lootTable, skills, level: 1)
    {
    }
}