class TabMenuNavigator
{
    private readonly string menuHeader;
    private readonly List<(string tabName, TabbedMenu menu, Action<int> menuAction)> tabs;

    public TabMenuNavigator(string menuHeader, List<(string tabName, TabbedMenu menu, Action<int> menuAction)> tabs)
    {
        this.menuHeader = menuHeader;
        this.tabs = tabs;
    }


    public void ShowTabbedMenu()
    {
        var selectedTab = 0;
        var position = 0;

        Console.CursorVisible = false;
        do
        {
            Console.Clear();
            Console.WriteLine(menuHeader + "\n");
            position++;
            Console.WriteLine("Use arrow keys to navigate, Enter to select, number keys or keybinds to select, and Esc to exit.\n");
            position++;
            for (int i = 0; i < tabs.Count; i++)
            {
                if (i == selectedTab)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }

                var tab = tabs[i];
                Console.Write($"[{i + 1}] {tab.tabName}");
                Console.ResetColor();

                if (i != tabs.Count - 1)
                {
                    Console.Write("\t");
                    Console.Write(" | ");
                }
                else
                {
                    Console.Write("\t");
                }
            }
            position += 5;

            var selectedMenu = tabs[selectedTab].menu.ShowMenu(position);

            if (selectedMenu == -1)
            {
                return; // Exit
            }
            else if (selectedMenu == -2)
            {
                selectedTab = tabs.Count == 0 ? 0 : (selectedTab - 1 + tabs.Count) % tabs.Count;
            }
            else if (selectedMenu == -3)
            {
                selectedTab = tabs.Count == 0 ? 0 : (selectedTab + 1) % tabs.Count;
            }
            else
            {
                tabs[selectedTab].menuAction(selectedMenu);
            }

            Console.ResetColor();
            Console.Clear();
            position = 0;
        } while (true);
    }

    public class TabbedMenu
    {
        private readonly string menuHeader;
        private readonly IReadOnlyList<MenuOption> options;

        public TabbedMenu(string menuHeader, IReadOnlyList<MenuOption> options)
        {
            this.menuHeader = menuHeader;
            this.options = options;
        }

        private void ClearLine(int position)
        {
            Console.SetCursorPosition(0, position);
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }

        public int ShowMenu(int initialPosition)
        {
            int selectedIndex = 0;
            ConsoleKey key;

            Console.CursorVisible = false;

            do
            {
                var position = initialPosition;
                ClearLine(position);
                Console.SetCursorPosition(0, position);
                Console.WriteLine(menuHeader + "\n");
                position += 2;

                for (int i = 0; i < options.Count; i++)
                {
                    var option = options[i];
                    var text = options[i].Text;
                    position++;
                    ClearLine(position);
                    Console.SetCursorPosition(0, position);
                    if (i == selectedIndex)
                    {
                        text = [.. text.Select(t => t with { BackgroundColor = ConsoleColor.DarkGray })];
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    string keybindDisplay = option.KeyBind.HasValue ? $"{option.KeyBind.Value}" : $"{i + 1}";
                    GameTextPrinter.DefaultInstance.PrintLine([new TextPacket($"[{keybindDisplay}] "), .. text], false, 0);
                    Console.ResetColor();

                    position++;
                }

                var keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = options.Count == 0 ? 0 : (selectedIndex - 1 + options.Count) % options.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = options.Count == 0 ? 0 : (selectedIndex + 1) % options.Count;
                        break;
                    case ConsoleKey.LeftArrow:
                        return -2;
                    case ConsoleKey.RightArrow:
                        return -3;
                    case ConsoleKey.Enter:
                        return selectedIndex;
                    case ConsoleKey.Escape:
                        return -1;
                }
            } while (true);
        }
    }
}