using UnityEngine;
using UnityEngine.SceneManagement;


// FALLS PERFORMANCE PROBLEME: DER ANSATZ MIT "FindObjectsOfTypeAll" IST SEHR RECHENINTENSIV!!!!

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject eText;

    private bool isPlayerInTrigger = false;
    private bool isShopActive = true;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetUIReference();
    }

    private void SetUIReference()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.CompareTag("ShopUI"))
            {
                ui = go;
                Debug.Log("ShopUI gesetzt");
                break;
            }
        }


        if (ui != null)
        {
            ui.SetActive(false);
            isShopActive = false;
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
                ToggleShop();
        }
    }

    private void ToggleShop()
    {
        isShopActive = !isShopActive;
        if (isShopActive)
        {
            ui.SetActive(false);
        }
        else
        {
            ui.SetActive(true);
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