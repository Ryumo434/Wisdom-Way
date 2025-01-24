using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject ShopUI;
    public GameObject eText;

    private bool isPlayerInTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        eText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInTrigger)
        {
            if (Input.GetKey(KeyCode.E))
            {
                ShopUI.SetActive(true);
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                ShopUI.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("player entered");
            isPlayerInTrigger = true;
            eText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            ShopUI.SetActive(false);
            eText.SetActive(false);
        }
    }
}
