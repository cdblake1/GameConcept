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
        Console.CursorVisible = false;
        do
        {
            Console.Clear();
            Console.WriteLine(menuHeader + "\n");
            Console.WriteLine("Use arrow keys to navigate, Enter to select, number keys or keybinds to select, and Esc to exit.\n");

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
                if (i != tabs.Count - 1)
                {
                    Console.Write("\t");
                    Console.Write(" | ");
                }

                Console.ResetColor();
            }

            Console.WriteLine("\n\n");

            var selectedMenu = tabs[selectedTab].menu.ShowMenu();

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

        public int ShowMenu()
        {
            int selectedIndex = 0;
            ConsoleKey key;

            Console.CursorVisible = false;

            do
            {
                Console.WriteLine(menuHeader + "\n");
                Console.WriteLine("Use arrow keys to navigate, Enter to select, number keys or keybinds to select, and Esc to exit.\n");

                for (int i = 0; i < options.Count; i++)
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
                    Console.WriteLine($"[{keybindDisplay}] {option.Text}");
                    Console.ResetColor();
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