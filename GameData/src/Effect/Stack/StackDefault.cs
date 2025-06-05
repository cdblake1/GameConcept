namespace GameData.src.Effect.Stack
{
    public record StackDefault : IStackStrategy
    {
        public required int StacksPerApplication { get; init; }

        public required int MaxStacks { get; init; }

        public required StackRefreshMode RefreshMode { get; init; }
    }
}