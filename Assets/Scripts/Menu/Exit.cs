using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Diese Funktion wird aufgerufen, wenn der Button geklickt wird
    public void QuitGame()
    {
        // Beendet das Spiel, wenn es in einer Build-Version ausgeführt wird
        Application.Quit();

        // Beendet das Spiel im Editor (nur für Testzwecke)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}