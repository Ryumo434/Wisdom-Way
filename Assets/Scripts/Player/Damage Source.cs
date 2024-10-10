using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount;

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //durch das other wird auf das Objekt zugegriffen was in den Collider eintritt
        if (other.gameObject.GetComponent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth?.TakeDamage(damageAmount);
        }
    }
}
