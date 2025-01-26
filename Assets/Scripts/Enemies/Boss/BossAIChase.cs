using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIChase : MonoBehaviour
{
    public GameObject player;
    public float ChaseSpeed;
    public float ChaseDistance;
    private Animator bossAnimator;

    private float distance;
    EnemyHealth enemyHealth;
    BossHealth bossHealth;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

    }

    private void Awake()
    {
        bossAnimator = GetComponent<Animator>();
        //enemyHealth = GetComponent<EnemyHealth>();
        bossHealth = GetComponent<BossHealth>();

    }

    // Update is called once per frame
    void Update()
    {    
        
        if(bossHealth.bossIsDead) { return; }
        // Distance berechnet den Abstand zwischen zwei gameobjects und gibt diesen als Float zurück
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        //Math.Atan2 wird verwendet um den winkel von zwei objekten zu berechnen
        //Rad2Deg konvertiert Radiant zu Degree
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        


        if (distance < ChaseDistance)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, ChaseSpeed * Time.deltaTime);
            // transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            Debug.Log("isMovingTrue");
            bossAnimator.SetBool("isMoving", true);
        }
        else
        {
            Debug.Log("isMovingFalse");
            bossAnimator.SetBool("isMoving", false);
        }
    }
}
