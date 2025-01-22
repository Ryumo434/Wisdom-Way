using UnityEngine;
using TMPro;

public class addCoin : MonoBehaviour
{
    public TMP_Text scoreText;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger");
            UpdateScore();
            Destroy(gameObject);
        }
    }
    void UpdateScore()
    {
        if (scoreText != null)
        {
            // Aktuellen Score auslesen und um 1 erhöhen
            int currentScore;
            if (int.TryParse(scoreText.text, out currentScore))
            {
                currentScore = Mathf.Min(currentScore + 1, 9999); // Erhöhe Score, begrenze auf 9999
                scoreText.text = currentScore.ToString();
            }
            else
            {
                Debug.LogError("ScoreText enthält keinen gültigen Integer-Wert!");
            }
        }
        else
        {
            Debug.LogError("ScoreText ist nicht zugewiesen!");
        }
    }
}
