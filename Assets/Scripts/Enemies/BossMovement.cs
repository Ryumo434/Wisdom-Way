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
    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        bossAnimation = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction * moveSpeed;

        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;  // Flip nach links
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false; // Flip nach rechts
        }
        //sqrMagnitude gibt die quadratische Länge des Vektors zurück (0 = Idle, > 0 = Run)
        bossAnimation.SetFloat("speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (knockback.GettingKnockedBack) { return; }
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

    }

   
}
