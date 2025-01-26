using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] public int enemyDamage;
    [SerializeField] Slider bossHealthbar;
    //[SerializeField] Transform enemyTransform;
    BossMovement bossMovement;
    private Animator bossAnimator;
    GameObject AttackObject;
    private PolygonCollider2D attackCollider;
    //private GameObject attackZone;

  
    //EnemyHealth enemyHealth;
    BossHealth bossHealth;
    


    private enum State
    {
        Roaming
    }

    private State state;
    private EnemyPathFinding enemyPathFinding;

    private void Awake()
    {
        
        bossMovement = GetComponent<BossMovement>();
        bossAnimator = GetComponent<Animator>();
        //enemyHealth = GetComponent<EnemyHealth>();
        bossHealth = GetComponent<BossHealth>();


        AttackObject = transform.GetChild(0).gameObject;

        if (AttackObject == null)
        {
            Debug.Log(" Kind Objekt nicht gefunden");
        }

        attackCollider = AttackObject.GetComponent<PolygonCollider2D>();
        attackCollider.enabled = false;
        
        
        //playerHealth = GetComponent<PlayerHealth>();

        state = State.Roaming;
    }

    private void Start()
    {
        
        StartCoroutine(RoamingRoutine());
    }


    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            //bossMovement.MoveTo(roamPosition);
            yield return new WaitForSeconds(roamChangeDirFloat);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }



    private void OnCollisionStay2D(Collision2D other)
    {

        //EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
        //PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth)
        {
            playerHealth.TakeDamage(enemyDamage, this.transform);
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth)
        {
            bossAnimator.SetBool("Attack1", true);
            //playerHealth.TakeDamage(2, this.transform);
        }

    }

    */



    void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    void DisableAttackCollider()
    {
        attackCollider.enabled = false;
        bossAnimator.SetBool("Attack1", false);
    }


}
