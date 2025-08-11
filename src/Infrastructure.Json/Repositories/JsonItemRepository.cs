using GameData.src.Item;
using GameLogic.Ports;
using Infrastructure.Json.Dto.Item;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

public class JsonItemRepository : IItemRepository
{
    private const string searchPattern = "*.item.json";
    private readonly string contentDirectory;
    private readonly Lazy<IReadOnlyDictionary<string, IItemDefinition>> cache;

    public JsonItemRepository(string contentDirectory)
    {
        this.contentDirectory = contentDirectory;
        this.cache = new(this.LoadAll);
    }

    public IItemDefinition Get(string id)
    {
        return this.cache.Value[id];
    }

    public IReadOnlyList<IItemDefinition> GetAll()
    {
        return [.. this.cache.Value.Values];
    }

    private IReadOnlyDictionary<string, IItemDefinition> LoadAll()
    {
        Dictionary<string, IItemDefinition>? dict = null;

        foreach (var file in Directory.EnumerateFiles(this.contentDirectory, searchPattern, SearchOption.AllDirectories))
        {
            dict ??= [];
            var json = File.ReadAllText(file);
            var dto = JsonConvert.DeserializeObject<IItemDto>(json) ?? throw new InvalidOperationException($"Could not convert to ItemDto from file: {file}");
            var item = dto.ToDomain() ?? throw new InvalidOperationException($"could not convert from dto to ItemDefinition for file: {file}");

            dict.Add(item.Id, item);
        }

        return dict ?? [];
    }
}