public class DialogueQueue
{
    private static readonly GameTextPrinter textPrinter = new GameTextPrinter();

    public static void AddDialogue(IReadOnlyList<string> dialogue)
    {
        Console.Clear();

        foreach (var line in dialogue)
        {
            textPrinter.Print(line);

            textPrinter.WaitForInput();
        }
    }

    public static void AddDialogue(IReadOnlyList<IReadOnlyList<TextPacket>> dialogue)
    {
        Console.Clear();

        foreach (var line in dialogue)
        {
            textPrinter.Print(line);
            textPrinter.WaitForInput();
        }
    }
}