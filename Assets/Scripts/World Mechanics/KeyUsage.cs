using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUsage : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private KeyController keyController;
    [SerializeField] private GameObject eImage;

    private bool isPlayerInTrigger = false;
    private GameObject itemGameObject;

    void Start()
    {
        keyController.door.SetActive(true);
    }

    void Update()
    {
        if (prefab.name == "getKey")
        {
            if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
            {
                keyController.playerHasKey = true;
            }
        }

        if (keyController.playerHasKey)
        {
            eImage.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!keyController.playerHasKey && prefab.name != "useKey")
        {
            eImage.SetActive(true);
        }
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            if (keyController.playerHasKey && prefab.name == "useKey")
            {
                keyController.door.SetActive(false);
                eImage.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        eImage.SetActive(false);
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            if (keyController.playerHasKey && prefab.name == "useKey")
            {
                eImage.gameObject.SetActive(false);
            }
        }
    }

}
