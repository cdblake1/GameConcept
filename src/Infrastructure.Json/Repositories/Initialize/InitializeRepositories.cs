using GameLogic.Ports;
using Infrastructure.Json.Repositories;

namespace Infrastructure.Json.Repositories.Initialize
{
    public static class Repositories
    {
        private static readonly string ClassesFilePath = Path.Combine("Content", "Classes");
        private static readonly string EncounterFilePath = Path.Combine("Content", "Encounter");
        private static readonly string SkillsFilePath = Path.Combine("Content", "Skills");
        private static readonly string StatTemplateFilePath = Path.Combine("Content", "StatTemplate");
        private static readonly string MobFilePath = Path.Combine("Content", "Mob");
        private static readonly string CraftingRecipeFilePath = Path.Combine("Content", "CraftingRecipe");
        private static readonly string ItemFilePath = Path.Combine("Content", "Item");
        private static readonly string LootTableFilePath = Path.Combine("Content", "LootTable");
        private static readonly string ExpTableFilePath = Path.Combine("Content", "ExpTable");
        private static readonly string EffectsFilePath = Path.Combine("Content", "Effects");
        private static readonly string TalentsFilePath = Path.Combine("Content", "Talents");

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
