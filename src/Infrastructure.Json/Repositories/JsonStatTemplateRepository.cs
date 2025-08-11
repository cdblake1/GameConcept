using GameData.src.Shared.StatTemplate;
using GameData.src.Stat;
using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

public class JsonStatTemplateRepository : IStatTemplateRepository
{
    private const string searchPattern = "*.stat_template.json";
    private static readonly IReadOnlyDictionary<string, StatTemplateDefinition> EmptyDictionary = new Dictionary<string, StatTemplateDefinition>();
    private readonly string contentDirectory;
    private readonly Lazy<IReadOnlyDictionary<string, StatTemplateDefinition>> cache;

    public JsonStatTemplateRepository(string contentDirectory)
    {
        this.contentDirectory = contentDirectory;
        this.cache = new Lazy<IReadOnlyDictionary<string, StatTemplateDefinition>>(this.LoadAll);
    }

    public StatTemplateDefinition Get(string id)
    => this.cache.Value[id];

    public IReadOnlyList<StatTemplateDefinition> GetAll()
    => [.. this.cache.Value.Values];

    private IReadOnlyDictionary<string, StatTemplateDefinition> LoadAll()
    {
        Dictionary<string, StatTemplateDefinition>? dict = null;

        foreach (var file in Directory.EnumerateFiles(this.contentDirectory, searchPattern, SearchOption.AllDirectories))
        {
            dict ??= [];
            var json = File.ReadAllText(file);
            var dto = JsonConvert.DeserializeObject<StatTemplateDto>(json) ?? throw new InvalidOperationException($"Could not convert to StatTemplateDto from file: {file}");
            var statTemplate = dto.ToDomain() ?? throw new InvalidOperationException($"could not convert from dto to StatTemplate for file: {file}");

            dict.Add(statTemplate.Id, statTemplate);
        }

        return dict ?? EmptyDictionary;
    }
}