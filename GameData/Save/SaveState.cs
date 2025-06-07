using static GameData.PlayerOld;

public struct SaveState
{
    public string GameName { get; set; }
    public string Id { get; set; }

    public PlayerDto Player { get; set; }
}