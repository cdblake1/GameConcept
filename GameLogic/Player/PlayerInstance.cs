using System.Collections.Immutable;
using GameData.src.Class;
using GameData.src.Player;

namespace GameLogic.Player
{
    public class PlayerInstance
    {
        private readonly PlayerDefinition playerDefinition;
        private readonly StatCollection stats;
        private readonly int level;
        private int[] selectedTalents = [];
        private int[] selectedSkills = [];

        public ClassDefinition ClassDefinition => this.playerDefinition.ClassDefinition;
        public StatCollection Stats => this.stats;
        public int Level => this.level;

        public PlayerInstance(
            PlayerDefinition playerDefinition,
            StatCollection statCollection,
            int level)
        {
            this.stats = new StatCollection();
            this.playerDefinition = playerDefinition;
            this.stats = statCollection;
            this.level = level;
        }

        public string[] GetTalents()
        {
            var allTalents = this.ClassDefinition.Talents;

            var talents = new string[this.selectedTalents.Length];
            for (int i = 0; i < this.selectedTalents.Length; i++)
            {
                var idx = this.selectedTalents[i];
                talents[i] = allTalents[idx].Id;
            }

            return talents;
        }

        public string[] GetSkills()
        {
            var skillIds = new List<string>();

            for (int i = 0; i < this.playerDefinition.ClassDefinition.SkillEntries.Count; i++)
            {
                var entry = this.playerDefinition.ClassDefinition.SkillEntries[i];
                if (this.level >= entry.Level)
                {
                    skillIds.Add(this.ClassDefinition.SkillEntries[i].Id);
                }
            }
            // for (int i = 0; i < this.selectedSkills.Length; i++)
            // {
            //     skillIds[i] = this.ClassDefinition.SkillEntries[this.selectedSkills[i]].Id;
            // }

            return skillIds.ToArray();
        }
    }
}