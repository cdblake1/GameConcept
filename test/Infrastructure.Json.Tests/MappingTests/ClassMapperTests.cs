using GameData.src.Class;
using Infrastructure.Json.Dto.Class;
using Infrastructure.Json.Mappers;

namespace Infrastructure.Json.Tests.MappingTests
{
    public class ClassMapperTests
    {
        [Fact]
        public void CanMapTalentNodeDto()
        {
            var dto = new TalentNodeDto()
            {
                id = "talent_test",
                prerequisites = ["talent_test_2"],
                talent_tier = TalentNodeDto.Tier.tier_05
            };

            var talentNode = dto.ToDomain();

            Assert.Equal(dto.id, talentNode.Id);
            Assert.Equal(dto.prerequisites.Length, talentNode.Prerequisites.Count);

            Assert.Equal(dto.prerequisites[0], talentNode.Prerequisites[0]);

            Assert.Equal(TalentTier.Tier05, talentNode.Tier);
        }

        [Fact]
        public void CanMapSkillEntry()
        {
            var dto = new ClassDto.SkillEntryDto()
            {
                level = 1,
                skill = "test_skill"
            };

            var skillEntry = dto.ToDomain();

            Assert.Equal(dto.level, skillEntry.Level);
            Assert.Equal(dto.skill, skillEntry.Id);
        }

        [Fact]
        public void CanMapClassDto()
        {
            var classDto = new ClassDto()
            {
                id = "class_test",
                presentation = new()
                {
                    description = "test class",
                    name = "Test Class"
                },
                skills = [new ClassDto.SkillEntryDto() {
                    level = 1,
                    skill = "test_skill"
                }, new ClassDto.SkillEntryDto() {
                    level = 1,
                    skill = "test_skill_2"
                }],
                talents = [
                    new TalentNodeDto() {id = "test_talent_1", prerequisites = [], talent_tier = TalentNodeDto.Tier.tier_05},
                    new TalentNodeDto() {id = "test_talent_1", prerequisites = ["test_talent_1"], talent_tier = TalentNodeDto.Tier.tier_10 }
                ]
            };

            var classDefinition = classDto.ToDomain();

            Assert.Equal(classDto.id, classDefinition.Id);
            Assert.Equal(classDto.talents.Length, classDefinition.Talents.Count);

            Assert.Equal(classDto.talents[0].id, classDefinition.Talents[0].Id);
            Assert.Equal(0, classDefinition.Talents[0].Prerequisites?.Count);
            Assert.Equal(TalentTier.Tier05, classDefinition.Talents[0].Tier);

            Assert.Equal(classDto.talents[1].id, classDefinition.Talents[1].Id);
            Assert.Equal(1, classDefinition.Talents[1].Prerequisites?.Count);
            Assert.Equal(classDto.talents[1].prerequisites[0], classDefinition.Talents[1].Prerequisites[0]);
            Assert.Equal(TalentTier.Tier10, classDefinition.Talents[1].Tier);


            Assert.Equal(classDto.skills.Length, classDefinition.SkillEntries.Count);
            Assert.Equal(classDto.skills[0].skill, classDefinition.SkillEntries[0].Id);
            Assert.Equal(classDto.skills[0].level, classDefinition.SkillEntries[0].Level);
            Assert.Equal(classDto.skills[1].skill, classDefinition.SkillEntries[1].Id);
            Assert.Equal(classDto.skills[1].level, classDefinition.SkillEntries[1].Level);
        }
    }
}