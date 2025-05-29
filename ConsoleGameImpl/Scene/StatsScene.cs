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

        GameTextPrinter.DefaultInstance.Print([$"\n{player.Name}'s Stats",
            $"\nClass: {player.Class?.Name ?? "None"}",
            $"\nLevel: {player.LevelManager.CurrentLevel}",
            $"\nExperience: {player.LevelManager.CurrentExperience} / {player.LevelManager.GetExperienceNeededForNextLevel()}",
            $"\nHealth: {player.CurrentHealth}/{player.MaxHealth}",
            $"\nAttack Power: {player.Stats.AttackPower}",
            $"\nDefense: {player.Stats.Defense}",
            $"\nSpeed: {player.Stats.Speed}",
        ]);
        GameTextPrinter.DefaultInstance.WaitForInput();
    }
}