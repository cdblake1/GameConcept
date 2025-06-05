public record ActivationRequirement
{
    public required Kind ActivationKind { get; init; }

    public required int Count { get; init; }

    public string? FromEffect { get; init; }

    public enum Kind
    { EffectPresent, HpBelowPercentage }
}