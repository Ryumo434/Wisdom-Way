using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    /*Das ist eine Performance-Optimierung: Statt st�ndig den String "Fire" zu verwenden, was vergleichsweise langsam ist, verwendet Unity den berechneten Hash-Wert (int), was schneller ist, insbesondere in h�ufig aufgerufenen Methoden wie Animationstriggern.
    "Fire" bezieht sich auf den Namen eines Animationsparameters, der im Animator-Controller festgelegt wurde(z.B.einen Trigger oder Bool mit dem Namen "Fire"). */
    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (!InventoryModeManager.InventoryIsOpen)
        {
            myAnimator.SetTrigger(FIRE_HASH);                                        //�bernimmt die Rotation der ausger�steten Waffe               
            GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
            newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);
        }
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
