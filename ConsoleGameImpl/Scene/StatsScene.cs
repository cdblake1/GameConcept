using GameData;

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
        if (GlobalGameState.Instance.Player is not Player player)
        {
            throw new InvalidOperationException("Player is not initialized.");
        }

        GameTextPrinter.DefaultInstance.PrintLine([new($"\n{player.Name}'s Stats")], false, 0);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nClass: {player.Class?.Name ?? "None"}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nLevel: {player.LevelManager.CurrentLevel}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nExperience: {player.LevelManager.CurrentExperience} / {player.LevelManager.GetExperienceNeededForNextLevel()}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nHealth: {player.CurrentHealth}/{player.MaxHealth}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nAttack Power: {player.Stats.AttackPower}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nDefense: {player.Stats.Defense}")]);
        GameTextPrinter.DefaultInstance.PrintLine([new($"\nSpeed: {player.Stats.Speed}")]);

        GameTextPrinter.DefaultInstance.WaitForInput();
    }
}