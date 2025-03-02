using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverCanvas;
    public GameObject gameOverScreen;
    public Button restartButton;
    public Button mainMenuButton;
    private GameObject player;

    private ActiveInventory inventory;
    private GameObject activeInventory;

    private void Awake()
    {
        GameOverCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
        player = GameObject.FindWithTag("Player");
        activeInventory = GameObject.FindWithTag("Inventory");
        inventory = activeInventory.GetComponent<ActiveInventory>();
    }

    private void Start()
    {
        gameOverScreen.SetActive(false);
        restartButton.onClick.AddListener(LoadGame);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void Update()
    {
        if (PlayerHealth.Instance == null)
        {
            Debug.LogError(" Fehler: PlayerHealth.Instance ist NULL!");
            return;
        }

        if (PlayerHealth.Instance.isDead)
        {
            Debug.Log(" Spieler ist tot, Game Over Screen wird angezeigt.");
            ShowGameOverScreen();
            
        }
    }

    private void ShowGameOverScreen()
    {
        if (gameOverScreen == null)
        {
            Debug.LogError(" Fehler: gameOverScreen ist NULL!");
            return;
        }

        Debug.Log(" Game Over Screen wird aktiviert.");
        gameOverScreen.SetActive(true);
    }


    public void LoadGame()
    {
        if (GameManager.Instance != null)
        {
            Destroy(player);
            GameManager.Instance.LoadGame();
        }
        else
        {
            Debug.LogWarning("GameManager instance is null!");
        }
    }


    public void LoadMainMenu()
    {
        inventory.ClearInventory();
        Destroy(player);
        SceneManager.LoadScene("Menu"); 
    }

}
