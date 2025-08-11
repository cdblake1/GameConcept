#nullable enable

using GameData.src.Player;
using GameLogic.Player;

namespace GameData.Save;

public class SaveManager
{
    private const string SaveFileDirectory = "KhabibGame";
    private readonly string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    internal string GetSaveFilePath(string fileIdentifier)
    {
        // Create a subdirectory for your application
        string saveDirectory = Path.Combine(localAppDataPath, SaveFileDirectory);
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        // Define the save file path
        return Path.Combine(saveDirectory, $"{fileIdentifier}.json");
    }

    public SaveManager(string? directoryOverride = null)
    {
        if (directoryOverride != null)
        {
            localAppDataPath = directoryOverride;
        }
    }

    public string SaveGame(string name, PlayerInstance player)
    {
        // Create a subdirectory for your application
        var saveState = new SaveState
        {
            GameName = name,
            Id = Guid.NewGuid().ToString(),
            // Player = player.Serialize()
        };

        var saveFilePath = GetSaveFilePath(saveState.Id);

        // Serialize the player object to JSON (using System.Text.Json or Newtonsoft.Json)
        string json = System.Text.Json.JsonSerializer.Serialize(saveState);

        // Write the JSON to the save file
        File.WriteAllText(saveFilePath, json);

        Console.WriteLine($"Game saved to {saveFilePath}");

        return saveState.Id;
    }

    public SaveState LoadGame(string Id)
    {
        var file = GetSaveFilePath(Id);
        // Check if the save file exists
        if (File.Exists(file))
        {

            // Deserialize the JSON back into a Player object
            string json = File.ReadAllText(file);

            return System.Text.Json.JsonSerializer.Deserialize<SaveState>(json);
        }
        else
        {
            throw new FileNotFoundException($"Save file {file} not found.");
        }
    }

    public IReadOnlyList<SaveState> LoadAvailableSaveStates()
    {
        var saveDirectory = Path.Combine(localAppDataPath, SaveFileDirectory);
        if (!Directory.Exists(saveDirectory))
        {
            return Array.Empty<SaveState>();
        }

        var saveFiles = Directory.GetFiles(saveDirectory, "*.json");
        List<SaveState>? saves = null;

        foreach (var file in saveFiles)
        {
            saves ??= [];
            string json = File.ReadAllText(file);
            var saveState = System.Text.Json.JsonSerializer.Deserialize<SaveState>(json);

            saves.Add(saveState);
        }

        return saves as IReadOnlyList<SaveState> ?? Array.Empty<SaveState>();
    }
}