using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    // public static EnemySpawner Instance;
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public int maxEnemies = 7;
    public float spawnInterval = 2.5f;

    private int currentEnemies = 0;

    private void Start()
    {
        // if(Instance != null && Instance != this)
        // {
        //     Destroy(this.gameObject);
        // }
        // else
        // {
        //     Instance = this;
        // }

        Debug.Log("Spawn points count: " + spawnPoints.Length);
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentEnemies < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        currentEnemies++;
        enemy.GetComponent<EnemyDisplay>().OnEnemyDie += () =>
        {
            currentEnemies--;
        };
    }
}
