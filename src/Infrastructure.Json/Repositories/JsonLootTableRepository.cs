using GameData.src.LootTable;
using GameLogic.Ports;
using Infrastructure.Json.Dto.LootTable;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

public class JsonLootTableRepository : ILootTableRepository
{
    private const string searchPattern = "*.loot_table.json";
    private readonly string contentDirectory;
    private readonly Lazy<IReadOnlyDictionary<string, LootTableDefinition>> cache;

    public JsonLootTableRepository(string contentDirectory)
    {
        this.contentDirectory = contentDirectory;
        this.cache = new(this.LoadAll);
    }

    public LootTableDefinition Get(string id)
    {
        return this.cache.Value[id];
    }

    public IReadOnlyList<LootTableDefinition> GetAll()
    {
        return [.. this.cache.Value.Values];
    }

    private IReadOnlyDictionary<string, LootTableDefinition> LoadAll()
    {
        Dictionary<string, LootTableDefinition>? dict = null;

        foreach (var file in Directory.EnumerateFiles(this.contentDirectory, searchPattern, SearchOption.AllDirectories))
        {
            dict ??= [];
            var json = File.ReadAllText(file);
            var dto = JsonConvert.DeserializeObject<LootTableDto>(json) ?? throw new InvalidOperationException($"Could not convert to LootTableDto from file: {file}");
            var lootTable = dto.ToDomain() ?? throw new InvalidOperationException($"could not convert from dto to LootTableDefinition for file: {file}");

            dict.Add(lootTable.Id, lootTable);
        }

        return dict ?? [];
    }
}