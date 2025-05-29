using GameData;

public struct SaveState
{
    public string GameName { get; set; }
    public string Id { get; set; }

    public Player.PlayerDto Player { get; set; }
}