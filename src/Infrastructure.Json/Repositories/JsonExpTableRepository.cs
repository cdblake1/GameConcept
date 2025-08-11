using GameData.src.ExpTable;
using GameLogic.Ports;
using Infrastructure.Json.Dto.ExpTable;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Repositories
{
    public class JsonExpTableRepository : IExpTableRepository
    {
        private const string searchPattern = "*.exp_table.json";
        private readonly string contentDirectory;
        private readonly Lazy<IReadOnlyDictionary<string, ExpTableDefinition>> cache;

        private static readonly IReadOnlyDictionary<string, ExpTableDefinition> EmptyDictionary =
                           new Dictionary<string, ExpTableDefinition>(0);

        public JsonExpTableRepository(string contentDirectory)
        {
            this.contentDirectory = contentDirectory;
            this.cache = new(this.LoadAll);
        }

        public ExpTableDefinition Get(string id)
        {
            return this.cache.Value.TryGetValue(id, out var expTable) ? expTable : throw new KeyNotFoundException($"Exp table with id {id} not found");
        }

        public IReadOnlyList<ExpTableDefinition> GetAll()
        {
            return this.cache.Value.Values.ToList();
        }

        private IReadOnlyDictionary<string, ExpTableDefinition> LoadAll()
        {
            Dictionary<string, ExpTableDefinition>? dict = null;

            foreach (var file in Directory.EnumerateFiles(this.contentDirectory, searchPattern, SearchOption.AllDirectories))
            {
                dict ??= [];
                var json = File.ReadAllText(file);
                var dto = JsonConvert.DeserializeObject<ExpTableDto>(json) ?? throw new InvalidOperationException($"Could not deserialize ExpTableDto from file: {file}");
                var expTable = dto.ToDomain();
                dict.Add(expTable.Id, expTable);
            }

            return dict ?? EmptyDictionary;
        }
    }
}