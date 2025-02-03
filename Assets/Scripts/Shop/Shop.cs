using UnityEngine;
using UnityEngine.SceneManagement;


// FALLS PERFORMANCE PROBLEME DEN ANSATZ MIT "FindObjectsOfTypeAll" IST SEHR RECHENINTENSIV!!!!

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject eText;

    private bool isPlayerInTrigger = false;

    private void OnEnable()
    {
        // Registriere den Callback, damit OnSceneLoaded immer aufgerufen wird, wenn eine neue Szene geladen wird
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Vergiss nicht, den Callback wieder zu deregistrieren!
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Diese Methode wird jedes Mal aufgerufen, wenn eine neue Szene geladen wird
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetUIReference();
    }

    // Methode, um die UI-Referenz zu aktualisieren
    private void SetUIReference()
    {
        // Versuch zuerst den Standardweg:
        ui = GameObject.FindWithTag("ShopUI");

        if (ui == null)
        {
            Debug.Log("ShopUI nicht gefunden via FindWithTag, versuche alternative Suche...");

            // Suche auch in inaktiven Objekten:
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject go in allObjects)
            {
                if (go.CompareTag("ShopUI"))
                {
                    ui = go;
                    break;
                }
            }
        }

        if (ui != null)
        {
            ui.SetActive(false);
            Debug.Log("ShopUI wurde neu referenziert.");
        }
        else
        {
            Debug.Log("ShopUI konnte nicht gefunden werden.");
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && ui != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
                ui.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Escape))
                ui.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            if (eText != null)
                eText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            if (ui != null)
                ui.SetActive(false);
            if (eText != null)
                eText.SetActive(false);
        }
    }
}