using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Button restartButton;
    public Button mainMenuButton;

    private void Start()
    {
        gameOverScreen.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void Update()
    {
        if (PlayerHealth.Instance.isDead)
        {
            ShowGameOverScreen();
        }
    }

    private void ShowGameOverScreen()
    {
        Time.timeScale = 0f; // Spiel pausieren
        gameOverScreen.SetActive(true);
        
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Spiel fortsetzen
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); 
    }
}
