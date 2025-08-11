namespace GameData.src.Shared
{
    public record Duration
    {
        private Duration(DurationKind kind, int turns = 0, ExpiresWith expiry = default)
            => (Kind, Turns, Expiry) = (kind, turns, expiry);

        public static Duration FromTurns(int t) => new(DurationKind.Turns, t);
        public static Duration Permanent() => new(DurationKind.Permanent);
        public static Duration FromExpiry(ExpiresWith e) => new(DurationKind.ExpiresWith, expiry: e);

        public DurationKind Kind { get; }
        public int Turns { get; }
        public ExpiresWith Expiry { get; }

        public readonly record struct ExpiresWith(ExpiresWith.Category Source, string Id)
        {
            public enum Category { Skill, Effect }
        }
    }

    public enum DurationKind { Turns, Permanent, ExpiresWith }
}