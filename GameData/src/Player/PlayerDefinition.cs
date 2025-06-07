using GameData.src.Class;
using GameData.src.Effect.Talent;

namespace GameData.src.Player
{
    public class Player(StatTemplateOld baseStats, ClassDefinition classDefinition, int level)
    {
        public StatTemplateOld BaseStats { get; } = baseStats;
        public List<TalentDefinition> Talents => talents;
        public List<TalentDefinition> talents = [];
        public int Level => level;
        public int level = level;
        public ClassDefinition ClassDefinition { get; } = classDefinition;
    }
}