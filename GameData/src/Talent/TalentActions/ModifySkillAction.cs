using GameData.src.Shared.Modifiers.Operations;
using GameData.src.Skill;

namespace GameData.src.Talent.TalentActions
{
    public class ModifySkillAction : ITalentAction
    {
        public required string SkillId { get; init; }
        public ScalarOperation? Cost { get; init; }
        public ScalarOperation? Cooldown { get; init; }
        public ActivationRequirement? ActivationRequirement { get; init; }
    }
}