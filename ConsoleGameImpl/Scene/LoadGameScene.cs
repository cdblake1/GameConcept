#nullable enable

using GameData;
using GameData.Save;

public class LoadGameScene
{
    public static void Show()
    {
        var saveManager = new SaveManager();
        var textPrinter = new GameTextPrinter();

        var saveStates = saveManager.LoadAvailableSaveStates();

        if (saveStates.Count == 0)
        {
            textPrinter.Print("No saves found.");
            textPrinter.WaitForInput();
            return;
        }
        var saveStateList = new List<MenuOption>();
        foreach (var saveState in saveStates)
        {
            saveStateList.Add(new MenuOption(saveState.GameName));
        }

        while (true)
        {
            var menu = new Menu("Load Game", saveStateList);
            var selectedOption = menu.ShowMenu();

            if (selectedOption == -1)
            {
                return;
            }

            if (selectedOption >= 0 && selectedOption < saveStates.Count)
            {
                var selectedSave = saveStates[selectedOption];
                GlobalGameState.Instance.Player = Player.Restore(selectedSave.Player);
                textPrinter.Print($"Loaded game: {selectedSave.GameName}");
                textPrinter.WaitForInput();
                break;
            }
            else
            {
                break;
            }
        }
    }
}