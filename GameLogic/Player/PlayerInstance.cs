using GameData;
using GameData.src.Class;
using GameData.src.Effect.Talent;
using GameData.src.Player;
using GameData.src.Skill;
using GameLogic.Inventory;

namespace GameLogic.Player
{
    public class PlayerInstance
    {
        private readonly string name;
        private readonly StatCollection stats;
        private readonly PlayerDefinition playerDefinition;
        private readonly int level;
        private readonly List<TalentDefinition> selectedTalents;
        private readonly List<SkillDefinition> selectedSkills;

        public EquipmentManager Equipment { get; init; }
        public InventoryManager Inventory { get; init; }
        public LevelManager LevelManager { get; set; }
        public StatCollection Stats => this.stats;
        public int Level => this.level;

        public PlayerInstance(string name, PlayerDefinition playerDefinition, int startingLevel)
        {
            this.name = name;
            this.playerDefinition = playerDefinition;
            this.Equipment = new();
            this.Inventory = new();
            this.stats = new();
            this.selectedSkills = new();
            this.selectedTalents = new();
            this.LevelManager = new(
                maxLevel: Globals.MaxLevel,
                experienceTable: ExperienceTable.PlayerExpTable,
                startingLevel: startingLevel
            );
        }
        public PlayerInstance(string name, PlayerDefinition playerDefinition) : this(name, playerDefinition, 1) { }

        public ClassDefinition ClassDefinition => this.playerDefinition.ClassDefinition;
        public List<TalentDefinition> SelectedTalents => this.selectedTalents;
        public List<SkillDefinition> SelectedSkills => this.selectedSkills;
        public PlayerDefinition PlayerDefinition => this.playerDefinition;

        public void AddSelectedSkill(SkillDefinition skill)
        {
            this.selectedSkills.Add(skill);
        }

        public void AddTalent(TalentDefinition definition)
        {
            this.selectedTalents.Add(definition);
        }

        public IReadOnlyList<SkillDefinition> GetSelectedSkills() => this.selectedSkills;
        public IReadOnlyList<TalentDefinition> GetSelectedTalents() => this.selectedTalents;

        public void EquipItem(string item)
        {
            //TODO(Caleb): Add logic to equip item and remove from inventory.
            throw new NotImplementedException();
        }

        public void UnequipItem(string item)
        {
            //TODO(Caleb): Add logic to unequip item and add to the inventory.
            //TODO(Caleb): consider merging Equipment into the inventory manager.
            throw new NotImplementedException();
        }

    }
}