using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Skill.SkillStep;
using Infrastructure.Json.Repositories;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class SkillRepositoryTests
    {
        private const string testSkillOneId = "test_skill_one";
        private const string testSkillTwoId = "test_skill_two";
        private readonly JsonSkillRepository repository;
        public static readonly string SkillDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Skills");

        public SkillRepositoryTests()
        {
            this.repository = new JsonSkillRepository(SkillDirectoryPath);
        }

        [Fact]
        public void CanLoadIntoRepository()
        {
            var skills = this.repository.GetAll();

            Assert.Contains(skills, item => item.Id == testSkillOneId);
            Assert.Contains(skills, item => item.Id == testSkillTwoId);

            var skillOne = this.repository.Get(testSkillOneId);
            var skillTwo = this.repository.Get(testSkillTwoId);

            Assert.Equal(1, skillOne.Cooldown);
            Assert.Equal(1, skillOne.Cost);
            Assert.Equal(testSkillOneId, skillOne.Id);
            Assert.Equal(2, skillOne.Effects.Count);

            var modOne = skillOne.Effects[0] as HitDamageStep;
            var modTwo = skillOne.Effects[1] as ApplyEffectStep;

            Assert.NotNull(modOne);
            Assert.Equal(WeaponType.Range, modOne.WeaponType);
            Assert.Equal(AttackType.Hit, modOne.AttackType);
            Assert.True(modOne.Crit);
            Assert.Equal(1, modOne.MinBaseDamage);
            Assert.Equal(1, modOne.MaxBaseDamage);
            Assert.Equal(DamageType.Nature, modOne.DamageType);
            Assert.Equal(1, modOne.ScaleProperties.ScaleAdded);
            Assert.Equal(1, modOne.ScaleProperties.ScaleIncreased);
            Assert.Equal(0, modOne.ScaleProperties.ScaleSpeed);

            var stack = modOne.StackFromEffect;

            Assert.NotNull(stack);
            Assert.Equal("test_effect", stack.EffectId);
            Assert.False(stack.ConsumeStacks);

            Assert.NotNull(modTwo);
            Assert.Equal("test_effect", modTwo.EffectId);

            Assert.Equal("test skill one description", skillOne.Presentation.Description);
            Assert.Equal("test skill one", skillOne.Presentation.Name);
            Assert.Equal("./test_skill_one.png", skillOne.Presentation.Icon);

            Assert.Equal(testSkillTwoId, skillTwo.Id);
            Assert.Equal(2, skillTwo.Cooldown);
            Assert.Equal(2, skillTwo.Cost);
            Assert.Single(skillTwo.Effects);

            var dot = skillTwo.Effects[0] as DotDamageStep;
            Assert.NotNull(dot);
            Assert.Equal(AttackType.Dot, dot.AttackType);
            Assert.Equal(WeaponType.Spell, dot.WeaponType);
            Assert.Equal(1, dot.MinBaseDamage);
            Assert.Equal(1, dot.MaxBaseDamage);
            Assert.Equal(1, dot.ScaleProperties.ScaleAdded);
            Assert.Equal(1, dot.ScaleProperties.ScaleIncreased);
            Assert.Equal(0, dot.ScaleProperties.ScaleSpeed);
            Assert.False(dot.Crit);
            Assert.Equal(DamageType.Nature, dot.DamageType);
            Assert.Equal(DurationKind.Permanent, dot.Duration.Kind);
            Assert.Equal(1, dot.Frequency);
            var stacking = dot.StackStrategy as StackDefault;
            Assert.NotNull(stacking);
            Assert.Equal(1, stacking.MaxStacks);
            Assert.Equal(1, stacking.StacksPerApplication);
            Assert.Equal(StackRefreshMode.AddTime, stacking.RefreshMode);

            Assert.NotNull(skillTwo.ActivationRequirement);
            Assert.Equal(1, skillTwo.ActivationRequirement.Count);
            Assert.Equal("test_effect", skillTwo.ActivationRequirement.EffectId);

            Assert.Equal("test skill two description", skillTwo.Presentation.Description);
            Assert.Equal("test skill two", skillTwo.Presentation.Name);
            Assert.Equal("./test_skill_two.png", skillTwo.Presentation.Icon);
        }
    }
}