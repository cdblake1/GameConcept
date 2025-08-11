using GameData.src.ExpTable;
using Infrastructure.Json.Repositories;
using Xunit;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class ExpTableRepositoryTests
    {
        private const string testExpTableId = "test_xp_table";
        private readonly JsonExpTableRepository repository;
        public static readonly string ExpTableDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets");

        public ExpTableRepositoryTests()
        {
            this.repository = new JsonExpTableRepository(ExpTableDirectoryPath);
        }

        [Fact]
        public void CanLoadExpTablesIntoRepository()
        {
            var expTables = this.repository.GetAll();

            Assert.Single(expTables, table => table.Id == testExpTableId);

            var expTable = this.repository.Get(testExpTableId);

            Assert.NotNull(expTable);
            Assert.Equal(testExpTableId, expTable.Id);
            Assert.Equal(50, expTable.Entries.Length);

            // Test first level
            var firstLevel = expTable.Entries[0];
            Assert.Equal(1, firstLevel.Level);
            Assert.Equal(437, firstLevel.Exp);

            // Test middle level
            var middleLevel = expTable.Entries[24];
            Assert.Equal(25, middleLevel.Level);
            Assert.Equal(2523, middleLevel.Exp);

            // Test last level
            var lastLevel = expTable.Entries[49];
            Assert.Equal(50, lastLevel.Level);
            Assert.Equal(142168, lastLevel.Exp);
        }
    }
}