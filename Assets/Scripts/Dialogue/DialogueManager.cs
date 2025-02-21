using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.SearchService;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private int selectedChoiceIndex = 0;

    public bool dialogueIsPlaying { get; private set; }
    public static DialogueManager Instance { get; private set; }

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Mehr als ein DialogueManager in der Szene gefunden!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // Initialisiere das Array für die Wahltexte
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
            return;

        if (currentStory.currentChoices.Count == 0)
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

    private void UpdateChoiceUI()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i].color = (i == selectedChoiceIndex) ? Color.yellow : Color.white;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();

            HandleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void HandleTags(List<string> currentTags)
    {

        foreach (string tag in currentTags)
        {
            //parse the tag
            string[] splitTag = tag.Split(':');

            if(splitTag.Length != 2 )
            {
                Debug.LogError("Tag could not be appropriately parsed : "+ tag);
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
                    Debug.Log("portrait=" + tagValue);
                    break;
                case LAYOUT_TAG:
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
        Debug.Log($"Gewählte Wahl: {choiceIndex}");
        currentStory.ChooseChoiceIndex(choiceIndex);
        PlayerController.GetInstance().RegisterSubmitPressed();
        ContinueStory();
    }
}
