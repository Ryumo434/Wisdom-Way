using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.SceneManagement;





public class PauseMenuScript : MonoBehaviour

{

    bool gamePaused = false;

    [SerializeField] GameObject pauseMenu;



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



    public void Home()

    {

        SceneManager.LoadScene(0);

    }



    public void Continue()

    {

        Time.timeScale = 1;

        gamePaused = false;

        pauseMenu.SetActive(false);

    }

}
