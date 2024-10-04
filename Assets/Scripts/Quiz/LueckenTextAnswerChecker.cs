using UnityEngine;

public class LueckenTextAnswerChecker : MonoBehaviour
{
    [SerializeField] private GameObject quizPanel;
    [SerializeField] private WallController wallController;



    public void CheckAnswers()
    {
        bool allAnswersCorrect = true;

        // Finde alle DropZones in der Szene
        DropZone[] dropZones = FindObjectsOfType<DropZone>();

        foreach (DropZone dropZone in dropZones)
        {
            if (dropZone.AssignedDraggable == null)
            {
                // Eine DropZone ist leer
                allAnswersCorrect = false;
                Debug.Log("Eine DropZone ist leer.");
                break;
            }
            else if (dropZone.AssignedDraggable.gameObject.tag != dropZone.erwarteterTag)
            {
                // Falsches Draggable in der DropZone
                allAnswersCorrect = false;
                Debug.Log("Falsche Antwort in einer DropZone.");
                break;
            }
        }

        if (allAnswersCorrect)
        {
            Debug.Log("Alle Antworten sind korrekt!");
            Time.timeScale = 1;
            quizPanel.SetActive(false);
            wallController.OpenWall();
            
        }
        else
        {
            Debug.Log("Nicht alle Antworten sind korrekt.");
            // Hier kannst du Feedback an den Spieler geben
        }
    }
}