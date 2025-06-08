using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill.SkillStep;
using GameData.src.Talent.TalentActions;
using Infrastructure.Json.Repositories;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class TalentRepositoryTests
    {
        private const string talentId = "test_talent_one";
        private readonly JsonTalentRepository repository;
        public static readonly string TalentDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Talents");

        public TalentRepositoryTests()
        {
            this.repository = new JsonTalentRepository(TalentDirectoryPath);
        }

        [Fact]
        public void CanLoadTalentIntoRepository()
        {
            var talents = this.repository.GetAll();

            Assert.Single(talents, talent => talent.Id == talentId);

            var talent = this.repository.Get(talentId);

            Assert.NotNull(talent);
            Assert.Equal(3, talent.Actions.Count);
            Assert.Equal(talentId, talent.Id);

            var modOne = talent.Actions[0] as ModifyDotDamageAction;
            var modTwo = talent.Actions[1] as ModifyEffectAction;
            var modThree = talent.Actions[2] as ModifySkillAction;

            Assert.NotNull(modOne);

            Assert.Equal("test_skill", modOne.SkillId);

            Assert.NotNull(modOne.MinBaseDamage);
            Assert.Equal(1, modOne.MinBaseDamage.ScaleAdded);
            Assert.NotNull(modOne.MaxBaseDamage);
            Assert.Equal(1, modOne.MaxBaseDamage.ScaleAdded);

            Assert.NotNull(modOne.Crit);
            Assert.True(modOne.Crit);

            Assert.NotNull(modOne.DamageTypes);
            Assert.Equal(CollectionOperationKind.Add, modOne.DamageTypes.Operation.Operation);
            Assert.Single(modOne.DamageTypes.Operation.Items);

            Assert.NotNull(modOne.Duration);
            Assert.True(modOne.Duration.Permanent);

            Assert.NotNull(modOne.Frequency);
            Assert.Equal(1, modOne.Frequency.ScaleAdded);

            Assert.NotNull(modOne.StackStrategy);
            Assert.Equal(1, modOne.StackStrategy.Maxstacks?.ScaleAdded);
            Assert.Equal(1, modOne.StackStrategy.StacksPerApplication?.ScaleAdded);

            Assert.Equal("test_description", talent.Presentation.Description);
            Assert.Equal("./test_talent.ico", talent.Presentation.Icon);
            Assert.Equal("Test Talent", talent.Presentation.Name);

            Assert.NotNull(modTwo);

            Assert.NotNull(modTwo.Duration);
            Assert.NotNull(modTwo.StackStrategy);
            Assert.NotNull(modTwo.Modifiers);

            Assert.Equal(1, modTwo.Duration.Turns?.ScaleAdded);

            Assert.Equal(CollectionOperationKind.Remove, modTwo.Modifiers.Operation.Operation);
            Assert.Single(modTwo.Modifiers.Operation.Items);

            Assert.Equal(1, modTwo.StackStrategy.Maxstacks?.ScaleAdded);
            Assert.Equal(1, modTwo.StackStrategy.StacksPerApplication?.ScaleAdded);

            Assert.NotNull(modThree);
            Assert.NotNull(modThree.Cooldown);
            Assert.NotNull(modThree.Cost);

            Assert.Equal(1, modThree.Cooldown.ScaleAdded);
            Assert.Equal(1, modThree.Cost.ScaleAdded);

        }
    }
}