namespace GameData.src.Effect.Stack
{
    public record StackFromEffect : IStackStrategy
    {
        public required string FromEffect { get; init; }

        public required bool ConsumeStacks { get; init; }
    }
}