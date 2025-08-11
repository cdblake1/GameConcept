using GameData.src.Effect;
using GameLogic.Ports;
using Infrastructure.Json.Dto.Effect;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Repositories
{
    public class JsonEffectRepository : IEffectRepository
    {
        private readonly string contentDirectory;
        private readonly Lazy<IReadOnlyDictionary<string, EffectDefinition>> cache;
        private static readonly IReadOnlyDictionary<string, EffectDefinition> EmptyDictionary =
                    new Dictionary<string, EffectDefinition>(0);

        public JsonEffectRepository(string contentDirectory)
        {
            this.contentDirectory = contentDirectory;
            this.cache = new Lazy<IReadOnlyDictionary<string, EffectDefinition>>(this.LoadAll);
        }
        public EffectDefinition Get(string id)
        {
            return this.cache.Value[id];
        }

        public IReadOnlyList<EffectDefinition> GetAll()
        {
            return [.. this.cache.Value.Values];
        }

        private IReadOnlyDictionary<string, EffectDefinition> LoadAll()
        {
            Dictionary<string, EffectDefinition>? dict = null;

            foreach (var file in Directory.EnumerateFiles(this.contentDirectory, "*.effect.json", SearchOption.AllDirectories))
            {
                dict ??= [];
                var json = File.ReadAllText(file);
                var dto = JsonConvert.DeserializeObject<EffectBaseDto>(json) ?? throw new InvalidOperationException($"Could not convert to EffectDto from file: {file}");
                var effect = dto.ToDomain() ?? throw new InvalidOperationException($"could not convert from dto to EffectDefinition for file: {file}");

                dict.Add(effect.Id, effect);
            }

            return dict ?? EmptyDictionary;
        }
    }
}