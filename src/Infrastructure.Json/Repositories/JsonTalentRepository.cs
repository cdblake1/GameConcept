using GameData.src.Effect.Talent;
using GameLogic.Ports;
using Infrastructure.Json.Dto.Talent;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Repositories
{
    public class JsonTalentRepository : ITalentRepository
    {
        private readonly string contentDirectory;
        private readonly Lazy<IReadOnlyDictionary<string, TalentDefinition>> cache;
        private static readonly IReadOnlyDictionary<string, TalentDefinition> EmptyDictionary =
                    new Dictionary<string, TalentDefinition>(0);

        public JsonTalentRepository(string contentDirectory)
        {
            this.contentDirectory = contentDirectory;
            this.cache = new(this.LoadAll);
        }
        public TalentDefinition Get(string id)
        {
            return this.cache.Value[id];
        }

        public IReadOnlyList<TalentDefinition> GetAll()
        {
            return [.. this.cache.Value.Values];
        }

        private IReadOnlyDictionary<string, TalentDefinition> LoadAll()
        {
            Dictionary<string, TalentDefinition>? dict = null;

            foreach (var file in Directory.EnumerateFiles(this.contentDirectory, "*.talent.json", SearchOption.AllDirectories))
            {
                dict ??= [];
                var json = File.ReadAllText(file);
                var dto = JsonConvert.DeserializeObject<TalentDto>(json) ?? throw new InvalidOperationException($"Could not convert to EffectDto from file: {file}");
                var effect = dto.ToDomain() ?? throw new InvalidOperationException($"could not convert from dto to TalentDefinition for file: {file}");

                dict.Add(effect.Id, effect);
            }

            return dict ?? EmptyDictionary;
        }
    }
}