using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private GameObject SavePointPanel;
    [SerializeField] private Button SaveButton;  
    [SerializeField] private GameObject SpawnPoint;

   private void Start()
    {
        // Finde das Root-Objekt "UICanvas"
        GameObject uiCanvas = GameObject.Find("UICanvas");
        GameObject SpawnPoint = GameObject.Find("SpawnPoint");

        if (uiCanvas != null)
        {
            // Suche nach dem Kindobjekt "SavePointPanel" innerhalb von "UICanvas"
            Transform savePointPanelTransform = uiCanvas.transform.Find("SavePointPanel");

            if (savePointPanelTransform != null)
            {
                SavePointPanel = savePointPanelTransform.gameObject;
                DontDestroyOnLoad(SavePointPanel);

                // Finde den SaveButton und füge die Click-Event-Listener hinzu
                Transform backgroundTransform = savePointPanelTransform.Find("Background");
                if (backgroundTransform != null)
                {
                    Transform saveButtonTransform = backgroundTransform.Find("SaveButton");
                    if (saveButtonTransform != null)
                    {
                        SaveButton = saveButtonTransform.GetComponent<Button>();
                        if (SaveButton != null)
                        {
                            SaveButton.onClick.AddListener(OnSaveButtonClicked);
                            Debug.Log("SaveButton wurde erfolgreich gefunden und Listener hinzugefügt.");
                        }
                        else
                        {
                            Debug.LogError("SaveButton-Komponente wurde nicht gefunden!");
                        }
                    }
                    else
                    {
                        Debug.LogError("SaveButton unter Background nicht gefunden!");
                    }
                }
                else
                {
                    Debug.LogError("Background unter SavePointPanel nicht gefunden!");
                }
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

    private void OnSaveButtonClicked()
    {
        SavePointPanel.SetActive(false);
        SpawnPoint.SetActive(false);
        Debug.Log("Save Button wurde gedrückt!");
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
