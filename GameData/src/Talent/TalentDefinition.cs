using GameData.src.Shared;

namespace GameData.src.Effect.Talent
{
    public record TalentDefinition
    {
        public required string Id { get; init; }

        public required IReadOnlyList<ITalentAction> Actions { get; init; }

        public required PresentationDefinition Presentation { get; init; }
    }
}