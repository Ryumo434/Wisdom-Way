using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private Button LoadButton;
    //[SerializeField] private GameManager gameManager;

    GameManager gameManager;
   // [SerializeField] private GameObject SpawnPoint;

    void Start()
    {
        //gameManager = GetComponent<GameManager>();
        LoadButton.onClick.AddListener(OnSaveButtonClicked);
    }

    private void OnSaveButtonClicked()
    {
    /*    SavePointPanel.SetActive(false);
        if (SpawnPoint != null)
        {
            SpawnPoint.SetActive(false);
            Debug.Log("SpawnPoint wurde deaktiviert.");
        }
        else
        {
            Debug.LogError("SpawnPoint wurde nicht gefunden!");
        }
        Debug.Log("Save Button wurde gedrückt!");  */
        
        //dieses if statemant ist unnnötig //Nasser
        if (gameManager != null)
        {
            //gameManager.LoadGame();
            GameManager.Instance.LoadGame();
        }
        else
        {
            
           // Debug.LogError("GameManager wurde nicht gefunden!");
        }


        GameManager.Instance.LoadGame();
    }
}
