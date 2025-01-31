using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [Header("Drag & Drop der UI aus der jeweiligen Szene")]
    [SerializeField] private GameObject ui;
    [Header("Drag & Drop vom E-Text")]
    [SerializeField] private GameObject eText;

    private static Shop instance; // Singleton-Instanz
    private bool isPlayerInTrigger = false;

    void Awake()
    {
        // Singleton sicherstellen
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AssignReferences();
    }

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
        AssignReferences();
    }

    private void AssignReferences()
    {
        // UI finden, falls Referenz fehlt
        if (ui == null)
            ui = GameObject.Find("UICanvas/Shop Container");

        if (ui == null)
            Debug.LogError($"{name}: Keine UI-Referenz gefunden!");

        if (eText == null)
            Debug.LogError($"{name}: Kein E-Text gefunden!");
        else
            eText.SetActive(false);
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