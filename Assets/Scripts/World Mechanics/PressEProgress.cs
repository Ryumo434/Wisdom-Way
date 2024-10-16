using UnityEngine;
using UnityEngine.UI;

public class PressEProgress : MonoBehaviour
{
    public Slider progressBar;
    public float totalPressTimeRequired = 15f; // Gesamtzeit, die 'e' gedr?ckt werden muss
    public GameObject canvas;
    public GameObject plankBarrier;
    public GameObject plank;

    private bool isPlayerInTrigger = false;
    private float ePressTime = 0f;

    private void Start()
    {
        if (progressBar != null)
        {
            progressBar.maxValue = totalPressTimeRequired;
            progressBar.value = 0f;
            canvas.SetActive(false);
            plank.SetActive(false);
            plankBarrier.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            if (progressBar != null)
            {
                canvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            ePressTime = 0f;
            if (progressBar != null)
            {
                progressBar.value = 0f;
                canvas.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger)
        {
            if (Input.GetKey(KeyCode.E))
            {

                ePressTime += Time.deltaTime;
                if (ePressTime >= totalPressTimeRequired)
                {
                    ePressTime = totalPressTimeRequired;

                    Debug.Log("'e' wurde f?r 15 Sekunden gedr?ckt!");
                    plank.SetActive(true);
                    plankBarrier.SetActive(false);
                }
            }
            else
            {

                ePressTime = 0f;
            }


            if (progressBar != null)
            {
                progressBar.value = ePressTime;
            }
        }
    }
}