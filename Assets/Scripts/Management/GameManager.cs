using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private ActiveInventory activeInventory;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            LoadGame();
        }
    }*/

    public void SaveGame()
    {
        activeInventory = ActiveInventory.Instance;

        int slotCount = activeInventory.transform.childCount;
        List<InventoryItem> inventoryItems = new List<InventoryItem>();

        for (int i = 0; i < slotCount; i++)
        {
            Transform slotTransform = activeInventory.transform.GetChild(i);
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();

            if (slot != null)
            {
                string itemName = slot.getItemName();
                string stackCount = slot.getStackCount().text; 

                if (string.IsNullOrEmpty(itemName))
                {
                    Debug.LogWarning($"[SaveGame] Leerer Item-Name im Slot {i}");
                }

                inventoryItems.Add(new InventoryItem(itemName, stackCount));
            }
        }

        Vector2 playerPosition = PlayerController.Instance.transform.position;
        string currentScene = SceneManager.GetActiveScene().name;
        int currentScore = ScoreManager.instance.GetScore();
        int currentHealth = PlayerHealth.Instance.currentHealth;

        PlayerData data = new PlayerData(playerPosition, currentScene, currentScore, currentHealth, inventoryItems);

        SaveSystem.Save(data);
        Debug.Log($"GameManager: Spielstand gespeichert! Inventar: {inventoryItems.Count} Eintr�ge.");
    }

    public void LoadInitialGame()
    {
        Debug.Log("GameManager: LoadInitialGame wurde aufgerufen!");
        PlayerData data = SaveSystem.LoadInitial();

        if (data != null)
        {
            Debug.Log($"GameManager: Geladene Daten - Scene: {data.sceneName}, Inventar: {data.inventoryItems.Count} Eintr�ge");
            foreach (var item in data.inventoryItems)
            {
                Debug.Log($"Item geladen: {item.name}, StackCount: {item.stackCount}");
            }

            StartCoroutine(LoadSceneAndSetPosition(data));
        }
        else
        {
            Debug.LogWarning("GameManager: Kein gespeicherter Spielstand gefunden!");
        }
    }


    public void LoadGame()
    {
        Debug.Log("GameManager: LoadGame wurde aufgerufen!");
        PlayerData data = SaveSystem.Load();

        if (data != null)
        {
            Debug.Log($"GameManager: Geladene Daten - Scene: {data.sceneName}, Inventar: {data.inventoryItems.Count} Eintr�ge");
            foreach (var item in data.inventoryItems)
            {
                Debug.Log($"Item geladen: {item.name}, StackCount: {item.stackCount}");
            }

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
        
        ActiveInventory.Instance.ClearInventory();
        foreach (InventoryItem item in data.inventoryItems)
        {
            ActiveInventory.Instance.LoadInventoryItems(item.name, item.stackCount);
        }
        
        Debug.Log("GameManager: Spielstand geladen!");

        Cinemachine.CinemachineVirtualCamera virtualCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();

        if (virtualCamera != null)
        {
            virtualCamera.Follow = PlayerController.Instance.transform;
        }
    }
}
