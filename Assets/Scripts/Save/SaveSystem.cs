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
        Debug.Log(initialPath);
        if (!File.Exists(initialPath))
        {
            Debug.Log("SaveSystem: InitialSave.json nicht gefunden. Erstelle Standardwerte...");

            string defaultJson = "{\"position\":{\"x\":4.05,\"y\":4.75},\"sceneName\":\"WisWay\",\"score\":0,\"health\":20,\"inventoryItems\":[{\"name\":\"Sword\",\"stackCount\":\"1\"},{\"name\":\"EMPTY_SLOT\",\"stackCount\":\"0\"},{\"name\":\"EMPTY_SLOT\",\"stackCount\":\"0\"},{\"name\":\"EMPTY_SLOT\",\"stackCount\":\"0\"},{\"name\":\"EMPTY_SLOT\",\"stackCount\":\"0\"}]}";

            File.WriteAllText(initialPath, defaultJson);
            Debug.Log("SaveSystem: InitialSave.json wurde mit Standardwerten erstellt.");
        }
        string json = File.ReadAllText(initialPath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        return data;
    }
}

