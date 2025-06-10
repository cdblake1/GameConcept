#nullable enable

using GameData;
using GameData.src.Player;

public class GlobalGameState()
{
    public static GlobalGameState Instance => instance;
    private static readonly GlobalGameState instance = new GlobalGameState();

    public PlayerDefinition? Player { get; set; }
}