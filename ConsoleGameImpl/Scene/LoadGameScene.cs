#nullable enable

using ConsoleGameImpl.State;
using GameData.Save;
using GameData.src.Player;
using GameLogic.Player;

public class LoadGameScene
{
    public static bool Show()
    {
        var saveManager = new SaveManager();
        var textPrinter = new GameTextPrinter();

        var saveStates = saveManager.LoadAvailableSaveStates();

        if (saveStates.Count == 0)
        {
            DialogueQueue.AddDialogue(["No saves found."]);
            return false;
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
                return false;
            }

            if (selectedOption >= 0 && selectedOption < saveStates.Count)
            {
                var selectedSave = saveStates[selectedOption];
                GlobalGameState.Instance.Player = new PlayerInstance("Player", selectedSave.Player);
                textPrinter.Print($"Loaded game: {selectedSave.GameName}");
                textPrinter.WaitForInput();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}