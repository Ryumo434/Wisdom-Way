using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.SearchService;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f; //je kleiner die Zahl ist destu Schneller die Geschwindigkeit da es sich hier um den zeitlichen Abstand zwischen den Buchstaben handelt


    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;

    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject ActiveWeapon;
    [SerializeField] private GameObject darkBackground;

    [SerializeField] private GameObject pauseMenuCanvas;
   //public GameObject pauseMenu; // Dein Pausemenü-Canvas

    

    private Animator layoutAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private int selectedChoiceIndex = 0;

    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;

    private bool canSkip;

    private bool submitSkip;

    private Coroutine displayLineCoroutine;

    public static DialogueManager Instance { get; private set; }

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private void Awake()
    {

        UICanvas = GameObject.FindWithTag("UICanvas");
        ActiveWeapon = GameObject.FindWithTag("ActiveWeapon");
        pauseMenuCanvas = GameObject.FindWithTag("PauseMenuCanvas");


        if (Instance != null)
        {
            Debug.LogWarning("Mehr als ein DialogueManager in der Szene gefunden!");
            return;
        }
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        // Initialisiere das Array für die Wahltexte
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            submitSkip = true;
        }

        if (!dialogueIsPlaying) { return; }

        if (canContinueToNextLine == true && currentStory.currentChoices.Count == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ContinueStory();
            }
        }
        else
        {
            HandleChoiceNavigation();
        }
    }

    private void HandleChoiceNavigation()
    {
        int choiceCount = currentStory.currentChoices.Count;

        if (choiceCount > 0)
        {
            // Navigation mit den Pfeiltasten
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                selectedChoiceIndex = (selectedChoiceIndex - 1 + choiceCount) % choiceCount;
                UpdateChoiceUI();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                selectedChoiceIndex = (selectedChoiceIndex + 1) % choiceCount;
                UpdateChoiceUI();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                MakeChoice(selectedChoiceIndex);
            }
        }
    }

    private void UpdateChoiceUI()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i].color = (i == selectedChoiceIndex) ? Color.yellow : Color.white;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {

        darkBackground.SetActive(true);
        UICanvas.SetActive(false);
        ActiveWeapon.SetActive(false);

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //setzt Portrait, layout und Sprecher zurück (Damit die Tagvalues nicht aus vergangenen Gespräche genommen werden.
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("right");



        ContinueStory();


    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        darkBackground.SetActive(false);
        UICanvas.SetActive(true);
        ActiveWeapon.SetActive(true);

        
        pauseMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //Sollte displaycotoutine noch instanziert sein wird der Coroutine Gestoppt; So wird verhindert das sich alte dialogcoroutine und neue üvberschneiden wenn zu schnell geskiptt wird
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));


            HandleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }



    private IEnumerator DisplayLine(string line)
    {


        //DialogText leeren
        dialogueText.text = "";

        //Objeckte Verstecken bis der Text zuende geschrieben wurde
        continueIcon.SetActive(false);
        HideChoices();

        submitSkip = false;
        canContinueToNextLine = false;

        StartCoroutine(CanSkip());

        //display ech letter one at a time
        foreach (char letter in line.ToCharArray())
        {

            //Wenn der Spieler auf die Leertaste drückt während dem Typwriting effekt wird sofort die ganze Zeile ausgegeben
            if (canSkip && submitSkip)
            {
                submitSkip = false;
                dialogueText.text = line;
                break;
            }

            //hinzufügen eines einzelnen Buchstaben zum gesamtdialog (so wird buchstabenach buchstabe hinzugefügt)
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);

        }

        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
        canSkip = false;
    }


    private IEnumerator CanSkip()
    {
        canSkip = false;
        yield return new WaitForSeconds(0.05f);
        canSkip = true;
    }


    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }


    private void HandleTags(List<string> currentTags)
    {

        foreach (string tag in currentTags)
        {
            //parse the tag
            string[] splitTag = tag.Split(':');

            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed : " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //handle the tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    Debug.Log("DisplayNameText=" + tagValue);
                    break;
                case PORTRAIT_TAG:
                    //"Play" ist in der Lage eine beliebige Animation abzuspielen
                    portraitAnimator.Play(tagValue);
                    Debug.Log("portrait=" + tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    Debug.Log("layout=" + tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but its not currently being handled: " + tag);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        selectedChoiceIndex = 0; // Setze Auswahl zurück

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError($"Zu viele Wahlmöglichkeiten! Maximal {choices.Length}, aber {currentChoices.Count} gegeben.");
            return;
        }


        // Aktiviere und initialisiere nur die benötigten Optionen
        for (int i = 0; i < choices.Length; i++)
        {
            if (i < currentChoices.Count)
            {
                choices[i].SetActive(true);
                choicesText[i].text = currentChoices[i].text;
            }
            else
            {
                choices[i].SetActive(false);
            }
        }

        UpdateChoiceUI();
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            Debug.Log($"Gewählte Wahl: {choiceIndex}");
            currentStory.ChooseChoiceIndex(choiceIndex);

            ContinueStory();
        }


    }

    public DialogueManager GetInstance() {

        return Instance;
    }
}