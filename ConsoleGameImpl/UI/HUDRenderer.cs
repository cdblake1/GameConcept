public static class HUDRenderer
{
    public static void DrawBar(int x, int y, string label, int value, int max, int width, ConsoleColor fillColor, ConsoleColor emptyColor)
    {
        Console.SetCursorPosition(x, y);
        Console.Write($"{label}: ");

        int barStart = Console.CursorLeft;
        int fillLength = (int)((value / (double)max) * width);
        int emptyLength = width - fillLength;

        Console.ForegroundColor = fillColor;
        Console.Write(new string('█', fillLength));

        Console.ForegroundColor = emptyColor;
        Console.Write(new string('░', emptyLength));

        Console.ResetColor();
        Console.Write($"  {value} / {max}");
    }

    public static void DrawHUD(string name, int level, int hp, int maxHp, int mp, int maxMp, int atk, int def, int top = 0, int left = 0)
    {

        Console.SetCursorPosition(left, top);
        Console.Write("╔" + new string('═', 30 - 2) + "╗");

        Console.SetCursorPosition(left, top + 1);
        Console.Write($"║ Name: {name,-10} Lv: {level,-3}{"",30 - 21}║");

        Console.SetCursorPosition(left, top + 2);
        Console.Write("║ ");
        HUDRenderer.DrawBar(left + 2, top + 2, "HP", hp, maxHp, 12, ConsoleColor.Red, ConsoleColor.DarkGray);
        Console.Write(" ║");

        Console.SetCursorPosition(left, top + 3);
        Console.Write("║ ");
        HUDRenderer.DrawBar(left + 2, top + 3, "MP", mp, maxMp, 12, ConsoleColor.Cyan, ConsoleColor.DarkGray);
        Console.Write(" ║");

        Console.SetCursorPosition(left, top + 4);
        Console.Write($"║ ATK: {atk,-5} DEF: {def,-5}{"",30 - 21}║");

        Console.SetCursorPosition(left, top + 5);
        Console.Write("╚" + new string('═', 30 - 2) + "╝");
    }

}
