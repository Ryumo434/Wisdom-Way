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
    private string[] currentDialogue;

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

    private string[] dialoge2 = new string[]
    {
        "Hallo mein kleiner...",
        "Ich frage mich",
        "wer der Künster",
        "dieses Meisterwekes war",
        "Leonardo da ... war sein Name."
    };

    private string[] dialoge3 = new string[]
    {
        "Du hast die Aufgaben geschafft,",
        "nun wird als letztes",
        "dein schafer Blick geprueft.",
        "Finde das echte Gemaelde",
        "und  entflamme die Fackel.",
        "Viel Erfolg!"
    };

    void Awake()
    { /*
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(PressE);
        DontDestroyOnLoad(Panel);
        DontDestroyOnLoad(darkBackground);
        //DontDestroyOnLoad(ActivateWeapon);
        DontDestroyOnLoad(UICanvas);
        DontDestroyOnLoad(Canvas)*/
    }

    void Update()
    {
        EisPressed();
    }

    private void EisPressed()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInTrigger)
        {
            // Call method to load correct dialogue
            AssignDialogueBasedOnNPC();

            if (!Panel.activeInHierarchy)
            {
                Panel.SetActive(true);
                PressE.SetActive(false);
                ActivateWeapon.SetActive(false);
                UICanvas.SetActive(false);
                darkBackground.SetActive(true);
                NPCText.text = currentDialogue[currentTextIndex];
                Time.timeScale = 0f;
            }
            else
            {
                currentTextIndex++;
                if (currentTextIndex >= currentDialogue.Length)
                {
                    Panel.SetActive(false);
                    currentTextIndex = 0;
                    UICanvas.SetActive(true);
                    darkBackground.SetActive(false);
                    Time.timeScale = 1f;
                }
                else
                {
                    NPCText.text = currentDialogue[currentTextIndex];
                }
            }
        }
    }

    // Method to assign dialogue based on NPC GameObject
    private void AssignDialogueBasedOnNPC()
    {
        // Assuming the NPC GameObject has a unique name or tag
        string npcName = gameObject.name;  // Get the name of the current NPC GameObject

        if (npcName == "NPC1")
        {
            currentDialogue = dialoge2;
        }
        else if (npcName == "NPC2")
        {
            currentDialogue = dialoge3;
        }
        else if (npcName == "NPC")
        {
            currentDialogue = dialoge;
        }
        else
        {
            currentDialogue = dialoge;  // Default dialogue if no specific NPC is matched
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
