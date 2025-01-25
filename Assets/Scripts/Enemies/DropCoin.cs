using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCoin : MonoBehaviour
{
    public EnemyHealth EnemyHealth; // Referenz zum EnemyHealth-Skript
    public GameObject coinPrefab; // Prefab der Münze
    public int minCoins = 1; // Minimale Anzahl an Münzen
    public int maxCoins = 5; // Maximale Anzahl an Münzen
    public float dropRadius = 1.0f; // Radius, in dem die Münzen fallen können
    private bool hasDropped = false; // Flag, um sicherzustellen, dass Münzen nur einmal droppen

    // Wird aufgerufen, wenn der Gegner stirbt
    public void OnMonsterDeath()
    {
        if (hasDropped) return; // Verhindert mehrfaches Droppen
        hasDropped = true;

        // Berechne zufällige Anzahl an Münzen
        int numberOfCoins = Random.Range(minCoins, maxCoins + 1);

        // Generiere Münzen um die Position des Monsters
        for (int i = 0; i < numberOfCoins; i++)
        {
            DropCoinFunc();
        }
    }

    private void DropCoinFunc()
    {
        // Zufällige Position innerhalb eines Kreises
        Vector2 dropPosition = (Vector2)transform.position + Random.insideUnitCircle * dropRadius;

        // Erstelle die Münze an der berechneten Position
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
