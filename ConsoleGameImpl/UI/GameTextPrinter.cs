
using System.Reflection;
using GameData;

public readonly struct TextPacket
{
    public string Text { get; }
    public ConsoleColor Color { get; init; }
    public ConsoleColor? BackgroundColor { get; init; }

    public TextPacket(string text, ConsoleColor? color = null, ConsoleColor? backgroundColor = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be null or whitespace.", nameof(text));

        Color = color ?? ConsoleColor.White;
        BackgroundColor = backgroundColor;
        Text = text;
    }
}

public class GameTextPrinter
{
    public int LineDelay { get; set; } = 1000;          // Delay between lines in ms
    public int TypingDelay { get; set; } = 40;          // Delay per character in ms
    public bool EnableTypingEffect { get; set; } = false;
    public bool AllowSkip { get; set; } = true;

    public static GameTextPrinter DefaultInstance => defaultInstance;
    private static readonly GameTextPrinter defaultInstance = new();

    public void PrintLine(IReadOnlyList<TextPacket> packet, bool newLine = true, int? delay = null)
    {
        foreach (var textPacket in packet)
        {
            Console.ForegroundColor = textPacket.Color;
            if (textPacket.BackgroundColor is ConsoleColor bgColor)
            {
                Console.BackgroundColor = bgColor;
            }

            if (EnableTypingEffect)
            {
                foreach (char c in textPacket.Text)
                {
                    Console.Write(c);
                    if (AllowSkip && Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                        Console.Write(textPacket.Text.Substring(textPacket.Text.IndexOf(c)));
                        break;
                    }
                    Thread.Sleep(delay ?? TypingDelay);
                }
            }
            else
            {
                Console.Write(textPacket.Text);
            }

            Console.ResetColor();
        }

        if (newLine)
        {
            Console.WriteLine();
        }
    }

    public void Print(TextPacket packet)
    {
        PrintLine([packet], true);
    }

    public void Print(string text, ConsoleColor? color = null)
    {
        Print(new TextPacket(text, color));
    }

    public void Print(IReadOnlyList<TextPacket> packets)
    {
        PrintLine(packets, true);
        WaitOrSkip(LineDelay);
    }

    public void WaitForInput()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
        Console.Clear();
    }

    private void WaitOrSkip(int delay)
    {
        if (!AllowSkip)
        {
            Thread.Sleep(delay);
            return;
        }

        for (int i = 0; i < delay / 100; i++)
        {
            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // clear input
                break;
            }
            Thread.Sleep(100);
        }
    }

    public void NotImplementedText(string text)
    {
        Console.Clear();
        Console.WriteLine($"[Not Implemented] {text}");
        this.WaitForInput();
    }

    public static ConsoleColor GetColor(TextKind kind)
    {
        return kind switch
        {
            TextKind.EasyMob => ConsoleColor.Green,         // Weak mob, safe/low threat
            TextKind.Mob => ConsoleColor.DarkYellow,         // Neutral/normal encounter
            TextKind.HardMob => ConsoleColor.Red,           // Strong enemy = danger
            TextKind.Health => ConsoleColor.DarkGreen,     // HP = vitality
            TextKind.Damage => ConsoleColor.DarkRed,       // Indicates damage output or taken
            TextKind.StatusEffect => ConsoleColor.Cyan,     // Status effects = blue/cool tone
            TextKind.LevelUp => ConsoleColor.Magenta,        // Level up = vibrant/powerful
            TextKind.Gold => ConsoleColor.Yellow,        // Classic for currency
            TextKind.Experience => ConsoleColor.Yellow,     // XP = yellow/gold
            TextKind.LootNormal => ConsoleColor.DarkGray,          // Common loot = basic
            TextKind.LootRare => ConsoleColor.DarkBlue,          // Rare = icy/cool tone
            TextKind.LootEpic => ConsoleColor.DarkMagenta,       // Epic = vibrant/powerful

            _ => ConsoleColor.White
        };
    }

    public static TextPacket GetItemText(IItem item)
    {
        var color = item.Rarity switch
        {
            ItemRarity.Common => ConsoleColor.DarkGray,
            ItemRarity.Uncommon => ConsoleColor.DarkGreen,
            ItemRarity.Rare => ConsoleColor.DarkBlue,
            ItemRarity.Epic => ConsoleColor.DarkMagenta,
            _ => ConsoleColor.White
        };

        return new TextPacket(item.Name, color);
    }
}

public enum TextKind
{
    EasyMob,
    Mob,
    HardMob,
    Health,
    Damage,
    Gold,
    Experience,
    LevelUp,
    LootNormal,
    LootRare,
    LootEpic,
    StatusEffect,
}