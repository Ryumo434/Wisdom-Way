using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuizManagerTMP : MonoBehaviour
{
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public Button answerButtonA;
    public Button answerButtonB;
    public Button answerButtonC;
    public Button answerButtonD;
    public TextMeshProUGUI buttonTextA;
    public TextMeshProUGUI buttonTextB;
    public TextMeshProUGUI buttonTextC;
    public TextMeshProUGUI buttonTextD;
    public GameObject quizTrigger;
    [SerializeField] private WallController wallController;

    public float fadeDuration = 1f;

    private string[,] quizData;
    private int currentQuestionIndex = 0;

    // Verschiedene Fragensets
    private string[,] mathQuizData = new string[,]
    {
        {"Was ist 5 + 7?", "10", "12", "14", "16"},
        {"Wie lautet die Quadratwurzel von 64?", "6", "7", "8", "9"},
        {"Wie viele Seiten hat ein W?rfel?", "4", "5", "6", "8"},
        {"Was ist das Ergebnis von 9 * 9?", "72", "81", "90", "99"},
        {"Was ist Pi (auf 2 Nachkommastellen)?", "3.12", "3.14", "3.16", "3.18"}
    };

    private string[,] historyQuizData = new string[,]
    {
        {"Wer war der erste Pr?sident der USA?", "George Washington", "Abraham Lincoln", "Thomas Jefferson", "John Adams", "1"},
        {"Wann begann der Zweite Weltkrieg?", "1937", "1939", "1941", "1945", "2"},
        {"Wann endete der Zweite Weltkrieg?", "1943", "1944", "1945", "1946", "3"},
        {"In welchem Jahr wurde die Berliner Mauer gebaut?", "1948", "1950", "1961", "1989", "3"},
        {"Wer war Julius Caesar?", "R?mischer Kaiser", "Griechischer Philosoph", "?gyptischer Pharao", "Indischer K?nig" , "1"}
    };

    private string[,] scienceQuizData = new string[,]
    {
        {"Was ist die chemische Formel von Wasser?", "H2O", "CO2", "O2", "NaCl"},
        {"Wie viele Planeten gibt es im Sonnensystem?", "7", "8", "9", "10"},
        {"Was ist die schwerste nat?rliche Substanz auf der Erde?", "Gold", "Silber", "Eisen", "Uran"},
        {"Was ist das gr??te Organ des menschlichen K?rpers?", "Herz", "Leber", "Lunge", "Haut"},
        {"Welches Gas ist am h?ufigsten in der Erdatmosph?re?", "Sauerstoff", "Stickstoff", "Kohlenstoffdioxid", "Wasserstoff"}
    };

    void Start()
    {
        answerButtonA.onClick.AddListener(() => SelectAnswer(0));
        answerButtonB.onClick.AddListener(() => SelectAnswer(1));
        answerButtonC.onClick.AddListener(() => SelectAnswer(2));
        answerButtonD.onClick.AddListener(() => SelectAnswer(3));
        
    }

    void LoadQuestion(int index)
    {
        questionText.text = quizData[index, 0];
        buttonTextA.text = "A: " + quizData[index, 1];
        buttonTextB.text = "B: " + quizData[index, 2];
        buttonTextC.text = "C: " + quizData[index, 3];
        buttonTextD.text = "D: " + quizData[index, 4];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hat Trigger betreten: " + gameObject.name);

            // ?berpr?fen, welcher Trigger das Quiz ausl?st
            if (gameObject.name == "MathQuiz")
            {
                quizData = mathQuizData;
                Debug.Log("Math Quiz geladen: " + quizData.Length + " Fragen");
            }
            else if (gameObject.name == "ScienceQuiz")
            {
                quizData = scienceQuizData;
                Debug.Log("Science Quiz geladen: " + quizData.Length + " Fragen");
            }
            else if (gameObject.name == "HistoryQuiz")
            {
                quizData = historyQuizData;
                Debug.Log("History Quiz geladen: " + quizData.Length + " Fragen");
            }

            if (quizData != null && quizData.GetLength(0) > 0)
            {
                quizPanel.SetActive(true);
                StartCoroutine(FadeInButtons());
                currentQuestionIndex = 0;
                LoadQuestion(currentQuestionIndex);
            }
            else
            {
                Debug.LogError("QuizData ist leer oder nicht initialisiert!");
            }
        }
    }


    public void SelectAnswer(int answerIndex)
    {
        // Debugging, um zu ?berpr?fen, ob quizData korrekt gesetzt ist
        if (quizData == null)
        {
            Debug.LogError("quizData ist null!");
            return;
        }

        // Debugging, um zu ?berpr?fen, ob currentQuestionIndex g?ltig ist
        if (currentQuestionIndex >= quizData.GetLength(0))
        {
            Debug.LogError("currentQuestionIndex ist au?erhalb des Bereichs!");
            return;
        }

        if ((answerIndex + 1).ToString() == quizData[currentQuestionIndex, 5])
        {
            Debug.Log("Antwort ist RICHTIG! Ausgew?hlte Antwort: " + quizData[currentQuestionIndex, answerIndex + 1]);
        } else
        {
            string correctAnswerIndexString = quizData[currentQuestionIndex, 5];
            int correctAnswerIndex = int.Parse(correctAnswerIndexString);
            Debug.Log("Antwort ist FALSCH! Ausgew?hlte Antwort: " + quizData[currentQuestionIndex, answerIndex + 1] + "Richtige Antwort w?re: " + quizData[currentQuestionIndex, correctAnswerIndex]);
        }

    

        currentQuestionIndex++;
        if (currentQuestionIndex < quizData.GetLength(0))
        {
            LoadQuestion(currentQuestionIndex);
        }
        else
        {
            Debug.Log("Quiz abgeschlossen");
            quizTrigger.SetActive(false);
            quizPanel.SetActive(false);
            Time.timeScale = 1;
            wallController.OpenWall();
        }
    }

    private IEnumerator FadeInButtons()
    {
        float elapsedTime = 0f;

        Image buttonAImage = answerButtonA.GetComponent<Image>();
        Image buttonBImage = answerButtonB.GetComponent<Image>();
        Image buttonCImage = answerButtonC.GetComponent<Image>();
        Image buttonDImage = answerButtonD.GetComponent<Image>();

        // Fade logic
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            SetAlpha(questionText, alpha);
            SetAlpha(buttonAImage, buttonTextA, alpha);
            SetAlpha(buttonBImage, buttonTextB, alpha);
            SetAlpha(buttonCImage, buttonTextC, alpha);
            SetAlpha(buttonDImage, buttonTextD, alpha);
            

            yield return null;
        }
        Time.timeScale = 0;
    }

    private void SetAlpha(TextMeshProUGUI tmpText, float alpha)
    {
        Color textColor = tmpText.color;
        textColor.a = alpha;
        tmpText.color = textColor;
    }

    private void SetAlpha(Image buttonImage, TextMeshProUGUI buttonText, float alpha)
    {
        Color imageColor = buttonImage.color;
        imageColor.a = alpha;
        buttonImage.color = imageColor;

        Color textColor = buttonText.color;
        textColor.a = alpha;
        buttonText.color = textColor;
    }
}
