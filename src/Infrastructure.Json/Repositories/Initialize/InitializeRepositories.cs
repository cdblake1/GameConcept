using GameLogic.Ports;
using Infrastructure.Json.Repositories;

namespace Infrastructure.Json.Repositories.Initialize
{
    public static class Repositories
    {
        private static readonly string ClassesFilePath = Path.Combine("src", "Content", "Classes");
        private static readonly string EncounterFilePath = Path.Combine("src", "Content", "Encounter");
        private static readonly string SkillsFilePath = Path.Combine("src", "Content", "Skills");
        private static readonly string StatTemplateFilePath = Path.Combine("src", "Content", "StatTemplate");
        private static readonly string MobFilePath = Path.Combine("src", "Content", "Mob");
        private static readonly string CraftingRecipeFilePath = Path.Combine("src", "Content", "CraftingRecipe");
        private static readonly string ItemFilePath = Path.Combine("src", "Content", "Item");
        private static readonly string LootTableFilePath = Path.Combine("src", "Content", "LootTable");
        private static readonly string ExpTableFilePath = Path.Combine("src", "Content", "ExpTable");
        private static readonly string EffectsFilePath = Path.Combine("src", "Content", "Effects");
        private static readonly string TalentsFilePath = Path.Combine("src", "Content", "Talents");

        public static IClassRepository ClassRepository { get; } = new JsonClassRepository(ClassesFilePath);
        public static IEncounterRepository EncounterRepository { get; } = new JsonEncounterRepository(EncounterFilePath);
        public static ISkillRepository SkillRepository { get; } = new JsonSkillRepository(SkillsFilePath);
        public static IStatTemplateRepository StatTemplateRepository { get; } = new JsonStatTemplateRepository(StatTemplateFilePath);
        public static IMobRepository MobRepository { get; } = new JsonMobRepository(MobFilePath);
        public static ICraftingRecipeRepository CraftingRecipeRepository { get; } = new JsonCraftingRecipeRepository(CraftingRecipeFilePath);
        public static IItemRepository ItemRepository { get; } = new JsonItemRepository(ItemFilePath);
        public static ILootTableRepository LootTableRepository { get; } = new JsonLootTableRepository(LootTableFilePath);
        public static IExpTableRepository ExpTableRepository { get; } = new JsonExpTableRepository(ExpTableFilePath);
        public static IEffectRepository EffectRepository { get; } = new JsonEffectRepository(EffectsFilePath);
        public static ITalentRepository TalentRepository { get; } = new JsonTalentRepository(TalentsFilePath);
    }
}