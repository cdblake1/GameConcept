
using GameData.src.Effect;
using GameData.src.Effect.Status;
using GameData.src.Shared;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
using Infrastructure.Json.Repositories;

namespace Infrastructure.Json.Tests.RepositoryTests
{
    public class EffectRepositoryTests
    {
        private const string testEffectOneId = "test_effect_one";
        private const string TestEffectTwoId = "test_effect_two";
        private readonly JsonEffectRepository repository;
        public static readonly string EffectDtoDirectoryPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Effects");

        public EffectRepositoryTests()
        {
            this.repository = new JsonEffectRepository(EffectDtoDirectoryPath);
        }

        [Fact]
        public void CanLoadIntoRepository()
        {
            var effects = this.repository.GetAll();

            Assert.Single(effects, item => item.Id == testEffectOneId);
            Assert.Single(effects, item => item.Id == TestEffectTwoId);

            var effectOne = this.repository.Get(testEffectOneId);
            var effectTwo = this.repository.Get(TestEffectTwoId);

            Assert.Equal(testEffectOneId, effectOne.Id);
            Assert.Equal(2, effects.Count);

            Assert.True(effectOne.ApplyStatus[0] is StunStatus);
            Assert.Equal(1, ((StunStatus)effectOne.ApplyStatus[0]).Duration.Turns);

            Assert.Equal(EffectDefinition.Kind.Buff, effectOne.Category);
            Assert.Equal(DamageType.Bleed, effectOne.DamageTypes[0]);
            Assert.Single(effectOne.Modifiers);
            Assert.Collection(effectOne.Modifiers,
                mod =>
                {
                    Assert.Equal(ScalarOperation.OperationKind.Add, mod.Operation.ModifierOperation);
                    Assert.Equal(1, mod.Operation.Value);
                });

            Assert.Equal(1, effectOne.Leech);

            Assert.Equal(TestEffectTwoId, effectTwo.Id);
            Assert.Equal(EffectDefinition.Kind.Buff, effectTwo.Category);
            Assert.Equal(DamageType.Physical, effectTwo.DamageTypes[0]);
            Assert.Equal(Duration.Kind.Permanent, effectTwo.Duration.Type);
            Assert.Collection(effectTwo.Modifiers,
                mod =>
                {
                    Assert.Equal(ScalarOperation.OperationKind.Mult, mod.Operation.ModifierOperation);
                    Assert.Equal(2, mod.Operation.Value);
                });
        }
    }
}