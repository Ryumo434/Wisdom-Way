using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using TMPro;





public class PauseMenuScript : MonoBehaviour

{

    bool gamePaused = false;

    [SerializeField] GameObject pauseMenu;

    public static PauseMenuScript Instance;
    private ActiveInventory inventory;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        inventory = ActiveInventory.Instance;
    }

    void Update()

    {

        if (Input.GetKeyDown(KeyCode.Escape) && gamePaused == false)
        {

            Time.timeScale = 0;

            gamePaused = true;

            pauseMenu.SetActive(true);

        }

        else if ((Input.GetKeyDown(KeyCode.Escape) && gamePaused == true))

        {

            Time.timeScale = 1;

            gamePaused = false;

            pauseMenu.SetActive(false);

        }

    }



    public void MainMenu()

    {
        inventory.ClearInventory();
        SceneManager.LoadScene("Menu");
      
        Time.timeScale = 1;

    }


    public void Resume()

    {

        Time.timeScale = 1;

        gamePaused = false;

        pauseMenu.SetActive(false);

    }

    public void Options()
    {

    }

    public void SaveGame()
    {

    }

    public void reactivatePauseMenu()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }


}
