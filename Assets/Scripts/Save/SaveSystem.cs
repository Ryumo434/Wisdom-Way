using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/playerSave.json";
    private static string initialPath = Application.persistentDataPath + "/initialSave.json";


    public static void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
        Debug.Log("SaveSystem: Spielstand gespeichert.");
         Debug.Log($"Json path: {savePath}");
    }

    public static PlayerData Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }
        else
        {
            Debug.LogWarning("SaveSystem: Kein gespeicherter Spielstand gefunden!");
            return null;
        }
    }

    public static PlayerData LoadInitial()
    {
        if (File.Exists(initialPath))
        {
            string json = File.ReadAllText(initialPath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }
        else
        {
            Debug.LogWarning("SaveSystem: Kein gespeicherter Spielstand gefunden!");
            return null;
        }
    }
}
