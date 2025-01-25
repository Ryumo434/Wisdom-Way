using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCoin : MonoBehaviour
{
    public EnemyHealth EnemyHealth; // Referenz zum EnemyHealth-Skript
    public GameObject coinPrefab; // Prefab der M�nze
    public int minCoins = 1; // Minimale Anzahl an M�nzen
    public int maxCoins = 5; // Maximale Anzahl an M�nzen
    public float dropRadius = 1.0f; // Radius, in dem die M�nzen fallen k�nnen
    private bool hasDropped = false; // Flag, um sicherzustellen, dass M�nzen nur einmal droppen

    // Wird aufgerufen, wenn der Gegner stirbt
    public void OnMonsterDeath()
    {
        if (hasDropped) return; // Verhindert mehrfaches Droppen
        hasDropped = true;

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

    void Update()
    {
        if (EnemyHealth != null && EnemyHealth.currentHealth <= 0)
        {
            OnMonsterDeath();
        }
    }
}
