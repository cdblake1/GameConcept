using ConsoleGameImpl.State;
using GameData.src.Class;
using GameData.src.Player;
using GameLogic.Player;
using GameData.src.Shared.Enums;

public class StatsScene
{
    public static StatsScene Create()
    {
        return new StatsScene();
    }

    private StatsScene()
    {
    }

    public void ShowScene()
    {
        if (GlobalGameState.Instance.Player is not PlayerInstance player)
        {
            throw new InvalidOperationException("Player is not initialized.");
        }

        GameTextPrinter.DefaultInstance.PrintLine([new($"\nClass: {player.PlayerDefinition.ClassDefinition.Id}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nLevel: {player.LevelManager.CurrentLevel}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nExperience: {player.LevelManager.CurrentExperience} / {player.LevelManager.GetExperienceNeededForNextLevel()}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nHealth: {player.Stats.GetStatValue(GlobalStat.Health)}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nAttack Power: {player.Stats.GetStatValue(AttackType.Hit)}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nDefense: {player.Stats.GetStatValue(GlobalStat.Armor)}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nSpeed: {player.Stats.GetStatValue(GlobalStat.Speed)}")]);
        GameTextPrinter.DefaultInstance.WaitForInput();
    }
}