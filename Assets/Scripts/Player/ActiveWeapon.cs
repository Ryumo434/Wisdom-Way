using System.Collections;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;
    private float timeBetweenAttacks = 0.5f;

    private bool attackButtonDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        AttackCooldown();
    }

    private void Update()
    {
        if (DialogueManager.Instance != null)
        {
            if (DialogueManager.Instance.dialogueIsPlaying)
            {
                return;
            }
        }

        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;

        if (CurrentActiveWeapon != null)
        {
            IWeapon weapon = CurrentActiveWeapon as IWeapon;
            if (weapon != null)
            {
                timeBetweenAttacks = weapon.GetWeaponInfo().weaponCooldown;
            }
        }
        else
        {
            timeBetweenAttacks = 0.5f;
        }

        AttackCooldown();
        isAttacking = false;
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
        isAttacking = false;
    }

    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (!attackButtonDown || isAttacking || CurrentActiveWeapon == null)
            return;

        IWeapon weapon = CurrentActiveWeapon as IWeapon;
        if (weapon == null)
        {
            Debug.LogWarning("CurrentActiveWeapon implementiert nicht das Interface IWeapon.");
            return;
        }

        AttackCooldown();

        weapon.Attack();
    }
}