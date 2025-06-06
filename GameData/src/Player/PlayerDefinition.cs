using GameData.src.Class;
using GameData.src.Effect.Talent;

namespace GameData.src.Player
{
    public class Player(StatTemplate baseStats, ClassDefinition classDefinition, int level)
    {
        public StatTemplate BaseStats { get; } = baseStats;
        public List<TalentDefinition> Talents => talents;
        public List<TalentDefinition> talents = [];
        public int Level => level;
        public int level = level;
        public ClassDefinition ClassDefinition { get; } = classDefinition;
    }
}