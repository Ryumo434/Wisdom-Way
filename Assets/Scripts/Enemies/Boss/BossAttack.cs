using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{

    private Animator bossAnimator;
    [SerializeField] private int attackDamage;
    

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
            playerHealth.TakeDamage(attackDamage, this.transform);
        }
        
    }


    
}
