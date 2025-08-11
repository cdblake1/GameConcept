using System.Collections.Immutable;
using GameData.src.Mob;
using GameLogic.Player;

namespace GameLogic.Mob
{
    public class MobInstance
    {
        private readonly MobDefinition mob;
        private readonly StatCollection stats;
        private readonly int level;

        public MobDefinition MobDefinition => this.mob;
        public StatCollection Stats => this.stats;
        public int Level => this.level;
        public ImmutableArray<string> Skills => [.. this.MobDefinition.Skills];

        public MobInstance(MobDefinition mob, StatCollection stats, int level)
        {
            this.mob = mob;
            this.stats = stats;
            this.level = level;
        }
    }
}