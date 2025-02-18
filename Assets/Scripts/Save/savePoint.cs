using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] public GameObject SavePointPanel;

    private void Start()
    {
    
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SavePointPanel.SetActive(true);
            Debug.Log("Player hat den Collider betreten.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SavePointPanel.SetActive(false);
            Debug.Log("Player hat den Collider verlassen.");
        }
    }
}
