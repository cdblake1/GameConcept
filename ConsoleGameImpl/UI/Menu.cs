class MenuOption
{
    public IReadOnlyList<TextPacket> Text { get; }
    public ConsoleKey? KeyBind { get; }

    public MenuOption(string text, ConsoleKey? keyBind = null)
    {
        Text = [new(text)];
        KeyBind = keyBind;
    }

    public MenuOption(IReadOnlyList<TextPacket> text, ConsoleKey? keyBind = null)
    {
        Text = text.ToArray();
        KeyBind = keyBind;
    }


}

class Menu
{
    private readonly string? menuHeader;
    private readonly IReadOnlyList<MenuOption> options;
    public bool ClearConsole { get; init; } = true;

    public Menu(string? menuHeader, IReadOnlyList<MenuOption> options)
    {
        this.menuHeader = menuHeader;
        this.options = options;
    }

    public int ShowMenu()
    {
        int selectedIndex = 0;
        ConsoleKey key;

        Console.CursorVisible = false;

        do
        {
            Console.Clear();
            if (menuHeader != null)
            {
                Console.WriteLine(menuHeader);
                Console.WriteLine(new string('-', menuHeader.Length));
            }

            GameTextPrinter.DefaultInstance.Print("Use arrow keys to navigate, Enter to select, number keys or keybinds to select, and Esc to exit.\n");

            for (int i = 0; i < options.Count && i >= 0; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }

                var option = options[i];
                string keybindDisplay = option.KeyBind.HasValue ? $"{option.KeyBind.Value}" : $"{i + 1}";

                GameTextPrinter.DefaultInstance.PrintLine([new($"[{keybindDisplay}]")], false);
                foreach (var textPacket in option.Text)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ResetColor();
                    }
                    GameTextPrinter.DefaultInstance.PrintLine([textPacket], false);
                }

                Console.WriteLine();
                Console.ResetColor();
            }

            var keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            // Check for number key selection
            if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9)
            {
                int num = key - ConsoleKey.D1;
                if (num < options.Count)
                    return num;
            }
            else if (key >= ConsoleKey.NumPad1 && key <= ConsoleKey.NumPad9)
            {
                int num = key - ConsoleKey.NumPad1;
                if (num < options.Count)
                    return num;
            }
            else if (key == ConsoleKey.D0 || key == ConsoleKey.NumPad0)
            {
                return -1;
            }
            else
            {
                // Check for custom keybinds
                for (int i = 0; i < options.Count; i++)
                {
                    if (options[i].KeyBind is ConsoleKey userInputKey && userInputKey == key)
                        return i;
                }
            }

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = options.Count == 0 ? 0 : (selectedIndex - 1 + options.Count) % options.Count;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = options.Count == 0 ? 0 : (selectedIndex + 1) % options.Count;
                    break;
                case ConsoleKey.Enter:
                    return selectedIndex;
                case ConsoleKey.Escape:
                    return -1;
            }
        } while (key != ConsoleKey.Escape);

        return -1;
    }
}