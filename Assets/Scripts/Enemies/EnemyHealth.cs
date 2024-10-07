using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private int currentHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;

    [SerializeField] private FloatingHealthbar healthbar;

    //private int currentHealth;
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();

        //healthbar = GetComponentInChildren<FloatingHealthbar>(); // Hier wird die Healthbar gefunden.
        /*
        if (healthbar == null)
        {
            Debug.LogError("FloatingHealthbar not found on the enemy object or its children!");
        }*/

    }

    private void Start()
    {
        currentHealth = startingHealth;
        // healthbar.UpdateHealthBar(currentHealth, startingHealth);
    }

    public void TakeDamage(int damage)
    {   // ist dasselbe wie "currentHealth = currentHealth - damage;"
        currentHealth -= damage;
        healthbar.UpdateHealthBar(currentHealth, startingHealth);

        knockback.GetknockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            //partikel(deathVFXPrefab ein Gameobject) wird an der position(transform.Position) des Enemy instanziiert ohne dass eine rotation stattfindet(Quaternion.identity)
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


}