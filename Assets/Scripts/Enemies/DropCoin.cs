using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCoin : MonoBehaviour
{
    public EnemyHealth EnemyHealth;
    //drop Coin vars
    public GameObject coinPrefab; // Prefab der M�nze
    public int minCoins = 1; // Minimale Anzahl an M�nzen
    public int maxCoins = 5; // Maximale Anzahl an M�nzen
    public float dropRadius = 1.0f; // Radius, in dem die M�nzen fallen k�nnen
                                    // Start is called before the first frame update
    public void OnMonsterDeath()
    {
        // Berechne zuf�llige Anzahl an M�nzen
        int numberOfCoins = Random.Range(minCoins, maxCoins + 1);

        // Generiere M�nzen um die Position des Monsters
        for (int i = 0; i < numberOfCoins; i++)
        {
            DropCoinFunc();
        }
    }

    private void DropCoinFunc()
    {
        // Zuf�llige Position innerhalb eines Kreises
        Vector2 dropPosition = (Vector2)transform.position + Random.insideUnitCircle * dropRadius;

        // Erstelle die M�nze an der berechneten Position
        Instantiate(coinPrefab, dropPosition, Quaternion.identity);
    }

    void Start()
    {
        EnemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if(EnemyHealth.currentHealth <= 0)
        {
            OnMonsterDeath();
        }
    }


}
