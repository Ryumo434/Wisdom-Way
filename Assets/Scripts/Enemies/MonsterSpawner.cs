using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    public GameObject monsterPrefab;
    public float spawnInterval = 3f;
    public Vector3 spawnOffset;

    private bool isPlayerInTrigger = false;

    private void Start()
    {
        
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            SpawnMonster();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMonster()
    {
        if (monsterPrefab != null && isPlayerInTrigger)
        {
            Instantiate(monsterPrefab, transform.position + spawnOffset, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Kein Monster-Prefab zugewiesen!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }
}