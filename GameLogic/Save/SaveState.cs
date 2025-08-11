using GameData.src.Player;

public struct SaveState
{
    public string GameName { get; set; }
    public string Id { get; set; }

    public PlayerDefinition Player { get; set; }
}