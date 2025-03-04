using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private Button LoadButton;
    private string saveFilePath;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "playerSave.json");

        LoadButton.interactable = File.Exists(saveFilePath);
        LoadButton.onClick.AddListener(OnSaveButtonClicked);
    }

    private void OnSaveButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadGame();
        }
        else
        {
            Debug.LogError("GameManager wurde nicht gefunden!");
        }
    }
}



