using Infrastructure.Json.Dto.Class;
using Infrastructure.Json.Dto.Talent;
using Newtonsoft.Json;

namespace Infrastructure.Json.Tests.DtoTests
{
    public class ClassDtoTests
    {
        public static readonly string ClassDtoFilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "class_test.test.class.json");

        [Fact]
        public void CanSerializeClassDto()
        {
            var json = File.ReadAllText(ClassDtoFilePath);
            var dto = JsonConvert.DeserializeObject<ClassDto>(json);

            Assert.NotNull(dto);
            Assert.NotNull(dto.skills);
            Assert.NotEmpty(dto.skills);
            Assert.NotNull(dto.talents);
            Assert.NotEmpty(dto.talents);
            Assert.NotNull(dto.skills[0].skill);
            Assert.True(dto.skills[0].level > 0);
            Assert.NotNull(dto.talents[0].id);
            Assert.NotNull(dto.talents[0].prerequisites);
            Assert.NotEmpty(dto.talents[0].prerequisites);
            Assert.NotNull(dto.talents[0].prerequisites[0]);
        }
    }
}