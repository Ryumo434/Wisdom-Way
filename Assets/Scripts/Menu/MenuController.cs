using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    // Array für alle Buttons im Hauptmenü
    public CanvasGroup[] menuButtons;
    // Die Geschwindigkeit, mit der die Buttons eingeblendet werden (kann im Editor angepasst werden)
    public float fadeInDuration = 1.0f;

    void Start()
    {
        // Zu Beginn sind die Buttons unsichtbar
        foreach (CanvasGroup button in menuButtons)
        {
            button.alpha = 0;
            button.interactable = false;
            button.blocksRaycasts = false;
        }

        // Starte die Einblendung der Buttons
        StartCoroutine(FadeInButtons());
    }

    IEnumerator FadeInButtons()
    {
        // Für jeden Button im Menü, Einblendung schrittweise
        foreach (CanvasGroup button in menuButtons)
        {
            float elapsedTime = 0.0f;

            // Buttons aktivieren
            button.interactable = true;
            button.blocksRaycasts = true;

            // Lasse den Button langsam einblenden
            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                button.alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
                yield return null;
            }
        }
    }
}
