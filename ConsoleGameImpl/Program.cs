using System.Text.RegularExpressions;
using GameDataLayer;

const int defaultDelay = 1000;

bool inCombat;
WriteToConsole("Game Started!");
WriteToConsole(".", 200);
WriteToConsole(".", 200);
WriteToConsole(".", 200);
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

void EnterCombatLoop(CharacterBase opponent)
{
    inCombat = true;
    Console.WriteLine($"Player Encounters {opponent.Name}!", 0, false);
    while (inCombat)
    {
        WriteToConsole($"You are in combat with a {opponent.Name}!", defaultDelay, true);
        WriteToConsole($"""
            Player Stats:
            Health: {playerOne.CurrentHealth}/{playerOne.MaxHealth}

            {opponent.Name} Stats:
            Health: {opponent.CurrentHealth}/{opponent.MaxHealth}
            """);
        WriteToConsole($"""
            What do you want to do?
            1. Attack
            2. Check Inventory
            3. Use Item
            4. Run
            """, 0);
        var input = Console.ReadLine();

        if (input == "1")
        {
            WriteToConsole($"You attack the {opponent.Name}!", 0);
            var damage = playerOne.Attack(opponent);
            WriteToConsole($"You dealt {damage} damage to the {opponent.Name}. {opponent}'s health is now {opponent.CurrentHealth}.");

            if (opponent.CurrentHealth <= 0)
            {
                Console.Clear();
                WriteToConsole($"The {opponent.Name} has been defeated!");
                inCombat = false;
            }
            else
            {
                WriteToConsole($"The {opponent.Name} attacks back!", 0);
                var opponentDamage = opponent.Attack(playerOne);
                WriteToConsole($"The goblin dealt {opponentDamage} damage to you. Your health is now {playerOne.CurrentHealth}.");
                var sleepTask = Task.Delay(3000);
                var inputTask = Task.Run(() => Console.ReadKey(true));
                var completedTask = Task.WhenAny(sleepTask, inputTask).Result;
                if (completedTask == inputTask)
                {
                    // Enter pressed, skip remaining sleep
                }
                Console.Clear();

                if (playerOne.CurrentHealth <= 0)
                {
                    WriteToConsole("You have been defeated!");
                    inCombat = false;
                }
            }
        }
        else if (input == "2")
        {
            WriteToConsole("You ran away from the goblin!");
            inCombat = false;
        }
        else
        {
            WriteToConsole("Invalid input. Please try again.");
        }
    }
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
        });
        WriteToConsole($"Welcome, {name}!");
        return player;
    }
}

void MainOptionsLoop()
{
    WriteToConsole("What would you like to do?", 0, true);
    WriteToConsole("1. Battle! I want loot!", 0, false);
    WriteToConsole("2. Check Inventory", 0, false);
    WriteToConsole("3. Check Stats", 0, false);
    WriteToConsole("4. Exit Game", 0, false);

    var input = Console.ReadLine();
    if (input == "1")
    {
        WriteToConsole("You have chosen to battle!", 0, false);
        EnterCombatLoop(GetRandomOpponent());
    }
    else if (input == "2")
    {
        WriteToConsole("You checked your inventory.", 0, false);
    }
    else if (input == "3")
    {
        WriteToConsole("You checked your stats.", 0, false);
    }
    else if (input == "4")
    {
        WriteToConsole("Exiting game...", 0, false);
        Environment.Exit(0);
    }
    else
    {
        WriteToConsole("Invalid input. Please try again.", 0, false);
        MainOptionsLoop();
    }
}

void CheckInventoryLoop()
{
    WriteToConsole("You checked your inventory.", 1000, false);

    while (true)
    {
        WriteToConsole("What would you like to do?", 0, false);
        WriteToConsole("1. Check Equipment", 0, false);
        WriteToConsole("2. Check Inventory", 0, false);
        WriteToConsole("3. Exit Inventory", 0, false);

        var input = Console.ReadLine();
        if (input == "1")
        {
        }
        else if (input == "2")
        {
            WriteToConsole("You checked your inventory.", 0, false);
        }
        else if (input == "3")
        {
            WriteToConsole("Exiting inventory...", 0, false);
            break;
        }
        else
        {
            WriteToConsole("Invalid input. Please try again.", 0, false);
            CheckInventoryLoop();
        }
    }
}

void CheckEquipment()
{
    WriteToConsole("You checked your equipment.", 0, false);

    var equippedItems = 0;
    foreach (var (slot, item) in playerOne.Equipment)
    {
        if (item is Item equippedItem)
        {
            WriteToConsole($"{slot}: {equippedItem.Name} - {equippedItem.Description}", 0, false);
            equippedItems++;
        }
        else
        {
            WriteToConsole($"{slot}: Empty", 0, false);
        }
    }

    if (equippedItems == 0)
    {
        WriteToConsole("You have no items equipped.", 0, false);
    }

    while (true)
    {
        var items = new List<Item>();

        WriteToConsole("What would you like to do?", 0, true);
        WriteToConsole("1. Equip Item", 0, false);
        WriteToConsole("2. Unequip Item", 0, false);
        WriteToConsole("3. Exit Equipment", 0, false);

        var input2 = Console.ReadLine();
        if (input2 == "1")
        {
            while (true)
            {
                var itemKinds = new List<ItemKind>();
                // Gather equipped slots
                foreach (var (slot, item) in playerOne.Equipment)
                {
                    if (item is Item)
                        itemKinds.Add(slot);
                }

                if (itemKinds.Count == 0)
                {
                    WriteToConsole("You have no items equipped to change.", 1000, true);
                    break;
                }

                WriteToConsole("Enter the number of the slot you want to equip an item to:");
                for (int i = 0; i < itemKinds.Count; i++)
                {
                    WriteToConsole($"{i + 1}. {itemKinds[i]}", 0, false);
                }

                Console.ReadLine();
                var slotInput = Console.ReadLine();

                if (int.TryParse(slotInput, out int slotIndex) && slotIndex > 0 && slotIndex <= itemKinds.Count)
                {
                    var selectedSlot = itemKinds[slotIndex - 1];
                    WriteToConsole($"You selected {selectedSlot}.", 1000, true);
                    // Logic to equip an item
                    break;
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
            WriteToConsole("You unequipped an item.", 0, false);
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



CharacterBase GetRandomOpponent()
{
    var random = new Random();
    // 0 = boar, 1 = goblin (you can add bear if you have it)
    int choice = random.Next(2);
    if (choice == 0)
        return new CharacterBase("Boar", new StatTemplate
        {
            AttackPower = 20,
            Defense = 10,
            Health = 80
        });
    else
        return new CharacterBase("Goblin", new StatTemplate
        {
            AttackPower = 10,
            Defense = 0,
            Health = 50
        }); ;
}