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
    public Sprite inventoryEmpty;

    private void Awake()
    {
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
        ClearInventory();
        SceneManager.LoadScene("Menu");
      
        Time.timeScale = 1;

    }

    private void ClearInventory()
    {
        int slotCount = inventory.transform.childCount;
        for (int i = 0; i < slotCount; i++)
        {
            Transform slotTransform = inventory.transform.GetChild(i);
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            GameObject item = slotTransform.Find("Item")?.gameObject;
            slot.setStackCount("1");
            slot.setStackCountInvisible();



            if (slot.GetWeaponInfo() != null)
            {
                slot.SetWeaponInfo(null);
            }

            Image itemImage = item.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = inventoryEmpty;
            }
        }
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
