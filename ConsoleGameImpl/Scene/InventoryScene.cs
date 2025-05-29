using GameData;
using static TabMenuNavigator;

class InventoryScene
{
    public static InventoryScene Create()
    {
        return new InventoryScene();
    }

    public void ShowScene()
    {
        if (GlobalGameState.Instance.Player is not Player player)
        {
            throw new InvalidOperationException("Player is not initialized.");
        }

        Console.Clear();
        GameTextPrinter.DefaultInstance.Print("Welcome to the inventory scene!");
        GameTextPrinter.DefaultInstance.WaitForInput();

        var menu = new TabMenuNavigator("Inventory", [
            ("Equipment", GetEquipment(player), ShowEquipmentMenu(player)),
            ("Crafting", GetCraftingMaterials(player), ShowCraftingMaterialsMenu(player)),
        ]);

        menu.ShowTabbedMenu();
    }

    TabbedMenu GetEquipment(Player player)
    {
        return new TabbedMenu("Equipment", player.Inventory.Equipment.Select(e => new MenuOption(e.Name)).ToList());
    }

    private static Action<int> ShowEquipmentMenu(Player player) => (int selectedIndex) =>
    {
        var selectedItem = player.Inventory.Equipment[selectedIndex];

        EquipmentScene.ShowEquipmentMenu(player, selectedItem);
    };

    TabbedMenu GetCraftingMaterials(Player player)
    {
        return new TabbedMenu("Crafting Materials", player.Inventory.CraftingMaterials
            .Select(material => new MenuOption($"{material.Name}: {material.Amount}")).ToList());
    }

    Action<int> ShowCraftingMaterialsMenu(Player player) => (int selectedIndex) =>
    {
        var selectedItem = player.Inventory.CraftingMaterials[selectedIndex];

        GameTextPrinter.DefaultInstance.Print(GameTextPrinter.GetItemText(selectedItem));
        GameTextPrinter.DefaultInstance.Print($"Amount: {selectedItem.Amount}");

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
                GameTextPrinter.DefaultInstance.Print(GameTextPrinter.GetItemText(selectedItem));
                GameTextPrinter.DefaultInstance.Print(selectedItem.Description.Split("\n"));
                GameTextPrinter.DefaultInstance.Print($"Amount: {selectedItem.Amount}");
                break;
        }
    };


}