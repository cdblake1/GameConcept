namespace GameData.src.Shared.Modifiers.Operations
{
    public sealed record ScalarOperation
    {
        public float ScaleIncreased { get; init; } = 0;
        public float ScaleAdded { get; init; } = 0;

        public float ScaleEmpowered { get; init; } = 0;

        public float? OverrideValue { get; init; }
    }
}