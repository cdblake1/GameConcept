namespace GameData.src.Shared.Modifiers.Operations
{
    public sealed record DurationOperation
    {
        public Duration.Kind Kind { get; private init; }

        public ScalarOperation? Turns { get; private init; }

        public bool? Permanent { get; private init; } = default;

        public Duration.ExpiresWith? ExpiresWith { get; private init; }

        public static DurationOperation FromTurns(ScalarOperation turns)
        {
            return new()
            {
                Kind = Duration.Kind.Turns,
                Turns = turns
            };
        }

        public static DurationOperation FromPermanent(bool permanent)
        {
            return new()
            {
                Kind = Duration.Kind.Permanent,
                Permanent = permanent
            };
        }

        public static DurationOperation FromExpiry(Duration.ExpiresWith expiresWith)
        {
            return new()
            {
                Kind = Duration.Kind.ExpiresWith,
                ExpiresWith = expiresWith
            };
        }
    }
}