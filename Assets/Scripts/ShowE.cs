using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowE : MonoBehaviour
{
    [SerializeField] private GameObject pressE;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player ist im Trigger.");

            pressE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player geht aus Trigger!");

            pressE.SetActive(false);
        }
    }
}
