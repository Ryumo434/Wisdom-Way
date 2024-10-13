using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyUsage : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private KeyController keyController;

    private bool isPlayerInTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
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
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger enter");
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            if (keyController.playerHasKey && prefab.name == "useKey")
            {
                keyController.door.SetActive(false);
            }
        }
    }
}
