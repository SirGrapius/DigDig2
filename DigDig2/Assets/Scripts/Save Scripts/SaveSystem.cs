using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static SaveData saveData = new SaveData();
    private static SettingsData settingsData = new SettingsData();

    [System.Serializable] public struct SaveData
    {
        public PlayerSaveData PlayerData;
        public RoundData RoundData;
    }

    [System.Serializable] public struct SettingsData 
    {
        public PermRoundData PermRoundData;
    }

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "save" + ".save";
        return saveFile;
    }
    public static string SettingsDataFileName()
    {
        string settingsFile = Application.persistentDataPath + "settings" + ".save";
        return settingsFile;
    }

    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true));
        File.WriteAllText(SettingsDataFileName(), JsonUtility.ToJson(settingsData, true));
        Debug.Log("saving");
    }

    private static void HandleSaveData()
    {
        GameStateManager.Instance.Player.Save(ref saveData.PlayerData);
        GameStateManager.Instance.RoundManager.Save(ref saveData.RoundData, settingsData.PermRoundData);
    }

    public static void ClearData()
    {
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(null, true));
    }

    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());
        string settingsContent = File.ReadAllText(SettingsDataFileName());

        saveData = JsonUtility.FromJson<SaveData>(saveContent);
        settingsData = JsonUtility.FromJson<SettingsData>(settingsContent);
        HandleLoadData();
    }

    private static void HandleLoadData()
    {
        GameStateManager.Instance.Player.Load(saveData.PlayerData);
        GameStateManager.Instance.RoundManager.Load(saveData.RoundData, settingsData.PermRoundData);
    }
}
