using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton-Instanz
    public TMP_Text scoreText; // TextMeshPro-Text für den Score
    private int currentScore = 0; // Aktueller Punktestand

    void Awake()
    {
        // Singleton-Setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Objekt zwischen Szenen erhalten
        }
        else
        {
            Destroy(gameObject); // Verhindere Duplikate
        }
    }

    public void AddScore(int amount)
    {
        // Score erhöhen
        currentScore = Mathf.Min(currentScore + amount, 9999);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
        else
        {
            Debug.LogError("ScoreText ist nicht zugewiesen!");
        }
    }
}
