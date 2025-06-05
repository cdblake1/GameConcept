using GameData.src.Effect.Stack;
using GameData.src.Effect.Status;
using GameData.src.Effect.Talent;
using GameData.src.Shared.Enums;
using GameData.src.Shared.Modifier;
using GameData.src.Shared.Modifiers;
using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill.SkillStep;
using GameData.src.Talent.TalentActions;
using Infrastructure.Json.Dto.Common.Operations;
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

            Assert.Collection(talent.Actions,
                action =>
                {
                    var modDamage = action as ModifyDotDamageAction;
                    Assert.NotNull(modDamage);
                    Assert.Equal(ScalarOperation.OperationKind.Add, modDamage.BaseDamage?.ModifierOperation);
                    Assert.Equal(1, modDamage.BaseDamage?.Value);
                    Assert.True(modDamage.Crit);
                    Assert.Equal(DamageType.Burn, modDamage.DamageTypes?.Operation.Items[0]);
                    Assert.Equal(CollectionOperationKind.Add, modDamage.DamageTypes?.Operation.Operation);

                    Assert.Equal(1, modDamage.Duration?.Turns?.Value);
                    Assert.Equal(ScalarOperation.OperationKind.Add, modDamage.Duration?.Turns?.ModifierOperation);

                    Assert.Equal(1, modDamage.Frequency?.Value);
                    Assert.Equal(ScalarOperation.OperationKind.Add, modDamage.Frequency?.ModifierOperation);

                    var stacking = modDamage.StackStrategy as StackFromEffect;
                    Assert.NotNull(stacking);

                    Assert.False(stacking.ConsumeStacks);
                    Assert.Equal("test_effect", stacking.FromEffect);

                    Assert.Equal(DotDamageStep.TimingKind.StartTurn, modDamage.Timing);
                    Assert.Equal("test_skill", modDamage.SkillId);
                },
                Action =>
                {
                    var modEffect = Action as ModifyEffectAction;
                    Assert.NotNull(modEffect);

                    var stacking = modEffect.Stacking as StackDefault;
                    Assert.NotNull(stacking);

                    Assert.Equal(1, stacking.MaxStacks);
                    Assert.Equal(1, stacking.StacksPerApplication);
                    Assert.Equal(StackRefreshMode.AddTime, stacking.RefreshMode);

                    Assert.Equal(ScalarOperation.OperationKind.Add, modEffect.Leech?.ModifierOperation);
                    Assert.Equal(1, modEffect.Leech?.Value);

                    Assert.Equal(1, modEffect.Duration?.Turns?.Value);
                    Assert.Equal(ScalarOperation.OperationKind.Add, modEffect?.Duration?.Turns?.ModifierOperation);

                    Assert.Equal("test_effect", modEffect?.Id);
                    Assert.Equal(CollectionOperationKind.Add, modEffect?.Modifiers?.Operation.Operation);
                    Assert.Equal(1, modEffect?.Modifiers?.Operation.Items[0].Operation.Value);
                    Assert.Equal(ScalarOperation.OperationKind.Add, modEffect?.Modifiers?.Operation.Items[0].Operation.ModifierOperation);


                    Assert.Equal(DamageType.Bleed, modEffect?.DamageTypes?.Operation.Items[0]);
                    Assert.Equal(CollectionOperationKind.Add, modEffect?.DamageTypes?.Operation.Operation);
                });

            Assert.Equal("test_description", talent.Presentation.Description);
            Assert.Equal("./test_talent.ico", talent.Presentation.Icon);
            Assert.Equal("Test Talent", talent.Presentation.Name);

        }
    }
}