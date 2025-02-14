using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector2 position;
    public string sceneName;
    public int score;
    public int health;
    public List<string> inventoryItems; // Gespeicherte Items als Liste von Namen

    public PlayerData(Vector2 position, string sceneName, int score, int health, List<string> inventoryItems)
    {
        this.position = position;
        this.sceneName = sceneName;
        this.score = score;
        this.health = health;
        this.inventoryItems = inventoryItems;
        Debug.Log($"Saved Position: {position}, Scene Name: {sceneName}, Csore: {score}, Health: {health}, inventoryItem: {inventoryItems}");
        Debug.Log(string.Join(", ", inventoryItems));

    }
}
