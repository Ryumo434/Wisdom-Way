using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }


    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform SlashSpawnPoint;

    public bool isQuizPlaying;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimatorPlayer;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;
    private bool isDragging = false;
    private bool submitPressed = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimatorPlayer = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }


    public static PlayerController GetInstance()
    {
        return Instance;
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
       // if (DialogueManager.Instance.dialogueIsPlaying) {return;}

        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

   

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimatorPlayer.SetFloat("moveX", movement.x);
        myAnimatorPlayer.SetFloat("moveY", movement.y);
        myAnimatorPlayer.SetBool("isDragging", isDragging);
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.isDead || isQuizPlaying) { return; }
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.isDead) { return; }

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    public void setMoveSpeed(float speedMultiplier)
    {
        moveSpeed = startingMoveSpeed * speedMultiplier;
        Debug.Log($"Player speed updated to: {moveSpeed} (Multiplier: {speedMultiplier})");
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.currentStamina > 0)
        {
            Stamina.Instance.UseStamina();

            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    public void activateDraggingAnimation()
    {
        isDragging = true;
    }

    public void deactivateDraggingAnimation()
    {
        isDragging = false;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }
}
