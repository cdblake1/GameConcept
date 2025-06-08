using Infrastructure.Json.Dto.Common;
using Infrastructure.Json.Dto.Skill;
using Infrastructure.Json.Dto.Skill.SkillStep;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class SkillDtoTests
    {
        public static readonly string HitDamageFilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "hit_damage.test.skill.json");

        [Fact]
        public void CanSerializeSkillDto()
        {
            var json = File.ReadAllText(HitDamageFilePath);
            var dto = JsonConvert.DeserializeObject<SkillDto>(json);

            Assert.NotNull(dto);
            Assert.True(dto.id is string);
            Assert.True(dto.effects is SkillStepDto[]);
            Assert.True(dto.presentation is PresentationDto);
            Assert.True(dto.activation_req is ActivationRequirementDto);
        }
    }
}