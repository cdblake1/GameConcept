#nullable enable

using GameData.src.Player;

namespace ConsoleGameImpl.State
{
    public class GlobalGameState()
    {
        public static GlobalGameState Instance => instance;
        private static readonly GlobalGameState instance = new GlobalGameState();

        public PlayerDefinition? Player { get; set; }
    }
}