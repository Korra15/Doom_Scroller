using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
public class SaveManager : MonoBehaviour
{
    public PlayerStateManager player;
    public SettingManager setting;

    public int slot = 0;

    public bool FileExists(int id){
        string FileName = "";
        switch(id){
            case 1:
                FileName = "Apple.json";
                break;
            case 2:
                FileName = "Bannanna.json";
                break;
            case 3:
                FileName = "Cherry.json";
                break;
            case 4:
                FileName = "Slot.json";
                break;
        }
        string path = Application.persistentDataPath + "/" + FileName;
        return File.Exists(path);
    }

    public static void DeleteSave(int id){
        string FileName = "";
        switch(id){
            case 1:
                FileName = "Apple.json";
                break;
            case 2:
                FileName = "Bannanna.json";
                break;
            case 3:
                FileName = "Cherry.json";
                break;
            case 4:
                FileName = "Slot.json";
                break;
        }
        string path = Application.persistentDataPath + "/" + FileName;
        File.Delete(path);
    }

    public void InstantLoad(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Scene Loader
    public void LoadSave(int id){
        slot = id;
        this.SaveSlot();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Save Functions
    public void NewSave(int id){
        SaveSystem.NewGameData(id);
    }

    public void Save(){
        slot = this.LoadSlot();
        SaveSystem.SaveData(player, slot);
    }

    public GameData LoadPlayerData(){
        slot = this.LoadSlot();
        return SaveSystem.LoadData(slot);
    }

    // Settings Functions
    public void saveSettings(){
        SaveSystem.SaveSettings(setting);
    }

    public SettingData LoadSettings(){
        return SaveSystem.LoadSettings();
    }

    private void SaveSlot()
    {
        string FileName = "Slot.json";
        string path = Application.persistentDataPath + "/" + FileName;
        int id = slot;

        // Serialize the integer id to JSON
        string jsonData = JsonConvert.SerializeObject(id, Formatting.Indented);

        // Write the JSON data to a file
        File.WriteAllText(path, jsonData);
    }

    private int LoadSlot()
    {
        string FileName = "Slot.json";
        string path = Application.persistentDataPath + "/" + FileName;

        if (File.Exists(path))
        {
            // Read JSON data from file
            string jsonData = File.ReadAllText(path);

            // Deserialize JSON data to integer
            int id = JsonConvert.DeserializeObject<int>(jsonData);
            return id;
        }
        else
        {
            // Return a default value or handle the missing file case appropriately
            // Debug.LogWarning("Slot file not found: " + path);
            return -1; // or some other default value
        }
    }
}
