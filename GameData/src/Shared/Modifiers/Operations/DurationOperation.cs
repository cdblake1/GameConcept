namespace GameData.src.Shared.Modifiers.Operations
{
    public sealed record DurationOperation
    {
        public DurationKind Kind { get; private init; }

        public ScalarOperation? Turns { get; private init; }

        public bool? Permanent { get; private init; } = default;

        public Duration.ExpiresWith? ExpiresWith { get; private init; }

        public static DurationOperation FromTurns(ScalarOperation turns)
        {
            return new()
            {
                Kind = DurationKind.Turns,
                Turns = turns
            };
        }

        public static DurationOperation FromPermanent(bool permanent)
        {
            return new()
            {
                Kind = DurationKind.Permanent,
                Permanent = permanent
            };
        }

        public static DurationOperation FromExpiry(Duration.ExpiresWith expiresWith)
        {
            return new()
            {
                Kind = DurationKind.ExpiresWith,
                ExpiresWith = expiresWith
            };
        }
    }
}