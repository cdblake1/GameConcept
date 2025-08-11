using GameData.src.LootTable;
using Infrastructure.Json.Repositories;
using Xunit;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class LootTableRepositoryTests
    {
        private const string testLootTableId = "test_loot_table";
        private readonly JsonLootTableRepository repository;
        public static readonly string LootTableDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets");

        public LootTableRepositoryTests()
        {
            this.repository = new JsonLootTableRepository(LootTableDirectoryPath);
        }

        [Fact]
        public void CanLoadLootTablesIntoRepository()
        {
            var lootTables = this.repository.GetAll();

            Assert.Single(lootTables, table => table.Id == testLootTableId);

            var lootTable = this.repository.Get(testLootTableId);

            Assert.NotNull(lootTable);
            Assert.Equal(testLootTableId, lootTable.Id);
            Assert.Single(lootTable.Groups);
            Assert.Equal(2, lootTable.Groups[0].Entries.Count);

            var firstEntry = lootTable.Groups[0].Entries[0];
            var secondEntry = lootTable.Groups[0].Entries[1];

            Assert.Equal("test_item", firstEntry.ItemId);
            Assert.Equal(500, firstEntry.Weight);
            Assert.False(firstEntry.Always);

            Assert.Equal("test_item_2", secondEntry.ItemId);
            Assert.Equal(1000, secondEntry.Weight);
            Assert.True(secondEntry.Always);
        }
    }
}