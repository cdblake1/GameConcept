namespace GameData.Mobs;

public class GoblinWarriorActor : MobBase
{
    public const string NameIdentifier = "Goblin Warrior";

    public static readonly IReadOnlyList<Skill> Skills = [
        new DefaultMobAttack(),
    ];

    public GoblinWarriorActor(MobDto goblinDto, LootTable lootTable, IReadOnlyList<Skill> skills)
        : base(goblinDto, lootTable, skills, level: 1)
    {
    }
}