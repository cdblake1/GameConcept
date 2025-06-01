using GameData.Save;

public class SaveGameScene
{
    private readonly SaveManager saveManager;
    private readonly GameTextPrinter gameTextPrinter;

    private SaveGameScene(SaveManager saveManager, GameTextPrinter gameTextPrinter)
    {
        this.saveManager = saveManager;
        this.gameTextPrinter = gameTextPrinter;
    }

    public void ShowScene()
    {
        while (true)
        {
            var player = GlobalGameState.Instance.Player;
            if (player == null)
            {
                gameTextPrinter.Print("Player data is unavailable. Saving progress is not possible at this time.");
                return;
            }

            gameTextPrinter.Print("Please provide a name to save your progress (or press 'Escape' to exit):");
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                gameTextPrinter.Print("\nYou have chosen to exit without saving.");
                return;
            }

            Console.Write("\b"); // Clear the key press from the console
            string? saveName = Console.ReadLine();

            if (string.IsNullOrEmpty(saveName))
            {
                gameTextPrinter.Print("The name provided is invalid. Please try again.");
                continue;
            }

            string saveId = saveManager.SaveGame(saveName, player);
            gameTextPrinter.Print($"Your progress has been successfully saved under the name '{saveName}'.");

            gameTextPrinter.WaitForInput();
            return;
        }
    }


    public static SaveGameScene Create()
    {
        var saveManager = new SaveManager();
        var gameTextPrinter = new GameTextPrinter();
        return new SaveGameScene(saveManager, gameTextPrinter);
    }
}