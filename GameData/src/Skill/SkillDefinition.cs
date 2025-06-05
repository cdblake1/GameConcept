using GameData.src.Shared;
using GameData.src.Skill.SkillStep;

namespace GameData.src.Skill
{
    public record SkillDefinition
    {
        public required string Id { get; init; }
        public required int Cost { get; init; }
        public required int Cooldown { get; init; }
        public required IReadOnlyList<ISkillStep> Effects { get; init; }
        public required PresentationDefinition Presentation { get; init; }
        public ActivationRequirement? ActivationRequirement { get; init; }
    }
}