using GameData.src.Class;
using Infrastructure.Json.Repositories;
using Xunit;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class ClassRepositoryTests
    {
        private const string classId = "test_class";
        private readonly JsonClassRepository repository;
        public static readonly string ClassDtoDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Classes");

        public ClassRepositoryTests()
        {
            this.repository = new JsonClassRepository(ClassDtoDirectoryPath);
        }
        [Fact]
        public void CanLoadClassesIntoRepository()
        {

            var classes = this.repository.GetAll();

            Assert.Single(classes, c1 => c1.Id == classId);

            var class1 = this.repository.Get(classId);

            Assert.Equal(classId, class1.Id);
            Assert.Single(class1.SkillEntries);
            Assert.Equal("test_skill", class1.SkillEntries[0].Id);
            Assert.Equal(10, class1.SkillEntries[0].Level);

            Assert.Single(class1.Talents);
            Assert.Equal("test_talent_one", class1.Talents[0].Id);
            Assert.Equal(TalentTier.Tier05, class1.Talents[0].Tier);
            Assert.Single(class1.Talents[0].Prerequisites);
            Assert.Equal("test_talent_two", class1.Talents[0].Prerequisites[0]);



        }
    }
}