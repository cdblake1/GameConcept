using GameData.src.Mob;
using GameLogic.Ports;
using Infrastructure.Json.Dto.Mob;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

public class JsonMobRepository : IMobRepository
{
    private const string searchPattern = "*.mob.json";
    private readonly string contentDirectory;
    private readonly Lazy<IReadOnlyDictionary<string, MobDefinition>> cache;

    public JsonMobRepository(string contentDirectory)
    {
        this.contentDirectory = contentDirectory;
        this.cache = new(this.LoadAll);
    }

    public MobDefinition Get(string id)
    {
        return this.cache.Value[id];
    }

    public IReadOnlyList<MobDefinition> GetAll()
    {
        return [.. this.cache.Value.Values];
    }

    private IReadOnlyDictionary<string, MobDefinition> LoadAll()
    {
        Dictionary<string, MobDefinition>? dict = null;

        foreach (var file in Directory.EnumerateFiles(this.contentDirectory, searchPattern, SearchOption.AllDirectories))
        {
            dict ??= [];
            var json = File.ReadAllText(file);
            var dto = JsonConvert.DeserializeObject<MobDto>(json) ?? throw new InvalidOperationException($"Could not convert to MobDto from file: {file}");
            var mob = dto.ToDomain() ?? throw new InvalidOperationException($"could not convert from dto to MobDefinition for file: {file}");

            dict.Add(mob.Id, mob);
        }

        return dict ?? [];
    }
}