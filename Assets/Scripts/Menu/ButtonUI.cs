using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public AudioSource backgroundMusic;  // Die AudioSource, die die Musik abspielt
    public float fadeDuration = 2f;      // Die Zeit, in der die Musik ausblendet
    [SerializeField] private string newGameLevel = "Home";
    public void NewGameButton()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(FadeOutAndLoadScene(newGameLevel));
        //SceneManager.LoadScene(newGameLevel);
    }


    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float startVolume = backgroundMusic.volume;

        // Verringere die Lautstärke allmählich
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        // Sobald die Musik ausgeblendet ist, stoppe die Musik
        backgroundMusic.volume = 0;
        backgroundMusic.Stop();

        // Wechsle die Szene
        SceneManager.LoadScene(sceneName);
    }
}
