using GameData.src.Shared;
using static GameData.src.Shared.Duration;

namespace GameLogic.Combat.Snapshots
{
    public struct DurationSnapshot
    {
        public ExpiresWith? ExpiresWith;
        public DurationKind Kind;
        public int Turns;
        public int RemainingTurns;

        private DurationSnapshot(Duration duration)
        {
            this.ExpiresWith = duration.Expiry;
            this.Kind = duration.Kind;
            this.RemainingTurns = 0;
            this.Turns = duration.Turns;
        }

        public static DurationSnapshot FromDuration(Duration duration)
            => new(duration);
    }
}