using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class SaveSystem
{
    public static void SaveData(PlayerStateManager player, int id)
    {
        string FileName = "";
        switch (id)
        {
            case 1:
                FileName = "Apple.json";
                break;
            case 2:
                FileName = "Bannanna.json";
                break;
            case 3:
                FileName = "Cherry.json";
                break;
        }

        //Debug.LogWarning(FileName);
        string path = Application.persistentDataPath + "/" + FileName;
        GameData data = new GameData(player);

        // Serialize the data to JSON using Newtonsoft.Json
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

        // Write JSON data to a file
        File.WriteAllText(path, jsonData);
    }

    public static GameData LoadData(int id)
    {
        string FileName = "";
        switch (id)
        {
            case 1:
                FileName = "Apple.json";
                break;
            case 2:
                FileName = "Bannanna.json";
                break;
            case 3:
                FileName = "Cherry.json";
                break;
        }

        //Debug.LogWarning(FileName);
        string path = Application.persistentDataPath + "/" + FileName;

        if (File.Exists(path))
        {
            // Read JSON data from file
            string jsonData = File.ReadAllText(path);

            // Deserialize JSON data to GameData
            GameData data = JsonConvert.DeserializeObject<GameData>(jsonData);
            return data;
        }
        else
        {
            Debug.LogWarning("File not found: " + path);
            return null;
        }
    }
    public static void NewGameData(int id)
    {
        string FileName = "";
        switch (id)
        {
            case 1:
                FileName = "Apple.json";
                break;
            case 2:
                FileName = "Bannanna.json";
                break;
            case 3:
                FileName = "Cherry.json";
                break;
        }

        string path = Application.persistentDataPath + "/" + FileName;

        // Create a new instance of GameData
        GameData data = new GameData();
        Debug.LogWarning("new:" + data.jump_ability);

        // Serialize the GameData object to JSON using Newtonsoft.Json
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

        // Write the JSON data to a file
        File.WriteAllText(path, jsonData);
    }

    public static void SaveSettings(SettingManager settings)
    {
        string FileName = "Settings.json";
        string path = Application.persistentDataPath + "/" + FileName;
        SettingData data = new SettingData(settings);

        // Serialize the SettingData object to JSON using Newtonsoft.Json
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

        // Write the JSON data to a file
        File.WriteAllText(path, jsonData);
    }

    public static SettingData LoadSettings()
    {
        string FileName = "Settings.json";
        string path = Application.persistentDataPath + "/" + FileName;

        if (File.Exists(path))
        {
            // Read JSON data from file
            string jsonData = File.ReadAllText(path);

            // Deserialize JSON data to SettingData
            SettingData data = JsonConvert.DeserializeObject<SettingData>(jsonData);
            return data;
        }
        else
        {
            // Return a new instance with default settings if the file doesn't exist
            return new SettingData();
        }
    }
}
