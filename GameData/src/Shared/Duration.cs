using GameData.src.Shared.Modifiers.Operations;

namespace GameData.src.Shared
{
    public readonly record struct Duration
    {
        private Duration(Kind kind, int turns = 0, ExpiresWith expiry = default)
            => (Type, Turns, Expiry) = (kind, turns, expiry);

        public static Duration FromTurns(int t) => new(Kind.Turns, t);
        public static Duration Permanent() => new(Kind.Permanent);
        public static Duration FromExpiry(ExpiresWith e) => new(Kind.ExpiresWith, expiry: e);

        public Kind Type { get; }
        public int Turns { get; }
        public ExpiresWith Expiry { get; }

        public enum Kind { Turns, Permanent, ExpiresWith }
        public readonly record struct ExpiresWith(ExpiresWith.Category Source, string Id)
        {
            public enum Category { Skill, Effect }
        }
    }
}