using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Diese Funktion wird aufgerufen, wenn der Button geklickt wird
    public void QuitGame()
    {
        // Beendet das Spiel, wenn es in einer Build-Version ausgef�hrt wird
        Application.Quit();

        // Beendet das Spiel im Editor (nur f�r Testzwecke)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}