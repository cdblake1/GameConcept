using ConsoleGameImpl.State;
using GameLogic.Player;
using Infrastructure.Json.Repositories.Initialize;
using static TabMenuNavigator;

class InventoryScene
{
    public static InventoryScene Create()
    {
        return new InventoryScene();
    }

    public void ShowScene()
    {
        if (GlobalGameState.Instance.Player is not PlayerInstance player)
        {
            throw new InvalidOperationException("Player is not initialized.");
        }

        var menu = new TabMenuNavigator("Inventory", [
            ("Equipment", GetEquipment(player), ShowEquipmentMenu(player)),
            ("Crafting", GetCraftingMaterials(player), ShowCraftingMaterialsMenu(player)),
        ]);

        menu.ShowTabbedMenu();
    }

    TabbedMenu GetEquipment(PlayerInstance player)
    {
        return new TabbedMenu("Equipment", [.. player.Inventory.GetAllEquipment().Select(e => new MenuOption(e.Presentation.Name))]);
    }

    private static Action<int> ShowEquipmentMenu(PlayerInstance player) => (int selectedIndex) =>
    {
        var selectedItem = player.Inventory.GetAllEquipment()[selectedIndex];

        EquipmentScene.ShowEquipmentMenu(player, selectedItem);
    };

    TabbedMenu GetCraftingMaterials(PlayerInstance player)
    {
        return new TabbedMenu("Crafting Materials", [..player.Inventory.GetAllCraftingMaterials()
            .Select(material => new MenuOption([GameTextPrinter.GetItemText(material)]))]);
    }

    Action<int> ShowCraftingMaterialsMenu(PlayerInstance player) => (int selectedIndex) =>
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
                ShowRecipesForMaterial(selectedItem);
                break;
            case 1:
                Console.Clear();
                GameTextPrinter.DefaultInstance.PrintLine([GameTextPrinter.GetItemText(selectedItem)], true, 0);
                GameTextPrinter.DefaultInstance.PrintLine([.. selectedItem.Presentation.Description.Split("\n").Select(s => new TextPacket(s))], true, 0);
                GameTextPrinter.DefaultInstance.PrintLine([new($"Rarity: {selectedItem.Rarity}")], true, 0);
                GameTextPrinter.DefaultInstance.WaitForInput();
                break;
        }
    };

    private void ShowRecipesForMaterial(GameData.src.Item.CraftingMaterialDefinition material)
    {
        var recipes = Repositories.CraftingRecipeRepository.GetAll()
            .Where(recipe => recipe.Materials.Any(m => m.ItemId == material.Id))
            .ToList();

        if (!recipes.Any())
        {
            GameTextPrinter.DefaultInstance.PrintLine([new($"No recipes found for {material.Presentation.Name}")], true, 0);
            GameTextPrinter.DefaultInstance.WaitForInput();
            return;
        }

        GameTextPrinter.DefaultInstance.PrintLine([new($"Recipes using {material.Presentation.Name}:")], true, 0);

        foreach (var recipe in recipes)
        {
            var craftedItem = Repositories.ItemRepository.Get(recipe.CraftedItemId);
            GameTextPrinter.DefaultInstance.PrintLine([new($"- {craftedItem.Presentation.Name}")], false, 0);

            GameTextPrinter.DefaultInstance.PrintLine([new("  Required materials:")], false, 0);
            foreach (var (itemId, count) in recipe.Materials)
            {
                var requiredItem = Repositories.ItemRepository.Get(itemId);
                GameTextPrinter.DefaultInstance.PrintLine([new($"    {requiredItem.Presentation.Name} x{count}")], false, 0);
            }
        }

        GameTextPrinter.DefaultInstance.WaitForInput();
    }
}