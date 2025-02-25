using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        Vector2 playerPosition = PlayerController.Instance.transform.position;
        string currentScene = SceneManager.GetActiveScene().name;
        int currentScore = ScoreManager.instance.GetScore();
        int currentHealth = PlayerHealth.Instance.currentHealth;
        List<string> inventoryItems = ActiveInventory.Instance.GetInventoryItems(); // Items holen

        PlayerData data = new PlayerData(playerPosition, currentScene, currentScore, currentHealth, inventoryItems);
        SaveSystem.Save(data);
        Debug.Log("GameManager: Spielstand gespeichert!");
    }

    public void LoadGame()
    {
        Debug.Log("GameManager: LoadGame wurde aufgerufen!");
        PlayerData data = SaveSystem.Load();

        if (data != null)
        {
            StartCoroutine(LoadSceneAndSetPosition(data));
        }
        else
        {
            Debug.LogWarning("GameManager: Kein gespeicherter Spielstand gefunden!");
        }
    }

    private System.Collections.IEnumerator LoadSceneAndSetPosition(PlayerData data)
    {
        Debug.Log("GameManager: LoadSceneAndSetPosition gestartet!");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(data.sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        PlayerController.Instance.transform.position = data.position;
        ScoreManager.instance.SetScore(data.score);
        PlayerHealth.Instance.currentHealth = data.health;
        PlayerHealth.Instance.UpdateHealthUI();
        ActiveInventory.Instance.LoadInventoryItems(data.inventoryItems); // Items ins Inventar laden
        
        Debug.Log("GameManger: Spielstand geladen!");

        // Stelle sicher, dass die Cinemachine-Kamera nach dem Laden der Szene folgt
        Cinemachine.CinemachineVirtualCamera virtualCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();

        if (virtualCamera != null)
        {
            virtualCamera.Follow = PlayerController.Instance.transform;  // Weist die Kamera dem Spieler zu
            //virtualCamera.LookAt = PlayerController.Instance.transform;   // Kamera schaut auch auf den Spieler/ alles dreht sich um den Spieler
        }
    }
}
