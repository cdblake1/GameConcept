using ConsoleGameImpl.State;
using GameData;
using GameData.src.Player;
using static TabMenuNavigator;

class InventoryScene
{
    public static InventoryScene Create()
    {
        return new InventoryScene();
    }

    public void ShowScene()
    {
        if (GlobalGameState.Instance.Player is not PlayerDefinition player)
        {
            throw new InvalidOperationException("Player is not initialized.");
        }

        var menu = new TabMenuNavigator("Inventory", [
            ("Equipment", GetEquipment(player), ShowEquipmentMenu(player)),
            ("Crafting", GetCraftingMaterials(player), ShowCraftingMaterialsMenu(player)),
        ]);

        menu.ShowTabbedMenu();
    }

    TabbedMenu GetEquipment(PlayerDefinition player)
    {
        return new TabbedMenu("Equipment", player.Inventory.Equipment.Select(e => new MenuOption(e.Name)).ToList());
    }

    private static Action<int> ShowEquipmentMenu(PlayerDefinition player) => (int selectedIndex) =>
    {
        var selectedItem = player.Inventory.Equipment[selectedIndex];

        EquipmentScene.ShowEquipmentMenu(player, selectedItem);
    };

    TabbedMenu GetCraftingMaterials(PlayerDefinition player)
    {
        return new TabbedMenu("Crafting Materials", [..player.Inventory.CraftingMaterials
            .Select(material => new MenuOption([GameTextPrinter.GetItemText(material)]))]);
    }

    Action<int> ShowCraftingMaterialsMenu(PlayerDefinition player) => (int selectedIndex) =>
    {
        var selectedItem = player.Inventory.CraftingMaterials[selectedIndex];

        GameTextPrinter.DefaultInstance.PrintLine([GameTextPrinter.GetItemText(selectedItem)], false, 0);

        var seeRecipes = "See Recipes";
        var menu = new Menu(null, [
            new(seeRecipes),
            new("Inspect"),
            new("Back")
        ]).ShowMenu();

        switch (menu)
        {
            case -1:
                break; // Exit
            case 0:
                GameTextPrinter.DefaultInstance.NotImplementedText(seeRecipes);
                break;
            case 1:
                Console.Clear();
                GameTextPrinter.DefaultInstance.PrintLine([GameTextPrinter.GetItemText(selectedItem)], true, 0);
                GameTextPrinter.DefaultInstance.PrintLine([.. selectedItem.Description.Split("\n").Select(s => new TextPacket(s))], true, 0);
                GameTextPrinter.DefaultInstance.PrintLine([new($"Amount: {selectedItem.Count}")], true, 0);
                GameTextPrinter.DefaultInstance.PrintLine([new($"Value: {selectedItem.Amount}")], true, 0);
                GameTextPrinter.DefaultInstance.WaitForInput();

                break;
        }
    };



}