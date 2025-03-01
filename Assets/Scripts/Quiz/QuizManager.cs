using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Threading;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject barrier;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerController playerController;

    public float fadeDuration = 1f;

    private string[,] quizData;
    private int currentQuestionIndex = 0;
    private bool isQuizActive;

    // Verschiedene Fragensets
    // {"Frage", "Antwort 1", "Antwort 2", "Antwort 3", "Antwort 4", "Index der richtigen Antwort"}
    private string[,] historyQuizData = new string[,]
    {
        {"Wer war der berühmte Pharao, dessen Grab im Tal der Könige entdeckt wurde?", "Ramses II", "Cheops", "Tutanchamun", "Echnaton", "3"},
        {"Wer gilt als Begründer des Mongolischen Reiches?", "Kublai Khan", "Genghis Khan", "Tamerlan", "Attila", "2"},
        {"In welchem Jahr wurde die Berliner Mauer gebaut?", "1948", "1950", "1961", "1989", "3"},
        {"Welches alte Reich war für die hängenden Gärten von Babylon berühmt?", "Mesopotamien", "Ägypten", "Persien", "Assyrien", "1"},
        {"Wann fand die Französische Revolution statt?", "1776", "1789", "1812", "1848", "2"},
        {"Wer war der Führer der Sowjetunion während des Zweiten Weltkriegs?", "Wladimir Lenin", "Josef Stalin", "Nikita Chruschtschow", "Leonid Breschnew", "2"},
        {"In welchem Jahr landeten die ersten Menschen auf dem Mond?", "1969", "1965", "1972", "1980", "1"},
        {"Welches antike Volk baute die berühmten Pyramiden von Gizeh?", "Die Römer", "Die Griechen", "Die Ägypter", "Die Mesopotamier", "3"},
        {"Wer war der Gründer des Osmanischen Reiches?", "Mehmed II", "Selim I", "Süleyman der Prächtige", "Osman I", "4"},
        {"Welches Dokument gilt als ein Grundstein der modernen Demokratie?", "Bill of Rights", "Magna Carta", "Vertrag von Versailles", "Habeas Corpus", "2"}
    };

    private string[,] ArtQuizData = new string[,]
    {
        {"Ich schnitt mir ein Ohr ab, doch meine Bilder sind weltberühmt. Wer bin ich?", "Vincent van Gogh", "Pablo Picasso", "Leonardo da Vinci", "Gustav Klimt", "1"},
        {"Ich erscheine oft in Kunstwerken, stehe für Frieden. Was bin ich?", "Taube", "Waschbär", "Schlange", "Eule","4"},
        {"Welche Kunstrichtung ist für ihre intensiven Farbflächen und geometrischen Formen bekannt?", "Kubismus", "Impressionismus", "Romantik", "Barock","2"},
        {"Welche Statue der Antike ist bekannt dafür, dass ihr die Arme fehlen?", "Venus von Milo", "David von Michelangelo", "Nike von Samothrake", "Der Denker","2"},
        {"Welcher Künstler ist als Mitbegründer der Pop-Art bekannt?", "Andy Warhol", "Salvador Dalí", "Rembrandt", "Claude Monet","1"}
    };

    void Start()
    {
        answerButtonA.onClick.AddListener(() => SelectAnswer(0));
        answerButtonB.onClick.AddListener(() => SelectAnswer(1));
        answerButtonC.onClick.AddListener(() => SelectAnswer(2));
        answerButtonD.onClick.AddListener(() => SelectAnswer(3));
    }


    void Update()
    {
        if (isQuizActive && Input.GetKeyDown(KeyCode.Escape))
        {
            // Zeit fortsetzen und das Panel schließen, wenn ESC gedrückt wird
            Time.timeScale = 1f;
            playerController.isQuizPlaying = false;
            quizPanel.SetActive(false);
            isQuizActive = false;
        }
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
                isQuizActive = true;

                if (gameObject.name == "HistoryQuiz")
                {
                    quizData = historyQuizData;
                    Debug.Log("History Quiz geladen: " + quizData.Length + " Fragen");
                }
                else if (gameObject.name == "ArtQuiz")
                {
                    quizData = ArtQuizData;
                    Debug.Log("Art Quiz geladen: " + quizData.Length + " Fragen");
                }

                if (quizData != null && quizData.GetLength(0) > 0)
                {
                    quizPanel.SetActive(true);
                    StartCoroutine(FadeInAndPause());
                    currentQuestionIndex = 0;
                    LoadQuestion(currentQuestionIndex);
                    playerController.isQuizPlaying = true;
                }
                else
                {
                    Debug.LogError("QuizData ist leer oder nicht initialisiert!");
                }
            }
        }


        public void SelectAnswer(int answerIndex)
        {
            if (quizData == null)
            {
                Debug.LogError("quizData ist null!");
                return;
            }

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


                if (health == null)
                {
                    FindPlayerHealth();
                }

                if (health != null)
                {
                    health.QuizTakeDamage(2, this.transform);
                }
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
                barrier.SetActive(false);
                playerController.isQuizPlaying = false;

                if (SceneManager.GetActiveScene().name == "Dungeon_GeE1")
                {
                    if (wallController != null)
                    {
                        wallController.OpenWall();
                    }
                    else
                    {
                        Debug.LogWarning("WallController ist nicht gesetzt, aber die Szene ist Dungeon_Ge E1!");
                    }
                }

                Time.timeScale = 1f;
            }
        }

        private IEnumerator FadeInAndPause()
        {
            yield return StartCoroutine(FadeInButtons());
            Time.timeScale = 0f;
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

        void FindPlayerHealth()
        {
            if (health == null)
            {
                health = FindObjectOfType<PlayerHealth>();
                if (health == null)
                {
                    Debug.LogError("PlayerHealth-Objekt wurde nicht gefunden!");
                }
            }
        }
    }
