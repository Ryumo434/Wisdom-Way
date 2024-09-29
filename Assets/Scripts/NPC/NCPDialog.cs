using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NCPDialog : MonoBehaviour
{
    [SerializeField] private GameObject PressE;
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject darkBackground;
    [SerializeField] private GameObject ActivateWeapon;
    [SerializeField] public TextMeshProUGUI NPCText;
    [SerializeField] public GameObject UICanvas;
    [SerializeField] private GameObject Canvas;

    private bool isPlayerInTrigger = false;
    private int currentTextIndex = 0;
    private string[] dialoge = new string[]
    {
        "Sei gegrüßt, Reisender!",
        "Hier wird dein Blick getestet.",
        "Finde das echte Gemälde",
        "unter den zwei Fälschungen.",
        "Folge dem Pfad des Gemäldes,",
        "um zur nächsten Prüfung zu gelangen.",
        "Viel Erfolg!"
    };

     // Methode, um sicherzustellen, dass das Objekt nicht zerstört wird
    void Awake()
    {
        // Schütze das Hauptobjekt und alle UI-Elemente vor Zerstörung beim Szenenwechsel
        DontDestroyOnLoad(this.gameObject);       // NPC-Dialog-Objekt selbst
        DontDestroyOnLoad(PressE);               // PressE UI-Element
        DontDestroyOnLoad(Panel);                // Panel UI-Element
        DontDestroyOnLoad(darkBackground);       // darkBackground UI-Element
        DontDestroyOnLoad(ActivateWeapon);       // ActivateWeapon UI-Element
        DontDestroyOnLoad(UICanvas);             // UICanvas (kann redundant mit Canvas sein)
        DontDestroyOnLoad(Canvas);               // Das übergeordnete Canvas
    }

    void Update()
    {
        EisPressed();
    }

    private void EisPressed()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInTrigger)
        {
            if (!Panel.activeInHierarchy)
            {
                Panel.SetActive(true);
                PressE.SetActive(false);
                ActivateWeapon.SetActive(false);
                UICanvas.SetActive(false);
                darkBackground.SetActive(true);
                NPCText.text = dialoge[currentTextIndex];
                Time.timeScale = 0f;
            }
            else
            {
                currentTextIndex++;
                if (currentTextIndex >= dialoge.Length)
                {
                    Panel.SetActive(false);
                    currentTextIndex = 0;
                    UICanvas.SetActive(true);
                    darkBackground.SetActive(false);
                    Time.timeScale = 1f;
                }
                else
                {
                    NPCText.text = dialoge[currentTextIndex];
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            PressE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            PressE.SetActive(false);
            Panel.SetActive(false);
            ActivateWeapon.SetActive(true);
            currentTextIndex = 0;
        }
    }
}
