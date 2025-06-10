using GameData;
using GameData.src.Player;

public class EquipmentScene
{
    private EquipmentScene()
    {

    }

    public static EquipmentScene Create() => new();

    public void ShowScene()
    {
        if (GlobalGameState.Instance.Player is not PlayerDefinition player)
        {
            throw new InvalidOperationException("Player is not initialized.");
        }

        var slots = new List<MenuOption>();
        var equipment = player.Equipment.GetAllEquipment().ToList();
        foreach (var (slot, item) in player.Equipment.GetAllEquipment())
        {
            if (item is Equipment equippedItem)
            {
                slots.Add(new([new($"{slot}: "), GameTextPrinter.GetItemText(equippedItem)]));
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
            var selectedItem = equipment[input].Item;

            if (selectedItem is Equipment item)
            {
                ShowEquipmentMenu(player, item);
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

    public static void ShowEquipmentMenu(PlayerDefinition player, Equipment item)
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
                player.EquipItem(item);
                break;
            }
            else if (input == 1)
            {
                GameTextPrinter.DefaultInstance.PrintLine([GameTextPrinter.GetItemText(item)], false, 0);
                GameTextPrinter.DefaultInstance.PrintLine([new($"Stats:\nAttack Power: {item.Stats.AttackPower}\nDefense: {item.Stats.Defense}\nHealth: {item.Stats.Health}\nSpeed: {item.Stats.Speed}\n\n")], false, 0);
                GameTextPrinter.DefaultInstance.PrintLine([.. item.Description.Split("\n").Select(s => new TextPacket(s))], false, 0);
                GameTextPrinter.DefaultInstance.WaitForInput();
                break;
            }
        }
    }



    private string GetEquipmentKindText(EquipmentKind kind) =>
        kind switch
        {
            EquipmentKind.Weapon => "Weapon",
            EquipmentKind.BodyArmor => "Body",
            EquipmentKind.LegArmor => "Legs",
            EquipmentKind.HeadArmor => "Head",
            _ => throw new InvalidOperationException("Unknown equipment kind.")
        };
}