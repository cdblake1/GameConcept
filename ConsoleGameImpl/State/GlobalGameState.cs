#nullable enable

using GameLogic.Player;

namespace ConsoleGameImpl.State
{
    public class GlobalGameState()
    {
        public static GlobalGameState Instance => instance;
        private static readonly GlobalGameState instance = new GlobalGameState();

        public PlayerInstance? Player { get; set; }
    }
}