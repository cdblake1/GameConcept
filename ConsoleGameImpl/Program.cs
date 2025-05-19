using GameDataLayer;

const int defaultDelay = 500;

var shop = new Shop("General Store", new List<IItem>()
{
    new HealthPotionBase.MinorHealthPotion()
});

bool inCombat;
WriteToConsole("Game Started!");
WriteToConsole(".");
WriteToConsole(".");
WriteToConsole(".");
Console.WriteLine();
var playerOne = CaptureCharacterDetails();
while (true)
{
    MainOptionsLoop();
}

void WriteToConsole(string message, int delayMs = defaultDelay, bool clearConsole = false)
{
    if (clearConsole)
    {
        Console.Clear();
    }

    Console.WriteLine(message);
    Thread.Sleep(delayMs);
}

void StartCombatSession(MobBase opponent)
{
    inCombat = true;
    WriteToConsole($"Player encounters {opponent.Name}!", defaultDelay, true);
    WaitForInput();

    while (inCombat)
    {
        DisplayCombatStatus(opponent);

        WriteToConsole("What do you want to do?", 0, false);
        WriteToConsole("1. Attack", 0, false);
        WriteToConsole("2. Check Consumables", 0, false);
        WriteToConsole("3. Run", 0, false);

        var input = Console.ReadKey();
        switch (input.KeyChar)
        {
            case '1':
                PerformPlayerAttack(opponent);

                if (opponent.CurrentHealth <= 0)
                {
                    WriteToConsole($"The {opponent.Name} has been defeated!", defaultDelay, true);
                    inCombat = false;
                    WaitForInput();

                    var experience = opponent.AwardExperience();
                    WriteToConsole($"You gained {experience} experience points.", defaultDelay, true);
                    WaitForInput();
                    if (playerOne.LevelManager.AddExperience(experience))
                    {
                        WriteToConsole($"Congratulations! You leveled up to level {playerOne.LevelManager.CurrentLevel}!", defaultDelay, true);
                        WaitForInput();
                        WriteToConsole($"Your stats have increased!", defaultDelay, true);
                        WriteToConsole($"{nameof(playerOne.LevelManager.Stats.AttackPower)} +{playerOne.LevelManager.Stats.AttackPower}", defaultDelay, false);
                        WriteToConsole($"{nameof(playerOne.LevelManager.Stats.Defense)} +{playerOne.LevelManager.Stats.Defense}", defaultDelay, false);
                        WriteToConsole($"{nameof(playerOne.MaxHealth)} +{playerOne.MaxHealth}", defaultDelay, false);
                    }
                    else
                    {
                        WriteToConsole($"You need {playerOne.LevelManager.GetExperienceNeededForNextLevel() - playerOne.LevelManager.CurrentExperience} more experience points to level up!", 0, false);
                        WaitForInput();
                    }

                    var loot = opponent.DropLoot();
                    if (loot is Equipment item)
                    {
                        WriteToConsole($"You found {item.Name}!", defaultDelay, true);
                        WaitForInput();

                        playerOne.Inventory.AddItem(item);

                        WriteToConsole($"You added {item.Name} to your inventory.", defaultDelay, true);
                    }
                    else
                    {
                        WriteToConsole("No loot dropped.", defaultDelay, true);
                    }
                    WaitForInput();

                    return;
                }

                PerformOpponentAttack(opponent);
                if (playerOne.CurrentHealth <= 0)
                {
                    WriteToConsole("You have been defeated!");
                    inCombat = false;
                    WaitForInput();
                    return;
                }

                break;
            case '2':
                WriteToConsole("You have no consumables in your inventory.", 1000);
                break;

            case '3':
                WriteToConsole("You fled the battle!", 1000, true);
                inCombat = false;
                break;

            default:
                WriteToConsole("Invalid input. Please try again.");
                break;
        }
    }
}

void DisplayCombatStatus(CharacterBase opponent)
{
    WriteToConsole($"You are in combat with a {opponent.Name}!", defaultDelay, true);
    WriteToConsole($"""
        {playerOne.Name} Level: {playerOne.LevelManager.CurrentLevel}
        {playerOne.Name} Health: {playerOne.CurrentHealth}/{playerOne.MaxHealth}
        {playerOne.LevelManager.CurrentExperience} / {playerOne.LevelManager.GetExperienceNeededForNextLevel()} experience to next level

        {opponent.Name} Level: {opponent.LevelManager.CurrentLevel}
        {opponent.Name} Health: {opponent.CurrentHealth}/{opponent.MaxHealth}
        """, 0, false);
    Console.WriteLine();
}

void PerformPlayerAttack(CharacterBase opponent)
{
    WriteToConsole($"You attack the {opponent.Name}!", 0, true);
    Console.WriteLine();
    var damage = playerOne.Attack(opponent);
    WriteToConsole($"You dealt {damage} damage to the {opponent.Name}.");
    WriteToConsole($"The {opponent.Name} has {opponent.CurrentHealth} health remaining.");

    WaitForInput();
}

void PerformOpponentAttack(CharacterBase opponent)
{
    WriteToConsole($"The {opponent.Name} attacks back!", 0, true);
    Console.WriteLine();

    var opponentDamage = opponent.Attack(playerOne);
    WriteToConsole($"The {opponent.Name} dealt {opponentDamage} damage to you.");
    WriteToConsole($"Your health is now {playerOne.CurrentHealth}.");

    WaitForInput();
}

CharacterBase CaptureCharacterDetails()
{
    while (true)
    {
        WriteToConsole("What is your name?");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            WriteToConsole("Name cannot be empty. Please try again.");
            continue;
        }

        var player = new CharacterBase(name, new StatTemplate
        {
            AttackPower = 20,
            Defense = 5,
            Health = 100
        },
        new LevelManager(15, ExperienceTable.Default, new StatTemplate
        {
            AttackPower = 1,
            Defense = 1,
            Health = 5
        }, 1));
        WriteToConsole($"Welcome, {name}!");
        return player;
    }
}

void MainOptionsLoop()
{
    WriteToConsole("What would you like to do?", 500, true);
    WriteToConsole("1. Battle! I want loot!", 0, false);
    WriteToConsole("2. Check Inventory", 0, false);
    WriteToConsole("3. Check Equipment", 0, false);
    WriteToConsole("4. Check Stats", 0, false);
    WriteToConsole("5. Heal at the Inn", 0, false);
    WriteToConsole("6. Go to the Shop", 0, false);
    WriteToConsole("0. Exit Game", 0, false);

    var input = Console.ReadKey();
    if (input.KeyChar == '1')
    {
        StartCombatSession(GetRandomOpponent(playerOne.LevelManager.CurrentLevel));
    }
    else if (input.KeyChar == '2')
    {
        DisplayInventory();
    }
    else if (input.KeyChar == '3')
    {
        CheckEquipment();
    }
    else if (input.KeyChar == '4')
    {
        CheckStats();
    }
    else if (input.KeyChar == '5')
    {
        WriteToConsole("You healed at the inn.", 0, true);
        playerOne.CurrentHealth = playerOne.MaxHealth;
        WaitForInput();
    }
    else if (input.KeyChar == '6')
    {
        ShopScene();
    }
    else if (input.KeyChar == '0')
    {
        WriteToConsole("Exiting game...", 0, true);
        Environment.Exit(0);
    }
    else
    {
        WriteToConsole("Invalid input. Please try again.", 0, false);
        MainOptionsLoop();
    }
}

void CheckEquipment()
{
    WriteToConsole("You checked your equipment.", 500, true);
    Console.WriteLine();

    var slots = new List<EquipmentKind>();
    foreach (var (slot, item) in playerOne.EquipmentManager.GetAllEquipment())
    {
        slots.Add(slot);
        if (item is Equipment equippedItem)
        {
            WriteToConsole($"{slot}: {equippedItem.Name} - {equippedItem.Description}", 250);
        }
        else
        {
            WriteToConsole($"{slot}: Empty");
        }
    }

    WaitForInput();

    while (true)
    {
        WriteToConsole("What would you like to do?", 0, true);
        WriteToConsole("1. Equip Item", 0, false);
        WriteToConsole("2. Unequip Item", 0, false);
        WriteToConsole("3. Exit Equipment", 0, false);

        var input2 = Console.ReadLine();
        if (input2 == "1")
        {
            while (true)
            {
                WriteToConsole("Enter the number of the slot you want to equip an item to:");
                for (int i = 0; i < slots.Count; i++)
                {
                    WriteToConsole($"{i + 1}. {slots[i]}", 0, false);
                }

                var slotInput = Console.ReadLine();
                if (int.TryParse(slotInput, out int slotIndex) && slotIndex > 0 && slotIndex <= slots.Count)
                {
                    var selectedSlot = slots[slotIndex - 1];
                    var item = playerOne.Inventory.GetItemsMatchingKind(selectedSlot);
                    if (item.Count == 0)
                    {
                        WriteToConsole($"You have no items of kind {selectedSlot} in your inventory.", defaultDelay, true);
                        WaitForInput();
                        break;
                    }
                    else
                    {
                        WriteToConsole($"You have the following items of kind {selectedSlot} in your inventory:", 0, false);
                        for (int i = 0; i < item.Count; i++)
                        {
                            WriteToConsole($"{i + 1}. {item[i].Name} - {item[i].Description}", 0, false);
                        }
                        WriteToConsole("0. Exit", 0, false);

                        WriteToConsole("Enter the number of the item you want to equip:");
                        var itemInput = Console.ReadLine();
                        if (itemInput == "0")
                        {
                            break;
                        }
                        if (int.TryParse(itemInput, out int itemIndex) && itemIndex > 0 && itemIndex <= item.Count)
                        {
                            var selectedItem = item[itemIndex - 1];
                            WriteToConsole($"You equipped {selectedItem.Name}.", defaultDelay, true);
                            playerOne.EquipItem(selectedItem);

                            WaitForInput();
                            break;
                        }
                        else
                        {
                            WriteToConsole("Invalid input. Please try again.", 0, false);
                            continue;
                        }
                    }
                }
                else
                {
                    WriteToConsole("Invalid input. Please try again.", 0, false);
                    continue;
                }
            }
        }
        else if (input2 == "2")
        {
            var itemKinds = new List<EquipmentKind>();
            WriteToConsole("Enter the number of the slot you want to unequip:");
            for (int i = 0; i < itemKinds.Count; i++)
            {
                WriteToConsole($"{i + 1}. {itemKinds[i]}", 0, false);
            }

            var slotInput = Console.ReadLine();
            if (int.TryParse(slotInput, out int slotIndex) && slotIndex > 0 && slotIndex <= itemKinds.Count)
            {
                var selectedSlot = itemKinds[slotIndex - 1];
                playerOne.UnequipItem(selectedSlot);
                WriteToConsole($"You unequipped the item from {selectedSlot}.", 1000, true);
                WaitForInput();
                break;
            }
            else
            {
                WriteToConsole("Invalid input. Please try again.");
                continue;
            }
            // Logic to unequip an item
        }
        else if (input2 == "3")
        {
            break;
        }
        else
        {
            WriteToConsole("Invalid input. Please try again.", 0, false);
            continue;
        }
    }
}

void DisplayInventory()
{
    WriteToConsole("You checked your inventory.", 500, true);
    Console.WriteLine();
    if (playerOne.Inventory.Count == 0)
    {
        WriteToConsole("Your inventory is empty.");
        WaitForInput();
        return;
    }

    foreach (var item in playerOne.Inventory)
    {
        WriteToConsole($"{item.Name} - {item.Description}", 0, false);
    }

    WaitForInput();
}

void CheckStats()
{
    WriteToConsole($"{playerOne.Name} Stats.", 0, true);
    WriteToConsole($"Level: {playerOne.LevelManager.CurrentLevel}");
    WriteToConsole($"Experience: {playerOne.LevelManager.CurrentExperience} / {playerOne.LevelManager.GetExperienceNeededForNextLevel()}");
    WriteToConsole($"Health: {playerOne.CurrentHealth}/{playerOne.MaxHealth}");
    WriteToConsole($"Attack Power: {playerOne.Stats.AttackPower}");
    WriteToConsole($"Defense: {playerOne.Stats.Defense}");
    WaitForInput();
}

void WaitForInput()
{
    Console.WriteLine();
    WriteToConsole("Press any key to continue...", 0, false);
    Console.ReadKey();
}

void ShopScene()
{
    WriteToConsole("You entered the shop.", defaultDelay, true);
    WaitForInput();

    while (true)
    {
        Console.WriteLine();
        WriteToConsole("What would you like to do?", 0, true);
        WriteToConsole("1. View Items", 0, false);
        WriteToConsole("2. Sell Item", 0, false);
        WriteToConsole("3. Exit Shop", 0, false);

        var input = Console.ReadKey();
        switch (input.KeyChar)
        {
            case '1':
                HandleViewItems();
                break;
            case '2':
                HandleSellItems();
                break;
            case '3':
                return;
            default:
                WriteToConsole("Invalid input. Please try again.", 0, false);
                break;
        }
    }
}

void HandleViewItems()
{
    while (true)
    {
        WriteToConsole($"You have {playerOne.Gold.Value} gold.", defaultDelay, true);
        WriteToConsole("Please select an item", 0, true);

        for (int i = 0; i < shop.Items.Count; i++)
        {
            var item = shop.Items[i];
            WriteToConsole($"{i + 1}. {item.Name} - {item.Description} - {item.Amount} gold", 0, false);
        }
        WriteToConsole("0. Go back", 0, false);

        var itemInput = Console.ReadKey();
        if (itemInput.KeyChar == '0') break;

        if (int.TryParse(itemInput.KeyChar.ToString(), out int itemIndex) && itemIndex > 0 && itemIndex <= shop.Items.Count)
        {
            HandleBuyItem(shop.Items[itemIndex - 1]);
        }
        else
        {
            WriteToConsole("Invalid input. Please try again.", 0, false);
        }
    }
}

void HandleBuyItem(IItem selectedItem)
{
    while (true)
    {
        WriteToConsole($"You have {playerOne.Gold.Value} gold.", defaultDelay, true);
        WriteToConsole($"{selectedItem.Name}", 0, false);
        WriteToConsole($"Price: {selectedItem.Amount.Value} gold.", defaultDelay, false);
        Console.WriteLine();
        WriteToConsole("What would you like to do?", 0, false);
        WriteToConsole("1. Inspect Item", 0, false);
        WriteToConsole("2. Buy Item", 0, false);
        WriteToConsole("3. Cancel", 0, false);

        var buyInput = Console.ReadKey();
        switch (buyInput.KeyChar)
        {
            case '1':
                WriteToConsole($"{selectedItem.Description}", defaultDelay, true);
                WaitForInput();
                break;
            case '2':
                ConfirmPurchase(selectedItem);
                return;
            case '3':
                return;
            default:
                WriteToConsole("Invalid input. Please try again.", 0, false);
                break;
        }
    }
}

void ConfirmPurchase(IItem selectedItem)
{
    if (playerOne.Gold >= selectedItem.Amount)
    {
        while (true)
        {
            WriteToConsole($"You have {playerOne.Gold.Value} gold.", defaultDelay, true);
            WriteToConsole($"Are you sure you want to buy {selectedItem.Name} for {selectedItem.Amount} gold?", 0, false);
            WriteToConsole("1. Yes", 0, false);
            WriteToConsole("2. No", 0, false);

            var confirmInput = Console.ReadKey();
            if (confirmInput.KeyChar == '1')
            {
                var item = shop.Buy(selectedItem);
                playerOne.Inventory.AddItem(item);
                WriteToConsole($"You bought {selectedItem.Name}.", defaultDelay, true);
                WaitForInput();
                return;
            }
            else if (confirmInput.KeyChar == '2')
            {
                return;
            }
            else
            {
                WriteToConsole("Invalid input. Please try again.", 0, false);
            }
        }
    }
    else
    {
        WriteToConsole($"You don't have enough gold to buy {selectedItem.Name}.", 0, true);
        WriteToConsole($"You need {selectedItem.Amount.Value - playerOne.Gold.Value} more gold to buy {selectedItem.Name}.", defaultDelay, false);
        WaitForInput();
    }
}

void HandleSellItems()
{
    while (true)
    {
        WriteToConsole("Which Item would you like to sell?", 0, true);

        for (int i = 0; i < playerOne.Inventory.Count; i++)
        {
            var item = playerOne.Inventory[i];
            WriteToConsole($"{i + 1}. {item.Name} - {item.Amount.Value} gold", 0, false);
        }
        WriteToConsole("0. Go back", 0, false);

        var itemInput = Console.ReadKey();
        if (itemInput.KeyChar == '0') break;

        if (int.TryParse(itemInput.KeyChar.ToString(), out int itemIndex) && itemIndex > 0 && itemIndex <= playerOne.Inventory.Count)
        {
            ConfirmSellItem(playerOne.Inventory[itemIndex - 1]);
        }
        else
        {
            WriteToConsole("Invalid input. Please try again.", 0, false);
        }
    }
}

void ConfirmSellItem(IItem selectedItem)
{
    WriteToConsole($"You selected {selectedItem.Name}.", defaultDelay, true);
    WriteToConsole($"You can sell it for {selectedItem.Amount.Value} gold.", defaultDelay, true);
    WriteToConsole("Do you want to sell this item?", 0, false);
    WriteToConsole("1. Yes", 0, false);
    WriteToConsole("2. No", 0, false);

    var confirmInput = Console.ReadKey();
    if (confirmInput.KeyChar == '1')
    {
        playerOne.Inventory.Remove(selectedItem);
        playerOne.AddGold(shop.Sell(selectedItem));
        WriteToConsole($"You sold {selectedItem.Name} for {selectedItem.Amount.Value} gold.", defaultDelay, true);
        WaitForInput();
    }
    else if (confirmInput.KeyChar != '2')
    {
        WriteToConsole("Invalid input. Please try again.", 0, false);
    }
}

MobBase GetRandomOpponent(int startingLevel)
{
    var random = new Random();
    var adjustedLevel = Math.Max(0, startingLevel + random.Next(-1, 2));

    return random.Next(0, 3) switch
    {
        0 => MobTemplates.GetWolf(adjustedLevel),
        1 => MobTemplates.GetBoar(adjustedLevel),
        2 => MobTemplates.GetBear(adjustedLevel),
        _ => throw new InvalidOperationException("Unexpected opponent index.")
    };
}
