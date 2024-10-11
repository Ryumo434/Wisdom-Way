using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pi : MonoBehaviour
{
    public CheckArray checkArray;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject text4;
    public GameObject text5;
    public GameObject torch1;
    public GameObject torch2;
    public GameObject torch3;
    public GameObject torch4;
    public GameObject torch5;
    private bool playerInRange = false;
    private int torchNumber = 0;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            EisPressed();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Player") && !checkArray.CorrectOrder)
    {
        playerInRange = true;
        // Unterscheide zwischen den verschiedenen Fackel-Tags
        if (this.CompareTag("torch1"))
        {
            // Logik für torch1
            Debug.Log("Fackel 1");
            //torch1.SetActive(true);
            text1.SetActive(true);
            torchNumber = 1;
        }
        else if (this.CompareTag("torch2"))
        {
            // Logik für torch2
            Debug.Log("Fackel 2");
            //torch2.SetActive(true);
            text2.SetActive(true);
            torchNumber = 2; 
        }
        else if (this.CompareTag("torch3"))
        {
            // Logik für torch3
            Debug.Log("Fackel 3");
            //torch3.SetActive(true);
            text3.SetActive(true);
            torchNumber = 3;
        }
        else if (this.CompareTag("torch4"))
        {
            // Logik für torch3
            Debug.Log("Fackel 4");
            //torch4.SetActive(true);
            text4.SetActive(true);
            torchNumber = 4;
        }
        else if (this.CompareTag("torch5"))
        {
            // Logik für torch3
            Debug.Log("Fackel 5");
            //torch5.SetActive(true);
            text5.SetActive(true);
            torchNumber = 5;
        }
        
    }
    }

    private void EisPressed() 
    {
        // Wenn der Spieler im Trigger-Bereich ist und die E-Taste gedrückt wird
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !checkArray.CorrectOrder)
        {
            Debug.Log("Aktueller Index vor Zuweisung: " + checkArray.currentTorchIndex);
            // Überprüfen, welche Fackel entfacht werden soll
            switch (torchNumber)
            {
                case 1:
                    torch1.SetActive(true);  // Fackel 1 wird aktiviert
                    checkArray.selectedOrder[checkArray.currentTorchIndex] = 1;
                    break;
                case 2:
                    torch2.SetActive(true);  // Fackel 2 wird aktiviert
                    checkArray.selectedOrder[checkArray.currentTorchIndex] = 2;
                    break;
                case 3:
                    torch3.SetActive(true);  // Fackel 3 wird aktiviert
                    checkArray.selectedOrder[checkArray.currentTorchIndex] = 3;
                    break;
                case 4:
                    torch4.SetActive(true);  
                    checkArray.selectedOrder[checkArray.currentTorchIndex] = 4;
                    break; 
                case 5:
                    torch5.SetActive(true);  
                    checkArray.selectedOrder[checkArray.currentTorchIndex] = 5;
                    break;   
                default:
                    Debug.LogWarning("Unbekannte Fackelnummer: ");
                    break;
            }
            checkArray.currentTorchIndex++;
            Debug.Log("aktueller Index" + checkArray.currentTorchIndex);
            Debug.Log(string.Join(", ", checkArray.selectedOrder));
            CeckArray();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           playerInRange = false;
           text1.SetActive(false);
           text2.SetActive(false);
           text3.SetActive(false);
           text4.SetActive(false);
           text5.SetActive(false);
        }
    }
     
    private void CeckArray() 
    {
        if (checkArray.currentTorchIndex == 5)
        {
            Debug.Log("Alle Fackeln entzündet: " + string.Join(", ", checkArray.selectedOrder));
            // Hier kannst du die Logik hinzufügen, die passiert, wenn alle Fackeln aktiviert sind.
            bool result = CompareArrays(checkArray.rightOrder, checkArray.selectedOrder);
            if (result)
            {
                Debug.Log("JaaaaaaaaaaaaaaaaaaaaaAAAAAAAAAAA");
                checkArray.CorrectOrder = true;
            } else {
                torch1.SetActive(false);
                torch2.SetActive(false);
                torch3.SetActive(false);
                torch4.SetActive(false);
                torch5.SetActive(false);
                ResetSelectedOrder();
                checkArray.currentTorchIndex = 0;
            }
        }
    }

    public bool CompareArrays(int[] array1, int[] array2)
    {
        // Zuerst prüfen, ob beide Arrays die gleiche Länge haben
        if (array1.Length != array2.Length)
        {
            return false;
        }

        // Überprüfe jedes Element auf Gleichheit
        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
            {
                return false;  // Sobald ein Unterschied gefunden wird, ist das Ergebnis false
            }
        }

        return true;  // Wenn alle Elemente gleich sind, ist das Ergebnis true
    }
    
    private void ResetSelectedOrder()
    // setze das Array auf 0 zurueck
    {
        Debug.Log("reset selectedOrder");
        for (int i = 0; i < checkArray.selectedOrder.Length; i++)
        {
            checkArray.selectedOrder[i] = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        checkArray = FindObjectOfType<CheckArray>();
    }
}
