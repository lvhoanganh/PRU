using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject basicEnemy;      // Enemy chung cho cả hai map
    [SerializeField] private GameObject energyEnemy;     // Enemy cho Map 1
    [SerializeField] private GameObject healEnemy;       // Enemy cho Map 2
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 0.5f;
    private Coroutine spawnCoroutine;
    private MenuManager menuManager; // Tham chiếu đến MenuManager để kiểm tra map

    void Awake()
    {
        // Tìm MenuManager trong scene
        menuManager = FindObjectOfType<MenuManager>();
        if (menuManager == null)
        {
            Debug.LogError("MenuManager not found in the scene!");
        }
    }

    void OnEnable()
    {
        Debug.Log("EnemySpawner OnEnable: Starting coroutine...");
        StartSpawn();
    }

    void OnDisable()
    {
        StopSpawn();
    }

    public void StartSpawn()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnEnemyCoroutine());
        }
    }

    public void StopSpawn()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
            Debug.Log("EnemySpawner: Stopped spawning enemies.");
        }
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            if (gameObject.activeInHierarchy)
            {
                Debug.Log("EnemySpawner: Attempting to spawn enemy...");

                // Kiểm tra spawn points
                if (spawnPoints == null || spawnPoints.Length == 0)
                {
                    Debug.LogError("SpawnPoints array is empty or null in EnemySpawner!");
                    yield break;
                }
                foreach (var point in spawnPoints)
                {
                    if (point == null)
                    {
                        Debug.LogError("A spawn point in SpawnPoints array is null in EnemySpawner!");
                        yield break;
                    }
                }

                // Kiểm tra xem có map nào active không
                if (menuManager != null && !menuManager.IsMap1Active() && !menuManager.IsMap2Active())
                {
                    Debug.Log("No map is active, skipping spawn...");
                    continue; // Bỏ qua lần spawn này, chờ đến khi có map active
                }

                // Chọn enemy dựa trên map đang active
                GameObject enemyToSpawn = null;
                if (menuManager != null)
                {
                    if (menuManager.IsMap1Active())
                    {
                        // Map 1: Spawn BasicEnemy hoặc EnergyEnemy
                        enemyToSpawn = Random.value > 0.5f ? basicEnemy : energyEnemy;
                    }
                    else if (menuManager.IsMap2Active())
                    {
                        // Map 2: Spawn BasicEnemy hoặc HealEnemy
                        enemyToSpawn = Random.value > 0.5f ? basicEnemy : healEnemy;
                    }
                }

                if (enemyToSpawn == null)
                {
                    Debug.LogError("No enemy to spawn! Check if a map is active or enemy prefabs are assigned.");
                    yield break;
                }

                // Spawn enemy
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
                Debug.Log($"Spawned enemy {spawnedEnemy.name} at position: {spawnPoint.position}");
            }
        }
    }
}