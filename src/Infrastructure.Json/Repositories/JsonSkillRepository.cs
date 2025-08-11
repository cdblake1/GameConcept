using GameData.src.Skill;
using GameLogic.Ports;
using Infrastructure.Json.Dto.Skill;
using Infrastructure.Json.Mappers;
using Newtonsoft.Json;

namespace Infrastructure.Json.Repositories
{
    public class JsonSkillRepository : ISkillRepository
    {
        private const string searchPattern = "*.skill.json";
        private readonly string contentDirectory;
        private readonly Lazy<IReadOnlyDictionary<string, SkillDefinition>> cache;

        private static readonly IReadOnlyDictionary<string, SkillDefinition> EmptyDictionary =
                           new Dictionary<string, SkillDefinition>(0);

        public JsonSkillRepository(string contentDirectory)
        {
            this.contentDirectory = contentDirectory;
            this.cache = new(this.LoadAll);
        }

        public SkillDefinition Get(string id)
        {
            return this.cache.Value[id];
        }

        public IReadOnlyList<SkillDefinition> GetAll()
        {
            return [.. this.cache.Value.Values];
        }

        private IReadOnlyDictionary<string, SkillDefinition> LoadAll()
        {
            Dictionary<string, SkillDefinition>? dict = null;

            foreach (var file in Directory.EnumerateFiles(this.contentDirectory, searchPattern, SearchOption.AllDirectories))
            {
                dict ??= [];
                var json = File.ReadAllText(file);
                var dto = JsonConvert.DeserializeObject<SkillDto>(json) ?? throw new InvalidOperationException($"Could not convert to SkillDto from file: {file}");
                var skill = dto.ToDomain() ?? throw new InvalidOperationException($"could not convert from dto to SkillDefinition for file: {file}");

                dict.Add(skill.Id, skill);
            }

            return dict ?? EmptyDictionary;
        }
    }
}