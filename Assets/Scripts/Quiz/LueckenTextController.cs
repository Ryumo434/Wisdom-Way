using UnityEngine;
using TMPro;

public class LueckenTextController : MonoBehaviour
{
    [SerializeField] private GameObject PressE;
    [SerializeField] private GameObject quizPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI buttonText1;
    [SerializeField] private TextMeshProUGUI buttonText2;
    [SerializeField] private TextMeshProUGUI buttonText3;
    [SerializeField] private TextMeshProUGUI buttonText4;
    [SerializeField] private TextMeshProUGUI buttonText5;

    private string[,] quizData;
    private int currentQuestionIndex = 0;
    private bool isPlayerInTrigger;


    // Lueckentext Sets
    private string[,] lueckenTextGeschichte = new string[,]
    {
        {"Im Jahr ______________ entdeckte ______________ und nicht ______________, den Seeweg nach Indien.", "1498", "Vasco da Gama", "Christoph Kolumbus", "Ferdinand Magellan", "1492"}
    };

    


    // Start is called before the first frame update
    void Start()
    {
        quizPanel.SetActive(false);
        PressE.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            quizPanel.SetActive(true);
            currentQuestionIndex = 0;
            LoadQuestion(currentQuestionIndex);
            Time.timeScale = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hat Trigger betreten: " + gameObject.name);

            // ?berpr?fen, welcher Trigger das Quiz ausl?st
            if (gameObject.name == "HistoryLueckenText")
            {
                quizData = lueckenTextGeschichte;
                Debug.Log("Geschichts Lückentext geladen: " + quizData.Length + " Fragen");
            }
            

            if (quizData != null && quizData.GetLength(0) > 0)
            {
                isPlayerInTrigger = true;
                PressE.SetActive(true);
            }
            else
            {
                Debug.LogError("QuizData ist leer oder nicht initialisiert!");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hat Trigger verlassen: " + gameObject.name);
            PressE.SetActive(false);
            isPlayerInTrigger = false;
        }
    }

    void LoadQuestion(int index)
    {
        questionText.text = quizData[index, 0];
        buttonText1.text = quizData[index, 1];
        buttonText2.text = quizData[index, 2];
        buttonText3.text = quizData[index, 3];
        buttonText4.text = quizData[index, 4];
        buttonText5.text = quizData[index, 5];
    }
}
