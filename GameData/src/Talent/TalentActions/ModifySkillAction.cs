using GameData.src.Shared.Modifier;

namespace GameData.src.Effect.Talent
{
    public class ModifySkillAction : ITalentAction
    {
        public required string Skill { get; init; }

        public ScalarOperation? Cost { get; init; }

        public ScalarOperation? Cooldown { get; init; }

        public ActivationRequirement? ActivationRequirement { get; init; }
    }
}