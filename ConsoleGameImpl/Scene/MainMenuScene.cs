#nullable enable
public class MainMenuScene
{

    public static void ShowMainMenu()
    {
        var menu = new Menu("Main Menu", new List<MenuOption>
        {
            new MenuOption("New Game"),
            new MenuOption("Load Game"),
            new MenuOption("Options"),
            new MenuOption("Exit")
        });

        while (true)
        {
            var input = menu.ShowMenu();
            switch (input)
            {
                case 0:
                    MainGameScene.InitializeNewGame();
                    return;
                case 1:
                    if (LoadGameScene.Show())
                    {
                        return;
                    }
                    break;
                case 2:
                    ShowOptions();
                    break;
                case 3:
                    ExitGameScene.Show();
                    break;
                default:
                    ExitGameScene.Show();
                    break;
            }
        }
    }

    public static void ShowInGame()
    {
        var textPrinter = new GameTextPrinter();
        var menu = new Menu("In-Game Menu", new List<MenuOption>
        {
            new MenuOption("Resume Game"),
            new MenuOption("Save Game"),
            new MenuOption("Load Game"),
            new MenuOption("Options"),
        });

        while (true)
        {
            switch (menu.ShowMenu())
            {
                case 0:
                    return; // Resume Game
                case 1:
                    SaveGameScene.Create().ShowScene();
                    break;
                case 2:
                    if (LoadGameScene.Show())
                    {
                        return; // Exit to resume the loaded game
                    }
                    break;
                case 3:

                    ShowOptions();
                    break;
                default:
                    break;
            }
        }
    }

    private static void ShowOptions()
    {
        DialogueQueue.AddDialogue(["Options are not implemented yet."]);
    }
}