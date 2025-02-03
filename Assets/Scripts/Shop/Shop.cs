using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [Header("Drag & Drop der UI aus der jeweiligen Szene")]
    [SerializeField] private GameObject ui;
    [Header("Drag & Drop vom E-Text")]
    [SerializeField] private GameObject eText;

    private bool isPlayerInTrigger = false;

    private void Start()
    {
        ui = GameObject.FindWithTag("ShopUI");
        if (ui == null)
        {
            Debug.Log("immernoch null...");
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