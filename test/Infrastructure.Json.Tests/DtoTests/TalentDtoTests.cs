using Infrastructure.Json.Dto.Talent;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class TalentDtoTests
    {
        public static readonly string TalentFilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "talent_test.test.talent.json");

        [Fact]
        public void CanSerializeTalentDto()
        {
            var json = File.ReadAllText(TalentFilePath);
            var dto = JsonConvert.DeserializeObject<TalentDto>(json);

            Assert.NotNull(dto);
            Assert.True(dto.id is string);
            Assert.True(dto.actions is not null);
            Assert.True(dto.presentation is not null);
        }
    }
}