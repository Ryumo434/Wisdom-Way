using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using System.Collections;

public class CreditsScroller : MonoBehaviour
{
    public TextMeshProUGUI creditsText;
    public float scrollSpeed = 20f;
    public float typeSpeed = 0.05f;
    private string fullText;
    private Coroutine typingCoroutine;
    public CanvasGroup fadePanel;
    public float fadeSpeed = 1f;
    public string nextSceneName = "Menu";
    private bool isFading = false;
    private bool hasStartedFadeOut = false;
    public float endScrollPositionY;

    void Start()
    {
        GameObject ui = GameObject.Find("UICanvas");
        ui.SetActive(false);
        fullText = creditsText.text;
        creditsText.text = "";
        typingCoroutine = StartCoroutine(TypeText());

        fadePanel.alpha = 0f;
    }

    IEnumerator TypeText()
    {
        foreach (char c in fullText)
        {
            creditsText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    void Update()
    {
        if (!isFading)
        {
            creditsText.rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                creditsText.transform.position.y,
                Camera.main.transform.position.z
            );

            if (creditsText.rectTransform.anchoredPosition.y >= endScrollPositionY && !hasStartedFadeOut)
            {
                hasStartedFadeOut = true;
                StartCoroutine(StartFadeOut());
            }
        }
    }

    IEnumerator StartFadeOut()
    {
        isFading = true;

        // Fade-In des Panels (Alpha von 0 auf 1)
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadePanel.alpha = alpha;
            yield return null;
        }

        // Kurz warten, bevor die Szene gewechselt wird
        yield return new WaitForSeconds(0.5f);

        // Szene wechseln
        SceneManager.LoadScene(nextSceneName);
    }
}