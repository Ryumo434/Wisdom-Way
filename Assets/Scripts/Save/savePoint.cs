using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] public GameObject SavePointPanel;

    private void Start()
    {
        // Finde das Root-Objekt "UICanvas"
        GameObject uiCanvas = GameObject.Find("UICanvas");

        if (uiCanvas != null)
        {
            // Suche nach dem Kindobjekt "SavePointPanel" innerhalb von "UICanvas"
            Transform savePointPanelTransform = uiCanvas.transform.Find("SavePointPanel");

            if (savePointPanelTransform != null)
            {
                SavePointPanel = savePointPanelTransform.gameObject;
                DontDestroyOnLoad(SavePointPanel);
            }
            else
            {
                Debug.LogError("SavePointPanel konnte unter UICanvas nicht gefunden werden!");
            }
        }
        else
        {
            Debug.LogError("UICanvas konnte nicht gefunden werden!");
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && SavePointPanel != null)
        {
            SavePointPanel.SetActive(true);
            Debug.Log("Player hat den Collider betreten.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && SavePointPanel != null)
        {
            SavePointPanel.SetActive(false);
            Debug.Log("Player hat den Collider verlassen.");
        }
    }
}
