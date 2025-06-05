using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
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

            Assert.Equal(testSkillOneId, skillOne.Id);
            Assert.Equal(1, skillOne.Cooldown);
            Assert.Equal(1, skillOne.Cost);
            Assert.Equal(2, skillOne.Effects.Count);
            Assert.Collection(skillOne.Effects,
                step =>
                {
                    var damage = step as HitDamageStep;
                    Assert.NotNull(damage);
                    Assert.True(damage.Crit);
                    Assert.Equal(1, damage.BaseDamage);
                    Assert.Equal(DamageType.Nature, damage.DamageTypes[0]);
                    Assert.Equal(StatKind.ElementalDamage, damage.ScaleCoefficient.Stat);
                    Assert.Equal(ScalarOperation.OperationKind.Mult, damage.ScaleCoefficient.Operation.ModifierOperation);
                    Assert.Equal(1, damage.ScaleCoefficient.Operation.Value);
                    Assert.NotNull(damage.StackFromEffect);
                    Assert.False(damage.StackFromEffect.ConsumeStacks);
                    Assert.Equal("test_effect", damage.StackFromEffect.FromEffect);
                },
                step =>
                {
                    var applyEffect = step as ApplyEffectStep;
                    Assert.NotNull(applyEffect);
                    Assert.Equal("test_effect", applyEffect.Effect);
                });

            Assert.Equal("test skill one description", skillOne.Presentation.Description);
            Assert.Equal("test skill one", skillOne.Presentation.Name);
            Assert.Equal("./test_skill_one.png", skillOne.Presentation.Icon);

            Assert.Equal(testSkillTwoId, skillTwo.Id);
            Assert.Equal(2, skillTwo.Cooldown);
            Assert.Equal(2, skillTwo.Cost);
            Assert.Single(skillTwo.Effects);

            var dot = skillTwo.Effects[0] as DotDamageStep;
            Assert.NotNull(dot);
            Assert.Equal(2, dot.BaseDamage);
            Assert.False(dot.Crit);
            Assert.Equal(DamageType.Nature, dot.DamageTypes[0]);
            Assert.Equal(Duration.Kind.Permanent, dot.Duration.Type);
            Assert.Equal(1, dot.Frequency);
            var stacking = dot.Stacking as StackDefault;
            Assert.NotNull(stacking);
            Assert.Equal(1, stacking.MaxStacks);
            Assert.Equal(1, stacking.StacksPerApplication);
            Assert.Equal(StackRefreshMode.NoRefresh, stacking.RefreshMode);
            Assert.Equal(StatKind.ElementalDamage, dot.ScaleCoefficient.Stat);
            Assert.Equal(2, dot.ScaleCoefficient.Operation.Value);
            Assert.Equal(ScalarOperation.OperationKind.Set, dot.ScaleCoefficient.Operation.ModifierOperation);

            Assert.NotNull(skillTwo.ActivationRequirement);
            Assert.Equal(1, skillTwo.ActivationRequirement.Count);
            Assert.Equal("test_effect", skillTwo.ActivationRequirement.FromEffect);

            Assert.Equal("test skill two description", skillTwo.Presentation.Description);
            Assert.Equal("test skill two", skillTwo.Presentation.Name);
            Assert.Equal("./test_skill_two.png", skillTwo.Presentation.Icon);
        }
    }
}