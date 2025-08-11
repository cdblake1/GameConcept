#nullable enable

using ConsoleGameImpl.State;
using GameData.src.Player;
using GameLogic.Player;
using GameLogic.Ports;
using Infrastructure.Json.Repositories.Initialize;

namespace ConsoleGameImpl.Scene
{
    public class MainGameSceneFactory
    {
        public static MainGameScene Create()
        {
            return new MainGameScene(Repositories.ClassRepository);
        }
    }

    public class MainGameScene
    {
        private readonly IClassRepository classRepository;

        public MainGameScene(IClassRepository classRepository)
        {
            this.classRepository = classRepository;
        }

        public void InitializeNewGame()
        {
            string? name;
            DialogueQueue.AddDialogue(
            [
            "Welcome to the game!",
            "You are a brave adventurer.",
            "Your journey begins now.",
            "But first, what is your name?"
        ]);

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

            DialogueQueue.AddDialogue(
            [
            "A name shouted to the heavens!",
            "Oh the battles you will brave,",
            "The monsters you will slay,",
            "The treasures you will find!",
            "But first, you must choose your class."
        ]);

            var classes = this.classRepository.GetAll();
            var classMenu = new Menu("Choose your class", [.. classes.Select(c => new MenuOption(c.Id))]);

            var selectedClass = classMenu.ShowMenu() switch
            {
                0 => classes[0],
                _ => throw new InvalidOperationException("Invalid class selection.")
            };

            GlobalGameState.Instance.Player = new PlayerInstance(
                name,
                new PlayerDefinition(
                    selectedClass,
                    new GameData.src.Shared.PresentationDefinition
                    {
                        Name = name,
                        Description = $"A brave {selectedClass.Id}",
                        Icon = null
                    }
                )
            );

            DialogueQueue.AddDialogue([
            $"You have chosen the {selectedClass.Id} class.",
            "Your adventure begins now!"
            ]);

            return;
        }

        public static void ShowScene()
        {
            if (GlobalGameState.Instance.Player is not PlayerInstance player)
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
                        // EncounterScene.Create().ShowScene(player,
                        //     new EncounterConfig(player.Level, 3, false), 5); // TODO(Caleb): Add encounter config.
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
                        GameTextPrinter.DefaultInstance.NotImplementedText("Shop");
                        break;
                    case 5:
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
}