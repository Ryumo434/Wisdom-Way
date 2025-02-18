using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public PlayerControls playerControls;

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJson;

    private bool playerInRange;


    private void Update()
    {
        if (playerInRange)
        {
             visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log(inkJson.text);
            }

        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerInRange = false;
        visualCue.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
