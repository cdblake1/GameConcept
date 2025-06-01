using GameData;
using GameData.Save;

public class ExitGameScene
{
    public static void Show()
    {
        var textPrinter = new GameTextPrinter();
        var menu = new Menu("would you like to save before you exit?", [
            new MenuOption("Save and Exit", ConsoleKey.S),
            new MenuOption("Exit without saving", ConsoleKey.E),
            new MenuOption("Cancel", ConsoleKey.C)
        ]);

        while (true)
        {
            var input = menu.ShowMenu();
            if (input == -1)
            {
                return;
            }

            switch (input)
            {
                case 0:
                    var saveManager = new SaveManager();

                    SaveGameScene.Create().ShowScene();
                    Environment.Exit(0);

                    break;
                case 1:
                    Environment.Exit(0);

                    break;
                case 2:
                    // Cancel exit
                    return;
                default:
                    throw new InvalidOperationException("Invalid menu selection");
            }
        }
    }
}