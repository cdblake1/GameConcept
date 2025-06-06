using GameData.src.Class;
using GameLogic.Ports;
using Infrastructure.Json.Dto.Class;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Repositories
{
    public class JsonClassRepository : IClassRepository
    {
        private readonly string contentDirectory;
        private readonly Lazy<IReadOnlyDictionary<string, ClassDefinition>> cache;
        private static readonly IReadOnlyDictionary<string, ClassDefinition> EmptyDictionary =
                    new Dictionary<string, ClassDefinition>(0);

        public JsonClassRepository(string contentDirectory)
        {
            this.contentDirectory = contentDirectory;
            this.cache = new Lazy<IReadOnlyDictionary<string, ClassDefinition>>(LoadAll);
        }
        public ClassDefinition Get(string id)
        {
            return this.cache.Value[id];
        }

        public IReadOnlyList<ClassDefinition> GetAll()
        {
            return [.. this.cache.Value.Values];
        }

        private IReadOnlyDictionary<string, ClassDefinition> LoadAll()
        {
            Dictionary<string, ClassDefinition>? dict = null;

            foreach (var file in Directory.EnumerateFiles(contentDirectory, "*.class.json", SearchOption.AllDirectories))
            {
                dict ??= [];
                var json = File.ReadAllText(file);
                var dto = JsonConvert.DeserializeObject<ClassDto>(json) ?? throw new InvalidOperationException($"Could not convert to EffectDto from file: {file}");
                var effect = dto.ToDomain() ?? throw new InvalidOperationException($"could not convert from dto to ClassDefinition for file: {file}");

                dict.Add(effect.Id, effect);
            }

            return dict ?? EmptyDictionary;
        }
    }
}