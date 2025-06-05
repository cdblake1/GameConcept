using GameData.src.Shared;

namespace GameData.src.Effect.Status
{
    public record StunStatus : IStatus
    {
        public required Duration Duration { get; init; }

        public static StunStatus Create(Duration duration) => new()
        {
            Duration = duration
        };
    }
}