using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private Button LoadButton;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        LoadButton.onClick.AddListener(OnSaveButtonClicked);
    }

    private void OnSaveButtonClicked()
    {
        if (gameManager != null)
        {
            Debug.Log("Load Button wurde gedrückt!");
            gameManager.LoadGame();
        }
        else
        {
            Debug.LogError("GameManager ist nicht zugewiesen!");
        }
    }
}
