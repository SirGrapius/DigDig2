using System.IO;
using UnityEngine;

public class SaveSystem
{

    private static SaveData saveData = new SaveData();

    [System.Serializable] public struct SaveData
    {
        public PlayerSaveData PlayerData;
        public RoundData RoundData;
    }

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "save" + ".save";
        return saveFile;
    }

    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true));
        Debug.Log("saving");
    }

    private static void HandleSaveData()
    {
        GameStateManager.Instance.Player.Save(ref saveData.PlayerData);
        GameStateManager.Instance.RoundManager.Save(ref saveData.RoundData);
    }

    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());

        saveData = JsonUtility.FromJson<SaveData>(saveContent);
        HandleLoadData();
    }

    private static void HandleLoadData()
    {
        GameStateManager.Instance.Player.Load(saveData.PlayerData);
        GameStateManager.Instance.RoundManager.Load(saveData.RoundData);
    }
}
