using ConsoleGameImpl.Scene;

public static class KhabibGame
{

    public static void Main(string[] args)
    {

        while (true)
        {
            MainMenuScene.ShowMainMenu();
            MainGameScene.ShowScene();
        }
    }
}

// void EncounterLoop()
// {
//     while (true)
//     {
//         var encounters = new List<dynamic>
//         {
//             EncounterTemplates.SpringLandsEncounter.FromDurationRange(playerOne.Level.CurrentLevel, 1, 3),
//             EncounterTemplates.GoblinEncampment.FromDurationRange(playerOne.Level.CurrentLevel, 1, 3)
//         };

//         WriteToConsole("What would you like to do?", 0, true);
//         for (int i = 0; i < encounters.Count; i++)
//         {
//             WriteToConsole($"{i + 1}. {encounters[i].Name} - {encounters[i].Description}", 0, false);
//         }
//         WriteToConsole("0. Go back", 0, false);

//         var encounterInput = Console.ReadKey();
//         int selectedIndex = -1;
//         if (int.TryParse(encounterInput.KeyChar.ToString(), out selectedIndex) && selectedIndex > 0 && selectedIndex <= encounters.Count)
//         {
//             var encounter = encounters[selectedIndex - 1];
//             HandleEncounter(encounter);
//             return;
//         }
//         else if (encounterInput.KeyChar == '0')
//         {
//             return;
//         }
//         else
//         {
//             WriteToConsole("Invalid input. Please try again.", 0, false);
//         }
//     }
// }

// void HandleEncounter(dynamic encounter)
// {
//     while (encounter.EncounterIsActive)
//     {
//         WriteToConsole($"You are in the {encounter.Name} encounter!", defaultDelay, true);
//         WriteToConsole($"You have {encounter.Duration - encounter.CurrentDuration} turns left.", defaultDelay, false);
//         WriteToConsole($"You have {playerOne.CurrentHealth}/{playerOne.MaxHealth} health remaining.", defaultDelay, false);

//         WaitForInput();

//         var input = new Menu("What would you like to do?", new List<MenuOption>
//         {
//             new MenuOption("Continue Encounter"),
//             new MenuOption("End Encounter Early", ConsoleKey.E)
//         }).ShowMenu();

//         switch (input)
//         {
//             case 0:
//                 var opponent = encounter.AdvanceEncounter();
//                 var outcome = new CombatScene(playerOne, opponent).StartScene();
//                 if (outcome == false)
//                 {
//                     encounter.EndEncounterEarly();
//                 }
//                 break;
//             case 1:
//                 encounter.EndEncounterEarly();
//                 WaitForInput();
//                 break;
//             default:
//                 continue;
//         }
//     }

//     if (!encounter.EncounterEndedEarly)
//     {
//         WriteToConsole($"You completed the {encounter.Name} encounter!", defaultDelay, true);
//         WaitForInput();

//         var reward = encounter.EncounterReward();
//         if (reward.GoldCoin is not null)
//         {
//             playerOne.Gold += reward.GoldCoin;
//             WriteToConsole($"You received {reward.GoldCoin.Amount} gold coins.", defaultDelay, false);
//             WaitForInput();
//         }

//         foreach (var item in reward.Loot)
//         {
//             if (item is CraftingMaterial material)
//             {
//                 WriteToConsole($"You received {material.Count} {material.Name}!", defaultDelay, false);
//             }
//             else if (item is Equipment equipment)
//             {
//                 WriteToConsole($"You received {equipment.Name}!", defaultDelay, false);
//             }
//             else
//             {
//                 WriteToConsole($"You received {item.Name}!", defaultDelay, false);
//             }

//             playerOne.Inventory.AddItem(item);
//         }

//         WaitForInput();
//     }
//     else
//     {
//         WriteToConsole($"You ended the {encounter.Name} encounter early.", defaultDelay, true);
//         WaitForInput();
//     }
// }

// void MainOptionsLoop()
// {
//     var mainMEnu = new Menu("Main Menu",
//     [
//         new MenuOption("New Game"),
//         new MenuOption("Load Game"),
//         new MenuOption("Settings"),
//         new MenuOption("Exit Game")
//     ]);

//     var input = mainMEnu.ShowMenu();
//     switch (input)
//     {
//         case 0:
//             WriteToConsole("Game Started!");
//             WriteToConsole(".", 300, false);
//             WriteToConsole(".", 300, false);
//             WriteToConsole(".", 300, false);

//             Console.WriteLine();
//             playerOne = CaptureCharacterDetails();
//             MainGameLoop();
//             break;
//         case 1:
//             WriteToConsole("Loading game...", defaultDelay, true);
//             break;
//         case 2:
//             WriteToConsole("Settings menu is not implemented yet.", defaultDelay, true);
//             break;
//         case 3:
//             WriteToConsole("Exiting game...", defaultDelay, true);
//             Environment.Exit(0);
//             break;
//         default:
//             WriteToConsole("Invalid input. Please try again.", 0, false);
//             break;
//     }
// }


// void MainGameLoop()
// {
//     var gameLoop = new Menu("Main Menu", new List<MenuOption>
//     {
//         new MenuOption("Check Active Encounters"),
//         new MenuOption("Check Inventory", ConsoleKey.I),
//         new MenuOption("Check Equipment", ConsoleKey.E),
//         new MenuOption("Check Stats", ConsoleKey.A),
//         new MenuOption("Heal at the Inn", ConsoleKey.H),
//         new MenuOption("Go to the Shop", ConsoleKey.S),
//         new MenuOption("Use Crafting Station", ConsoleKey.C),
//         new MenuOption("Exit Game", ConsoleKey.Escape)
//     });

//     var input = gameLoop.ShowMenu();
//     switch (input)
//     {
//         case 0:
//             EncounterLoop();
//             break;
//         case 1:
//             DisplayInventory();
//             break;
//         case 2:
//             CheckEquipment();
//             break;
//         case 3:
//             CheckStats();
//             break;
//         case 4:
//             WriteToConsole("You healed at the inn.", 0, true);
//             playerOne.CurrentHealth = playerOne.MaxHealth;
//             WaitForInput();
//             break;
//         case 5:
//             ShopScene();
//             break;
//         case 6:
//             CraftingLoop();
//             break;
//         case 7:
//             WriteToConsole("You exited the game.", 0, true);
//             WaitForInput();
//             Environment.Exit(0);
//             break;
//         case -1:
//             WriteToConsole("You exited the game.", 0, true);
//             WaitForInput();
//             Environment.Exit(0);
//             break;
//         default:
//             WriteToConsole("Invalid input. Please try again.", 0, false);
//             break;
//     }
// }

// void CraftingLoop()
// {
//     WriteToConsole("You used the crafting station.", 0, true);
//     while (true)
//     {
//         WriteToConsole("Which crafting building?", 0, true);
//         WriteToConsole("1. The Forgery", 0, false);
//         WriteToConsole("0. Go back", 0, false);
//         var input2 = Console.ReadKey();
//         if (input2.KeyChar == '1')
//         {
//             while (true)
//             {
//                 WriteToConsole("Available recipes:", 0, true);
//                 if (craftingStation.Recipes.Count == 0)
//                 {
//                     WriteToConsole("No recipes available.");
//                     WaitForInput();
//                     break;
//                 }
//                 for (int i = 0; i < craftingStation.Recipes.Count; i++)
//                 {
//                     var recipe = craftingStation.Recipes[i];
//                     WriteToConsole($"{i + 1}. {recipe.CraftedItem.Name} - {recipe.CraftedItem.Description}", 0, false);
//                 }
//                 WriteToConsole("0. Go back", 0, false);

//                 var recipeInput = Console.ReadLine();
//                 if (recipeInput == "0")
//                 {
//                     break;
//                 }
//                 if (int.TryParse(recipeInput, out int recipeIndex) && recipeIndex > 0 && recipeIndex <= craftingStation.Recipes.Count)
//                 {
//                     var selectedRecipe = craftingStation.Recipes[recipeIndex - 1];
//                     WriteToConsole($"You selected {selectedRecipe.CraftedItem.Name}.", 0, true);
//                     while (true)
//                     {
//                         WriteToConsole("What would you like to do?", 0, false);
//                         WriteToConsole("1. Inspect Recipe", 0, false);
//                         WriteToConsole("2. Craft Item", 0, false);
//                         WriteToConsole("0. Cancel", 0, false);

//                         var recipeActionInput = Console.ReadKey();
//                         if (recipeActionInput.KeyChar == '1')
//                         {
//                             WriteToConsole($"{selectedRecipe.CraftedItem.Name} - {selectedRecipe.CraftedItem.Description}", 0, true);
//                             WriteToConsole("Required Materials:", 0, false);
//                             foreach (var material in selectedRecipe.RequiredMaterials)
//                             {
//                                 if (material is CraftingMaterial craftingMaterial)
//                                 {
//                                     WriteToConsole($"{craftingMaterial.Name} - {craftingMaterial.Description}", 0, false);
//                                 }
//                                 else if (material is Equipment equipment)
//                                 {
//                                     WriteToConsole($"{equipment.Name} - {equipment.Description}", 0, false);
//                                 }
//                                 else
//                                 {
//                                     WriteToConsole($"{material.Name} - {material.Description}", 0, false);
//                                 }
//                             }

//                             WaitForInput();
//                         }
//                         else if (recipeActionInput.KeyChar == '2')
//                         {
//                             for (int i = 0; i < selectedRecipe.RequiredMaterials.Count; i++)
//                             {
//                                 var material = selectedRecipe.RequiredMaterials[i];
//                                 if (material is CraftingMaterial craftingMaterial)
//                                 {
//                                     if (!playerOne.Inventory.HasAmount(craftingMaterial))
//                                     {
//                                         WriteToConsole($"You don't have enough {craftingMaterial.Name} to craft {selectedRecipe.CraftedItem.Name}. You have {playerOne.Inventory.GetAmount(craftingMaterial)}, but need {craftingMaterial.Count}.", defaultDelay, true);
//                                         WaitForInput();
//                                         break;
//                                     }
//                                 }
//                                 else if (material is Equipment equipment)
//                                 {
//                                     if (!playerOne.Inventory.HasAmount(equipment))
//                                     {
//                                         WriteToConsole($"You don't have the required equipment to craft {selectedRecipe.CraftedItem.Name}.", defaultDelay, true);
//                                         WaitForInput();
//                                         break;
//                                     }
//                                 }
//                             }

//                             // Add crafted item to inventory
//                             playerOne.Inventory.AddItem(selectedRecipe.CraftedItem);
//                             WriteToConsole($"You crafted {selectedRecipe.CraftedItem.Name}.", defaultDelay, true);

//                             var totalMaterialsLeft = playerOne.Inventory.CraftingMaterials.Sum(mat => mat.Count);
//                             WriteToConsole($"You have {totalMaterialsLeft} crafting materials left.", defaultDelay, true);

//                             WaitForInput();
//                             break;
//                         }
//                         else if (recipeActionInput.KeyChar == '0')
//                         {
//                             break;
//                         }
//                         else
//                         {
//                             WriteToConsole("Invalid input. Please try again.", 0, false);
//                         }
//                     }
//                 }
//                 else
//                 {
//                     WriteToConsole("Invalid input. Please try again.", 0, false);
//                 }
//             }
//         }
//         else if (input2.KeyChar == '0')
//         {
//             break;
//         }
//         else
//         {
//             WriteToConsole("Invalid input. Please try again.", 0, false);
//             continue;
//         }
//     }
//     WaitForInput();
// }

// void CheckEquipment()
// {
//     WriteToConsole("You checked your equipment.", 500, true);
//     Console.WriteLine();

//     var slots = new List<EquipmentKind>();
//     foreach (var (slot, item) in playerOne.Equipment.GetAllEquipment())
//     {
//         slots.Add(slot);
//         if (item is Equipment equippedItem)
//         {
//             WriteToConsole($"{slot}: {equippedItem.Name} - {equippedItem.Description}", 250);
//         }
//         else
//         {
//             WriteToConsole($"{slot}: Empty");
//         }
//     }

//     WaitForInput();

//     while (true)
//     {
//         WriteToConsole("What would you like to do?", 0, true);
//         WriteToConsole("1. Equip Item", 0, false);
//         WriteToConsole("2. Unequip Item", 0, false);
//         WriteToConsole("3. Exit Equipment", 0, false);

//         var input2 = Console.ReadLine();
//         if (input2 == "1")
//         {
//             while (true)
//             {
//                 WriteToConsole("Enter the number of the slot you want to equip an item to:");
//                 for (int i = 0; i < slots.Count; i++)
//                 {
//                     WriteToConsole($"{i + 1}. {slots[i]}", 0, false);
//                 }

//                 var slotInput = Console.ReadLine();
//                 if (int.TryParse(slotInput, out int slotIndex) && slotIndex > 0 && slotIndex <= slots.Count)
//                 {
//                     var selectedSlot = slots[slotIndex - 1];
//                     var item = playerOne.Inventory.GetItemsOfKind(selectedSlot);
//                     if (item.Count == 0)
//                     {
//                         WriteToConsole($"You have no items of kind {selectedSlot} in your inventory.", defaultDelay, true);
//                         WaitForInput();
//                         break;
//                     }
//                     else
//                     {
//                         WriteToConsole($"You have the following items of kind {selectedSlot} in your inventory:", 0, false);
//                         for (int i = 0; i < item.Count; i++)
//                         {
//                             WriteToConsole($"{i + 1}. {item[i].Name} - {item[i].Description}", 0, false);
//                         }
//                         WriteToConsole("0. Exit", 0, false);

//                         WriteToConsole("Enter the number of the item you want to equip:");
//                         var itemInput = Console.ReadLine();
//                         if (itemInput == "0")
//                         {
//                             break;
//                         }
//                         if (int.TryParse(itemInput, out int itemIndex) && itemIndex > 0 && itemIndex <= item.Count)
//                         {
//                             var selectedItem = item[itemIndex - 1];
//                             WriteToConsole($"You equipped {selectedItem.Name}.", defaultDelay, true);
//                             playerOne.EquipItem(selectedItem);

//                             WaitForInput();
//                             break;
//                         }
//                         else
//                         {
//                             WriteToConsole("Invalid input. Please try again.", 0, false);
//                             continue;
//                         }
//                     }
//                 }
//                 else
//                 {
//                     WriteToConsole("Invalid input. Please try again.", 0, false);
//                     continue;
//                 }
//             }
//         }
//         else if (input2 == "2")
//         {
//             var itemKinds = new List<EquipmentKind>();
//             WriteToConsole("Enter the number of the slot you want to unequip:");
//             for (int i = 0; i < itemKinds.Count; i++)
//             {
//                 WriteToConsole($"{i + 1}. {itemKinds[i]}", 0, false);
//             }

//             var slotInput = Console.ReadLine();
//             if (int.TryParse(slotInput, out int slotIndex) && slotIndex > 0 && slotIndex <= itemKinds.Count)
//             {
//                 var selectedSlot = itemKinds[slotIndex - 1];
//                 playerOne.UnequipItem(selectedSlot);
//                 WriteToConsole($"You unequipped the item from {selectedSlot}.", 1000, true);
//                 WaitForInput();
//                 break;
//             }
//             else
//             {
//                 WriteToConsole("Invalid input. Please try again.");
//                 continue;
//             }
//             // Logic to unequip an item
//         }
//         else if (input2 == "3")
//         {
//             break;
//         }
//         else
//         {
//             WriteToConsole("Invalid input. Please try again.", 0, false);
//             continue;
//         }
//     }
// }

// void DisplayInventory()
// {
//     WriteToConsole("You checked your inventory.", 500, true);
//     Console.WriteLine();
//     if (playerOne.Inventory.Equipment.Count == 0 && playerOne.Inventory.CraftingMaterials.Count == 0 && playerOne.Inventory.CraftingMaterials.Count == 0)
//     {
//         WriteToConsole("Your inventory is empty.");
//         WaitForInput();
//         return;
//     }

//     WriteToConsole("Equipment:", 0, true);
//     foreach (var item in playerOne.Inventory.Equipment)
//     {
//         WriteToConsole($"{item.Name} - {item.Description}", 0, false);
//     }

//     WaitForInput();

//     WriteToConsole("Crafting Materials:", 0, true);
//     foreach (var item in playerOne.Inventory.CraftingMaterials)
//     {
//         WriteToConsole($"{item.Name}: {item.Count}", 0, false);
//     }

//     WaitForInput();
// }

// void CheckStats()
// {
//     WriteToConsole($"{playerOne.Name} Stats.", 0, true);
//     WriteToConsole($"Level: {playerOne.Level.CurrentLevel}");
//     WriteToConsole($"Experience: {playerOne.Level.CurrentExperience} / {playerOne.Level.GetExperienceNeededForNextLevel()}");
//     WriteToConsole($"Health: {playerOne.CurrentHealth}/{playerOne.MaxHealth}");
//     WriteToConsole($"Attack Power: {playerOne.Stats.AttackPower}");
//     WriteToConsole($"Defense: {playerOne.Stats.Defense}");
//     WaitForInput();
// }

// void WaitForInput()
// {
//     Console.WriteLine();
//     WriteToConsole("Press any key to continue...", 0, false);
//     Console.ReadKey();
// }

// void ShopScene()
// {
//     WriteToConsole("You entered the shop.", defaultDelay, true);
//     WaitForInput();

//     while (true)
//     {
//         Console.WriteLine();
//         WriteToConsole("What would you like to do?", 0, true);
//         WriteToConsole("1. View Items", 0, false);
//         WriteToConsole("2. Sell Item", 0, false);
//         WriteToConsole("3. Exit Shop", 0, false);

//         var input = Console.ReadKey();
//         switch (input.KeyChar)
//         {
//             case '1':
//                 HandleViewItems();
//                 break;
//             case '2':
//                 HandleSellItems();
//                 break;
//             case '3':
//                 return;
//             default:
//                 WriteToConsole("Invalid input. Please try again.", 0, false);
//                 break;
//         }
//     }
// }

// void HandleViewItems()
// {
//     while (true)
//     {
//         WriteToConsole($"You have {playerOne.Gold.Amount} gold.", defaultDelay, true);
//         WriteToConsole("Please select an item", 0, true);

//         for (int i = 0; i < shop.Items.Count; i++)
//         {
//             var item = shop.Items[i];
//             WriteToConsole($"{i + 1}. {item.Name} - {item.Description} - {item.Amount} gold", 0, false);
//         }
//         WriteToConsole("0. Go back", 0, false);

//         var itemInput = Console.ReadKey();
//         if (itemInput.KeyChar == '0') break;

//         if (int.TryParse(itemInput.KeyChar.ToString(), out int itemIndex) && itemIndex > 0 && itemIndex <= shop.Items.Count)
//         {
//             HandleBuyItem(shop.Items[itemIndex - 1]);
//         }
//         else
//         {
//             WriteToConsole("Invalid input. Please try again.", 0, false);
//         }
//     }
// }

// void HandleBuyItem(IItem selectedItem)
// {
//     while (true)
//     {
//         WriteToConsole($"You have {playerOne.Gold.Amount} gold.", defaultDelay, true);
//         WriteToConsole($"{selectedItem.Name}", 0, false);
//         WriteToConsole($"Price: {selectedItem.Amount.Amount} gold.", defaultDelay, false);
//         Console.WriteLine();
//         WriteToConsole("What would you like to do?", 0, false);
//         WriteToConsole("1. Inspect Item", 0, false);
//         WriteToConsole("2. Buy Item", 0, false);
//         WriteToConsole("3. Cancel", 0, false);

//         var buyInput = Console.ReadKey();
//         switch (buyInput.KeyChar)
//         {
//             case '1':
//                 WriteToConsole($"{selectedItem.Description}", defaultDelay, true);
//                 WaitForInput();
//                 break;
//             case '2':
//                 ConfirmPurchase(selectedItem);
//                 return;
//             case '3':
//                 return;
//             default:
//                 WriteToConsole("Invalid input. Please try again.", 0, false);
//                 break;
//         }
//     }
// }

// void ConfirmPurchase(IItem selectedItem)
// {
//     if (playerOne.Gold >= selectedItem.Amount)
//     {
//         while (true)
//         {
//             WriteToConsole($"You have {playerOne.Gold.Amount} gold.", defaultDelay, true);
//             WriteToConsole($"Are you sure you want to buy {selectedItem.Name} for {selectedItem.Amount} gold?", 0, false);
//             WriteToConsole("1. Yes", 0, false);
//             WriteToConsole("2. No", 0, false);

//             var confirmInput = Console.ReadKey();
//             if (confirmInput.KeyChar == '1')
//             {
//                 var item = shop.Buy(selectedItem);
//                 playerOne.Inventory.AddItem(item);
//                 WriteToConsole($"You bought {selectedItem.Name}.", defaultDelay, true);
//                 WaitForInput();
//                 return;
//             }
//             else if (confirmInput.KeyChar == '2')
//             {
//                 return;
//             }
//             else
//             {
//                 WriteToConsole("Invalid input. Please try again.", 0, false);
//             }
//         }
//     }
//     else
//     {
//         WriteToConsole($"You don't have enough gold to buy {selectedItem.Name}.", 0, true);
//         WriteToConsole($"You need {selectedItem.Amount.Amount - playerOne.Gold.Amount} more gold to buy {selectedItem.Name}.", defaultDelay, false);
//         WaitForInput();
//     }
// }

// void HandleSellItems()
// {
//     while (true)
//     {
//         WriteToConsole("Which Item would you like to sell?", 0, true);
//         var count = 0;
//         var combinedItems = new List<IItem>();
//         foreach (var item in playerOne.Inventory.Equipment)
//         {
//             count++;
//             combinedItems.Add(item);
//             WriteToConsole($"{count}. {item.Name} - {item.Description} - {item.Amount.Amount} gold", 0, false);
//         }
//         foreach (var item in playerOne.Inventory.CraftingMaterials)
//         {
//             count++;
//             combinedItems.Add(item);
//             WriteToConsole($"{count}. {item.Name} - {item.Description} - {item.Amount.Amount} gold", 0, false);
//         }

//         WriteToConsole("0. Go back", 0, false);

//         var itemInput = Console.ReadKey();
//         if (itemInput.KeyChar == '0') break;

//         if (int.TryParse(itemInput.KeyChar.ToString(), out int itemIndex) && itemIndex > 0 && itemIndex <= combinedItems.Count)
//         {
//             var selectedItem = combinedItems[itemIndex - 1];
//             if (selectedItem is Equipment item)
//             {
//                 ConfirmSellItem(item);
//             }
//             else if (selectedItem is CraftingMaterial material)
//             {
//                 WriteToConsole($"How many {material.Name} do you want to sell?", 0, false);
//                 WriteToConsole($"You have {material.Count} {material.Name}.", 0, false);
//                 var amountInput = Console.ReadLine();
//                 if (int.TryParse(amountInput, out int amount) && amount > 0 && amount <= material.Count)
//                 {
//                     playerOne.Inventory.RemoveItem(material);
//                     playerOne.Gold += shop.Sell(material, amount);
//                     WriteToConsole($"You sold {amount} {material.Name} for {material.Amount.Amount * amount} gold.", defaultDelay, true);
//                     WaitForInput();
//                 }
//                 else
//                 {
//                     WriteToConsole("Invalid input. Please try again.", 0, false);
//                 }
//             }

//             ConfirmSellItem(combinedItems[itemIndex - 1]);
//         }
//         else
//         {
//             WriteToConsole("Invalid input. Please try again.", 0, false);
//         }
//     }
// }

// void ConfirmSellItem(IItem selectedItem)
// {
//     WriteToConsole($"You selected {selectedItem.Name}.", defaultDelay, true);
//     WriteToConsole($"You can sell it for {selectedItem.Amount.Amount} gold.", defaultDelay, true);
//     WriteToConsole("Do you want to sell this item?", 0, false);
//     WriteToConsole("1. Yes", 0, false);
//     WriteToConsole("2. No", 0, false);

//     var confirmInput = Console.ReadKey();
//     if (confirmInput.KeyChar == '1')
//     {
//         playerOne.Inventory.RemoveItem(selectedItem);
//         playerOne.Gold += shop.Sell(selectedItem);
//         WriteToConsole($"You sold {selectedItem.Name} for {selectedItem.Amount.Amount} gold.", defaultDelay, true);
//         WaitForInput();
//     }
//     else if (confirmInput.KeyChar != '2')
//     {
//         WriteToConsole("Invalid input. Please try again.", 0, false);
//     }
// }
