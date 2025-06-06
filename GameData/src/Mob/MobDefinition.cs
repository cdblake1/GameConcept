using GameData.src.Skill;

namespace GameData.src.Mob
{
    public record Mob(StatTemplate BaseStats, List<string> Skills, int Level);
}