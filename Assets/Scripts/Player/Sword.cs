using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float swordAttackCD = .5f;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;
    private bool attackButtonDown, isAttacking = false;

    private GameObject slashAnim;

    private void Awake()
    {
         playerController = GetComponentInParent<PlayerController>();


        activeWeapon = GetComponentInParent<ActiveWeapon>();

        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }


    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        /*playerControls.Combat.Attack ist ein InputAction oder ein ähnliches Ereignis, das von der Steuerung des Spielers verwaltet wird.
         * 
        .started ist ein Ereignis, das ausgelöst wird, wenn der Spieler die Angriffstaste drückt oder den entsprechenden Eingabebefehl auslöst.

        += ist ein Operator, der eine Methode zu einem Ereignis hinzufügt. In diesem Fall wird die Methode StartAttacking hinzugefügt.

        _ => StartAttacking() ist eine Lambda-Ausdruck, der die Methode StartAttacking aufruft, wenn das Ereignis started ausgelöst wird.

        Zusammengefasst: Wenn der Spieler den Angriffsbefehl ausführt (die Angriffstaste drückt), wird die Methode StartAttacking aufgerufen.*/
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Update()
    {
        MouseFollowWithOffset();
        Attack();
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
    {   if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
            StartCoroutine(AttackCDRoutine());
        }
    }

    private IEnumerator AttackCDRoutine()
    {
        yield return new WaitForSeconds(swordAttackCD);
        isAttacking = false;
    }
    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180,0,0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }

    }

    public void SwingDownFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }

    }


        private void MouseFollowWithOffset()
    {
        Vector3 mousepos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousepos.y, mousepos.x) * Mathf.Rad2Deg;

        if (mousepos.x < playerScreenPoint.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0,-180,0);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }
}
