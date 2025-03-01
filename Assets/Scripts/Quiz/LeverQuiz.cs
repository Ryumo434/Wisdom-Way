using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverQuiz : MonoBehaviour
{
    [SerializeField] private LeverChecker checker;
    [SerializeField] private SpriteRenderer leverSprite;
    public Sprite leverUPSprite;
    public Sprite leverDOWNSprite;
    public int arrayIndex;

    private bool isPlayerInTrigger = false;
    private bool isLeverUp = false;

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            toggleLever();
            if (isLeverUp)
            {
                leverSprite.sprite = leverUPSprite;
                checker.leverPositioning[arrayIndex] = 1;
            }
            else
            {
                leverSprite.sprite = leverDOWNSprite;
                checker.leverPositioning[arrayIndex] = 0;
            }
            Debug.Log(checker.leverPositioning);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger enter");
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private void toggleLever()
    {
        isLeverUp = !isLeverUp;
    }
}
