namespace GameData.src.Shared
{
    public sealed record PresentationDefinition
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string? Icon { get; init; }
    }
}