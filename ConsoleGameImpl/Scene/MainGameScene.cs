#nullable enable

using ConsoleGameImpl;
using ConsoleGameImpl.State;
using GameData;
using GameData.src.Player;

public class MainGameScene
{
    public static void InitializeNewGame()
    {
        string? name;
        DialogueQueue.AddDialogue(new[]
        {
            "Welcome to the game!",
            "You are a brave adventurer.",
            "Your journey begins now.",
            "But first, what is your name?"
        });

        while (true)
        {
            GameTextPrinter.DefaultInstance.Print("Enter your name: ");


            name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                GameTextPrinter.DefaultInstance.Print("Name cannot be empty. Please try again.");
                continue;
            }

            break;
        }

        DialogueQueue.AddDialogue(new[]
        {
            "A name shouted to the heavens!",
            "Oh the battles you will brave,",
            "The monsters you will slay,",
            "The treasures you will find!",
            "But first, you must choose your class."
        });

        var classMenu = new Menu("Choose your class", [
            new MenuOption(BloodReaver.Identifier)
        ]);

        var selectedClass = classMenu.ShowMenu() switch
        {
            0 => new BloodReaver(),
            _ => throw new InvalidOperationException("Invalid class selection.")
        };

        GlobalGameState.Instance.Player = new Player(name)
        {
            Class = selectedClass
        };

        DialogueQueue.AddDialogue([
            $"You have chosen the {selectedClass.Name} class.",
            "Your adventure begins now!"
        ]);

        return;
    }

    public static void ShowScene()
    {
        if (GlobalGameState.Instance.Player is not PlayerDefinition player)
        {
            throw new InvalidOperationException("Player is not initialized.");
        }
        while (true)
        {
            var menu = new Menu("Game Menu", [
            new MenuOption("Check Encounters"),
            new MenuOption("Check Inventory", ConsoleKey.I),
            new MenuOption("Check Equipment", ConsoleKey.E),
            new MenuOption("Check Stats", ConsoleKey.A),
            new MenuOption("Heal at the Inn", ConsoleKey.H),
            new MenuOption("Go to the Shop", ConsoleKey.S),
            new MenuOption("Use Crafting Station", ConsoleKey.C),
            new MenuOption("Exit Game", ConsoleKey.Escape)
        ]);

            var input = menu.ShowMenu();
            switch (input)
            {
                case 0:
                    EncounterScene.Create().ShowScene(player,
                        new EncounterConfig(player.Level, 3, false), 5);
                    break;
                case 1:
                    InventoryScene.Create().ShowScene();
                    break;
                case 2:
                    EquipmentScene.Create().ShowScene();
                    break;
                case 3:
                    StatsScene.Create().ShowScene();
                    break;
                case 4:
                    GameTextPrinter.DefaultInstance.Print("You rest at the inn.");
                    GameTextPrinter.DefaultInstance.WaitForInput();
                    GameTextPrinter.DefaultInstance.Print([new($"you healed for "), new($"{player.MaxHealth - player.CurrentHealth}", GameTextPrinter.GetColor(TextKind.Health)), new(" health.")]);
                    GameTextPrinter.DefaultInstance.WaitForInput();
                    player.CurrentHealth = player.MaxHealth;
                    break;
                case 5:
                    GameTextPrinter.DefaultInstance.NotImplementedText("Shop");
                    break;
                case 6:
                    GameTextPrinter.DefaultInstance.NotImplementedText("Crafting Station");
                    break;
                case 7:
                case -1:
                    ExitGameScene.Show();
                    return;
                default:
                    break;
            }
        }
    }
}