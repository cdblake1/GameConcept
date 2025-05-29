#nullable enable

using GameData;

public class GlobalGameState()
{
    public static GlobalGameState Instance => instance;
    private static readonly GlobalGameState instance = new GlobalGameState();

    public Player? Player { get; set; }
}