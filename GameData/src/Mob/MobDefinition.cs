using GameData.src.Skill;

namespace GameData.src.Mob
{
    public record Mob(StatTemplateOld BaseStats, List<string> Skills, int Level);
}