
using GameData;
using GameData.src.Item;
using GameData.src.Player;

namespace ConsoleGameImpl;

public class EncounterScene
{
    private readonly GameTextPrinter textPrinter;
    private readonly EncounterSelector encounterSelector;

    public void ShowScene(PlayerDefinition player, EncounterConfig config, int maxEncounters)
    {
        var encounters = encounterSelector.SelectEncounters(config, maxEncounters);
        var menu = new Menu("Encounters", encounters.Select(e => new MenuOption(e.Name)).ToList());
        while (true)
        {
            var selectedOption = menu.ShowMenu();
            if (selectedOption == -1)
            {
                return; // Exit
            }

            if (selectedOption >= 0 && selectedOption < encounters.Count)
            {
                var selectedEncounter = encounters[selectedOption];
                StartEncounter(player, selectedEncounter);
                return;
            }
            else
            {
                textPrinter.Print("Invalid selection.");
                textPrinter.WaitForInput();
                continue;
            }
        }
    }

    private void StartEncounter(PlayerDefinition player, Encounter encounter)
    {
        DialogueQueue.AddDialogue([
            $"You entered the {encounter.Name}",
            ..encounter.Description.Split('\n'),
        ]);

        while (encounter.EncounterIsActive)
        {
            DialogueQueue.AddDialogue([
                $"You have {encounter.Config.Duration - encounter.CurrentDuration} turns left.",
                $"You have {player.CurrentHealth}/{player.MaxHealth} health remaining."
            ]);

            var input = new Menu("Encounter Actions", [
                new MenuOption("Continue Encounter"),
                new MenuOption("End Encounter")
            ]).ShowMenu();

            if (input == -1)
            {
                return; // Exit
            }

            if (input == 0)
            {
                var mob = encounter.AdvanceEncounter();
                var outcome = new CombatScene(player, mob).StartScene();

                switch (outcome)
                {
                    case CombatOutcome.Victory:
                        break;
                    case CombatOutcome.Defeat:
                        encounter.EndEncounter();
                        textPrinter.Print("You lost the encounter.");
                        break;
                    case CombatOutcome.Flee:
                        encounter.EndEncounter();
                        textPrinter.Print("You fled from the encounter.");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (input == 1)
            {
                encounter.EndEncounter();
                textPrinter.Print("You ended the encounter early.");
                textPrinter.WaitForInput();
                break;
            }
        }

        if (!encounter.EncounterEndedEarly)
        {
            textPrinter.Print($"You have completed the {encounter.Name} encounter!");
            textPrinter.WaitForInput();

            var reward = encounter.EncounterReward();
            if (reward.GoldCoin is not null)
            {
                player.Gold += reward.GoldCoin;
                textPrinter.Print([
                    new("You received"),
                    new($" {reward.GoldCoin.Amount}", GameTextPrinter.GetColor(TextKind.Gold)),
                    new(" Gold Coins.")
                ]);
                textPrinter.WaitForInput();

                foreach (var item in reward.Loot)
                {
                    if (item is CraftingMaterial material)
                    {
                        player.Inventory.AddItem(material);
                        textPrinter.Print([
                            new("You received"),
                            new($" {material.Count}"),
                            new($" {material.Name}", GameTextPrinter.GetColor(TextKind.LootNormal)),
                        ]);
                        textPrinter.WaitForInput();
                    }
                    else if (item is Equipment equipment)
                    {
                        player.Inventory.AddItem(equipment);
                        textPrinter.Print([
                            new("You received"),
                            new($" {equipment.Name}", GameTextPrinter.GetColor(TextKind.LootRare)),
                            new("!")
                        ]);
                        textPrinter.WaitForInput();
                    }
                    else
                    {
                        player.Inventory.AddItem(item);
                        textPrinter.Print([
                            new("You received"),
                            new($" {item.Name}", GameTextPrinter.GetColor(TextKind.LootNormal)),
                            new("!")
                        ]);
                        textPrinter.WaitForInput();
                        return;
                    }
                }
            }
        }
    }


    private EncounterScene()
    {
        encounterSelector = EncounterSelector.Create();
        textPrinter = new GameTextPrinter();
    }

    public static EncounterScene Create()
    {
        return new EncounterScene();
    }
}