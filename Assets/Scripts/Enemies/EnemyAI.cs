using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] public int enemyDamage;
    //[SerializeField] Transform enemyTransform;

    
    
   private enum State
    {
        Roaming
    }

    private State state;
    private EnemyPathFinding enemyPathFinding;

    private void Awake()
    {
        
        enemyPathFinding = GetComponent<EnemyPathFinding>();
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
            enemyPathFinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(roamChangeDirFloat);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized;
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
}


