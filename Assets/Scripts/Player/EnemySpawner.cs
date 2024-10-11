using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public Tilemap groundTilemap; 
    public float spawnRadius = 10f; 
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 10f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // Gegner an einem random Punkt im Radius spawnen
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (IsPositionOnGround(spawnPosition))
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    // random Position innerhalb des Radius bestimmen
    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPos.x, randomPos.y, 0) + transform.position;
        return spawnPosition;
    }

    // checken, ob die zufällige Position auf der "Ground"-Tilemap ist
    bool IsPositionOnGround(Vector3 position)
    {
        Vector3Int tilePosition = groundTilemap.WorldToCell(position);

        TileBase tile = groundTilemap.GetTile(tilePosition);

        return tile != null;
    }
}