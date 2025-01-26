using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float WaitToLoadTime = 1f;
    private PlayerHealth playerHealth;
    private GameObject player;
    
    public void Awake()
    {
        player = GameObject.Find("Player");
        if (playerHealth != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.GetComponent<PlayerController>())
        {
            
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            //playerHealth.currentHealth = playerHealth.maxHealth;
            
            StartCoroutine(LoadSceneRoutine());
            playerHealth.ResetPlayerHealth();

        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        while (WaitToLoadTime >= 0)
        {
            WaitToLoadTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
