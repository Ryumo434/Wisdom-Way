using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJson;

    private bool playerInRange = false;

    private void Awake()
    {
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange || DialogueManager.Instance.dialogueIsPlaying)
        {
            visualCue.SetActive(false);
            return;
        }

        visualCue.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.Instance.EnterDialogueMode(inkJson);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
