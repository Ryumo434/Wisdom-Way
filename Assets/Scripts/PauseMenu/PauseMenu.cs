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
    private GameObject ControlsPanel;

    public static PauseMenuScript Instance;
    private ActiveInventory inventory;
    private GameObject activeInventory;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        //inventory = ActiveInventory.Instance;
        activeInventory = GameObject.FindWithTag("Inventory");
        inventory = activeInventory.GetComponent<ActiveInventory>();
        //ControlsPanel = GameObject.Find("ControlsPanel");
        ControlsPanel = this.transform.GetChild(0).gameObject;

        if(ControlsPanel == null){
            Debug.Log("Controlspanel nicht gefunden");
        }
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


    public void Controls()
    {
        pauseMenu.SetActive(false);
        ControlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        pauseMenu.SetActive(true);
        ControlsPanel.SetActive(false);
    }

    public void reactivatePauseMenu()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }

}
