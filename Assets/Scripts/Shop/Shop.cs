using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Drag & Drop der UI aus der jeweiligen Szene")]
    [SerializeField] private GameObject ui;      // "Shop Container"
    [Header("Drag & Drop vom E-Text")]
    [SerializeField] private GameObject eText;   // z. B. "Press E"

    private bool isPlayerInTrigger = false;

    private void Start()
    {
        ui = GameObject.Find("UICanvas/Shop Container");
        // Kleiner Sicherheitscheck
        if (ui == null)
            Debug.LogError($"{name}: Keine UI-Referenz gesetzt!");
        if (eText == null)
            Debug.LogError($"{name}: Kein E-Text gesetzt!");
        else
            eText.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInTrigger && ui != null)
        {
            // Mit "GetKeyDown" öffnen/schließen wir das UI einmalig
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