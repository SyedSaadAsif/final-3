using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnHeight = 1f;
    [SerializeField] private float spawnIntervalMin = 2f; // Minimum time between spawns
    [SerializeField] private float spawnIntervalMax = 3f; // Maximum time between spawns

    private float spawnAreaWidth;
    private float spawnAreaLength;

    private void Start()
    {
        // Get the plane's dimensions
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        spawnAreaWidth = meshRenderer.bounds.size.x;
        spawnAreaLength = meshRenderer.bounds.size.z;

        // Spawn the first enemy and start the spawn timer
        SpawnEnemy();
        StartCoroutine(SpawnEnemiesPeriodically());
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Debug.Log("Spawning enemy at position: " + spawnPosition);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.LookRotation(Vector3.forward));
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
        float z = Random.Range(-spawnAreaLength / 2, spawnAreaLength / 2);
        return new Vector3(transform.position.x + x, spawnHeight, transform.position.z + z);
    }

    private IEnumerator SpawnEnemiesPeriodically()
    {
        while (true)
        {
            float interval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(interval);
            SpawnEnemy();
        }
    }
}
