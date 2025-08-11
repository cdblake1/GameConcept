using GameData.src.Encounter;
using GameLogic.Ports;
using Infrastructure.Json.Dto.Encounter;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Repositories
{
    public class JsonEncounterRepository : IEncounterRepository
    {
        private const string searchPattern = "*.encounter.json";
        private readonly string contentDirectory;
        private readonly Lazy<IReadOnlyDictionary<string, EncounterDefinition>> cache;

        public JsonEncounterRepository(string contentDirectory)
        {
            this.contentDirectory = contentDirectory;
            this.cache = new Lazy<IReadOnlyDictionary<string, EncounterDefinition>>(this.LoadAll);
        }

        public EncounterDefinition Get(string id)
        {
            return this.cache.Value[id];
        }

        public IReadOnlyList<EncounterDefinition> GetAll()
        {
            return [.. this.cache.Value.Values];
        }

        private IReadOnlyDictionary<string, EncounterDefinition> LoadAll()
        {
            Dictionary<string, EncounterDefinition>? dict = null;

            foreach (var file in Directory.EnumerateFiles(this.contentDirectory, searchPattern, SearchOption.AllDirectories))
            {
                dict ??= [];
                var json = File.ReadAllText(file);
                var dto = JsonConvert.DeserializeObject<EncounterDto>(json) ?? throw new InvalidOperationException($"Could not convert to EncounterDto from file: {file}");
                var encounter = dto.ToDomain() ?? throw new InvalidOperationException($"could not convert from dto to EncounterDefinition for file: {file}");

                dict.Add(encounter.Id, encounter);
            }

            return dict ?? [];
        }
    }
}