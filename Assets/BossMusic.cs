using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour
{

    AudioSource audioSource;
    private bool audioSourcePlaying = false;
    [SerializeField] private AudioClip WindSong;
    [SerializeField] private AudioClip bossFightSong;
    [SerializeField] private AudioClip WinnerSong;   

    GameObject Boss;
    BossHealth bossHealth;
    int counter = 0;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Boss = GameObject.FindWithTag("Boss");

        bossHealth = Boss.GetComponent<BossHealth>();

        
    }







    // Start is called before the first frame update
    void Start()
    {
        
        audioSource.clip = WindSong;
        audioSource.Play();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!audioSourcePlaying)
        {
            audioSource.clip = bossFightSong;   
            audioSource.Play();
            audioSourcePlaying = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth.bossIsDead && counter == 0)
        {
            
            //audioSource.Stop();
            

            audioSource.clip = WinnerSong;
            audioSource.Play();
            counter ++;
        }
    }
}
