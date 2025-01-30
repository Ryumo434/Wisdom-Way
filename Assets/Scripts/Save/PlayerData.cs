using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector2 position;
    public string sceneName;
    public int score;
    public int health;

    public PlayerData(Vector2 position, string sceneName, int score, int health)
    {
        this.position = position;
        this.sceneName = sceneName;
        this.score = score;
        this.health = health;
        Debug.Log($"Saved Position: {position}, Scene Name: {sceneName}, Csore: {score}, Health: {health}");
    }
}
