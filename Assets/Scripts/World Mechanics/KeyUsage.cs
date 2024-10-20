using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyUsage : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private KeyController keyController;

    private bool isPlayerInTrigger = false;
    private GameObject itemGameObject;

    // Start is called before the first frame update
    void Start()
    {
        GameObject inventory3 = GameObject.Find("Inventory (3)");

        if (inventory3 != null)
        {
            Transform item = inventory3.transform.Find("Item");

            if (item != null)
            {
                itemGameObject = item.gameObject;
                Debug.Log("Item gefunden: " + item.gameObject.name);
            }
            else
            {
                Debug.LogError("Item wurde nicht unter Inventory (3) gefunden.");
            }
        }
        else
        {
            Debug.LogError("Inventory (3) wurde nicht gefunden.");
        }

        keyController.door.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (prefab.name == "getKey")
        {
            if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
            {
                keyController.playerHasKey = true;
                itemGameObject.SetActive(true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            if (keyController.playerHasKey && prefab.name == "useKey")
            {
                keyController.door.SetActive(false);
                itemGameObject.SetActive(false);
            }
        }
    }
}
