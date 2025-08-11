using GameData.src.Effect.Stack;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using Infrastructure.Json.Repositories;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class EffectRepositoryTests
    {
        private const string testEffectOneId = "test_effect_one";
        private const string TestEffectTwoId = "test_effect_two";
        private readonly JsonEffectRepository repository;
        public static readonly string EffectDtoDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets");

        public EffectRepositoryTests()
        {
            this.repository = new JsonEffectRepository(EffectDtoDirectoryPath);
        }

        [Fact]
        public void CanLoadIntoRepository()
        {
            var effects = this.repository.GetAll();
            Assert.Equal(3, effects.Count);

            Assert.Single(effects, item => item.Id == testEffectOneId);
            Assert.Single(effects, item => item.Id == TestEffectTwoId);

            var effectOne = this.repository.Get(testEffectOneId);
            var effectTwo = this.repository.Get(TestEffectTwoId);

            Assert.NotNull(effectOne);
            Assert.NotNull(effectTwo);

            Assert.Equal(testEffectOneId, effectOne.Id);
            Assert.Equal(1, effectOne.Duration.Turns);

            Assert.Equal(2, effectOne.Modifiers.Count);
            var modOne = effectOne.Modifiers[0] as DamageModifier;
            var modTwo = effectOne.Modifiers[1] as WeaponModifier;

            Assert.NotNull(modOne);
            Assert.NotNull(modTwo);

            Assert.Equal(DamageType.Physical, modOne.DamageType);
            Assert.Equal(WeaponType.Melee, modTwo.WeaponType);

            Assert.Equal(ScalarOpType.Empowered, modOne.Operation);
            Assert.Equal(1, modOne.Value);

            Assert.Equal(ScalarOpType.Additive, modTwo.Operation);
            Assert.Equal(1, modTwo.Value);

            var stack = effectOne.StackStrategy as StackDefault;

            Assert.NotNull(stack);

            Assert.Equal(1, stack.StacksPerApplication);
            Assert.Equal(1, stack.MaxStacks);
            Assert.Equal(StackRefreshMode.NoRefresh, stack.RefreshMode);

            Assert.Equal(TestEffectTwoId, effectTwo.Id);
            Assert.Equal(DurationKind.Permanent, effectTwo.Duration.Kind);

            Assert.Single(effectTwo.Modifiers);
            var statModThree = effectTwo.Modifiers[0] as DamageModifier;

            Assert.NotNull(statModThree);
            Assert.Equal(DamageType.Physical, statModThree.DamageType);
            Assert.Equal(1, statModThree.Value);
            Assert.Equal(ScalarOpType.Empowered, statModThree.Operation);
        }
    }
}