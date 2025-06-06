using GameData;
using GameData.src.Player;

public struct SaveState
{
    public string GameName { get; set; }
    public string Id { get; set; }

    public Player.PlayerDto Player { get; set; }
}