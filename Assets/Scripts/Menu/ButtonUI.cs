using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public AudioSource backgroundMusic;  // Die AudioSource, die die Musik abspielt
    public float fadeDuration = 2f;      // Die Zeit, in der die Musik ausblendet
    [SerializeField] private string newGameLevel;
    public void NewGameButton()
    {
        DontDestroyOnLoad(gameObject);
        GameManager.Instance.LoadInitialGame();
    }
}
