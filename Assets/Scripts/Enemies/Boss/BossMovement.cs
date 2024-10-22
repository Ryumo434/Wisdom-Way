using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2F;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private Animator bossAnimation;
    private Transform player;

    private bool isMoving;

    GameObject AttackObject;
    private PolygonCollider2D attackCollider;
    EnemyHealth enemyHealth;

   // private BoxCollider2D boxCollider2D;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        bossAnimation = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();

       

        player = GameObject.FindGameObjectWithTag("Player").transform;


        AttackObject = transform.GetChild(0).gameObject;

        if (AttackObject == null)
        {
            Debug.Log("Kind Objekt nicht gefunden");
        }

        attackCollider = AttackObject.GetComponent<PolygonCollider2D>();
        //boxCollider2D = GetComponent<BoxCollider2D>();




    }

    private void Update()
    {

        if (enemyHealth.bossIsDead) { return; }

        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction * moveSpeed;
        

        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;  // Flip nach links
            AttackObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            //boxCollider2D.transform.localScale = new Vector3(-1f, 1f, 1f);


        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false; // Flip nach rechts
            AttackObject.transform.localScale = new Vector3(1f, 1f, 1f);
            //boxCollider2D.transform.localScale = new Vector3(1f, 1f, 1f);

        }


    }

    
    private void FixedUpdate()
    {
        if (knockback.GettingKnockedBack) { return; }
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

    }

    void stopMovement()
    {
        movement = Vector3.zero;
    }

   
}
