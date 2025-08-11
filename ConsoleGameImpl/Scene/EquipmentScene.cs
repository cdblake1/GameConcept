using ConsoleGameImpl.State;
using GameData.src.Player;
using GameLogic.Player;
using GameLogic.Inventory;
using GameData.src.Item.Equipment;

public class EquipmentScene
{
    private EquipmentScene()
    {

    }

    public static EquipmentScene Create() => new();

    public void ShowScene()
    {
        if (GlobalGameState.Instance.Player is not PlayerInstance player)
        {
            throw new InvalidOperationException("Player is not initialized.");
        }

        var slots = new List<MenuOption>();
        var equippedItems = player.Inventory.GetEquippedItems();

        foreach (var (slot, item) in equippedItems)
        {
            if (item != null)
            {
                slots.Add(new([new($"{slot}: "), GameTextPrinter.GetItemText(item)]));
            }
            else
            {
                slots.Add(new($"{slot}: Empty"));
            }
        }

        var menu = new Menu("Equipment", slots);

        var input = menu.ShowMenu();
        if (input == -1)
        {
            return; // Exit
        }

        if (input >= 0 && input < slots.Count)
        {
            var selectedSlot = equippedItems.Keys.ElementAt(input);
            var selectedItem = equippedItems[selectedSlot];

            if (selectedItem != null)
            {
                ShowEquipmentMenu(player, selectedItem);
            }
            else
            {
                GameTextPrinter.DefaultInstance.Print("No item equipped in this slot.");
                GameTextPrinter.DefaultInstance.WaitForInput();
            }
        }
        else
        {
            GameTextPrinter.DefaultInstance.Print("Invalid selection.");
        }
    }

    public static void ShowEquipmentMenu(PlayerInstance player, EquipmentDefinition item)
    {
        while (true)
        {
            GameTextPrinter.DefaultInstance.Print(GameTextPrinter.GetItemText(item));

            var input = new Menu(null, [
                new MenuOption("Equip"),
                new MenuOption("Inspect"),
                new MenuOption("Back")
            ])
            {
                ClearConsole = false
            }.ShowMenu();

            if (input == -1)
            {
                break; // Exit
            }
            else if (input == 0)
            {
                player.Inventory.EquipItem(item);
                break;
            }
            else if (input == 1)
            {
                GameTextPrinter.DefaultInstance.PrintLine([GameTextPrinter.GetItemText(item)], false, 0);
                GameTextPrinter.DefaultInstance.PrintLine([new($"Rarity: {item.Rarity}")], false, 0);
                GameTextPrinter.DefaultInstance.PrintLine([new($"Type: {item.Kind}")], false, 0);
                GameTextPrinter.DefaultInstance.PrintLine([.. item.Presentation.Description.Split("\n").Select(s => new TextPacket(s))], false, 0);
                GameTextPrinter.DefaultInstance.WaitForInput();
                break;
            }
        }
    }



    private string GetEquipmentKindText(EquipmentKind kind) =>
        kind switch
        {
            EquipmentKind.Weapon => "Weapon",
            EquipmentKind.Body => "Body",
            EquipmentKind.Legs => "Legs",
            EquipmentKind.Helmet => "Head",
            EquipmentKind.Gloves => "Gloves",
            EquipmentKind.Boots => "Boots",
            EquipmentKind.Necklace => "Necklace",
            EquipmentKind.Ring => "Ring",
            _ => throw new InvalidOperationException("Unknown equipment kind.")
        };
}