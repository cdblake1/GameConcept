namespace GameData.src.Skill
{
    public record ActivationRequirement
    {
        public required ActivationRequirementType ActivationKind { get; init; }

        public required int Count { get; init; }

        public string? EffectId { get; init; }
    }

    public enum ActivationRequirementType
    { EffectPresent, HpBelowPercentage }
}