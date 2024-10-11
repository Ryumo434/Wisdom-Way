using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    // Variablen, die du im Editor einstellen kannst
    public GameObject enemyPrefab; // Das Prefab des Gegners
    public Tilemap groundTilemap; // Die Ground-Tilemap
    public float spawnRadius = 10f; // Der Radius, innerhalb dessen die Gegner spawnen
    public float minSpawnTime = 1f; // Minimale Zeit bis zum nächsten Spawn
    public float maxSpawnTime = 10f; // Maximale Zeit bis zum nächsten Spawn

    private void Start()
    {
        // Coroutine für das wiederholte Spawnen von Gegnern starten
        StartCoroutine(SpawnEnemies());
    }

    // Coroutine für den Spawn-Prozess
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Zufällige Zeit zwischen minSpawnTime und maxSpawnTime warten
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // Gegner an einem zufälligen Punkt im Radius spawnen
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (IsPositionOnGround(spawnPosition))
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    // Zufällige Position innerhalb des angegebenen Radius bestimmen
    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPos.x, randomPos.y, 0) + transform.position;
        return spawnPosition;
    }

    // Prüfen, ob die zufällige Position auf der "Ground"-Tilemap ist
    bool IsPositionOnGround(Vector3 position)
    {
        // Die Position auf die nächste Tilemap-Zelle runden
        Vector3Int tilePosition = groundTilemap.WorldToCell(position);

        // Überprüfen, ob an dieser Position auf der "Ground"-Tilemap eine Tile vorhanden ist
        TileBase tile = groundTilemap.GetTile(tilePosition);

        // Wenn eine Tile existiert, ist es eine gültige Position
        return tile != null;
    }
}