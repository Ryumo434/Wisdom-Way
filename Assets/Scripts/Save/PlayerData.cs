using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector2 position;
    public string sceneName;
    public int score;
    public int health;
    public List<InventoryItem> inventoryItems;

    public PlayerData(Vector2 position, string sceneName, int score, int health, List<InventoryItem> inventoryItems)
    {
        this.position = position;
        this.sceneName = sceneName;
        this.score = score;
        this.health = health;
        this.inventoryItems = inventoryItems;
        
        Debug.Log($"Saved Position: {position}, Scene Name: {sceneName}, Score: {score}, Health: {health}");
        foreach (var item in inventoryItems)
        {
            Debug.Log($"Item: {item.name}, StackCount: {item.stackCount}");
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public string name;
    public string stackCount;
    
    public InventoryItem(string name, string stackCount)
    {
        this.name = name;
        this.stackCount = stackCount;
    }
}
