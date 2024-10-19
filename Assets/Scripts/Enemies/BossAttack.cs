using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{

    private Animator bossAnimator;
    

    private void Awake()
    {
        bossAnimator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth)
        {
            //bossAnimator.SetBool("Attack1", true);
            playerHealth.TakeDamage(2, this.transform);
        }
        
    }


    
}
