using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int currentEnergy = 0;
#pragma warning disable CS0414
    private bool bossCalled = false;
#pragma warning restore CS0414
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject enemySpawner;
    private float bulletDamage = 10f;
    private int currentWave = 1;
    private float baseEnemyHp = 10f;
    private float bossHealthIncreasePerWave = 100f;
    [SerializeField] private TextMeshProUGUI countBoom;
    [SerializeField] private TextMeshProUGUI countWave;
    [SerializeField] private TextMeshProUGUI countWaveIn;
    [SerializeField] private GameObject gameOverUi;
    [SerializeField] private GameUI audioManager;
    [SerializeField] private GameObject pause;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (boss != null)
        {
            boss.SetActive(false);
        }
        ResetGame();
        UpdateCountBoom();
        UpdateCountWave();
        UpdateCountWaveIn();
        pause.SetActive(false);
        audioManager.playDefaultAudio();
        if (gameOverUi != null)
        {
            gameOverUi.SetActive(false);
        }
    }

    public void AddEnergy()
    {
        currentEnergy += 1;
        UpdateCountBoom();
    }

    public int getCurrentEnergy()
    {
        return currentEnergy;
    }

    public void SubEnergy()
    {
        currentEnergy -= 1;
        UpdateCountBoom();
    }

    public void CallBoss()
    {
        if (!bossCalled)
        {
            bossCalled = true;

            Debug.Log("Calling boss and destroying enemies...");

            if (enemySpawner != null)
            {
                enemySpawner.SetActive(false);
                Debug.Log("Enemy spawner disabled.");
            }
            else
            {
                Debug.LogWarning("EnemySpawner is not assigned in GameManager!");
            }

            Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            Debug.Log($"Found {allEnemies.Length} enemies in the scene.");

            foreach (Enemy enemy in allEnemies)
            {
                if (enemy.gameObject != boss)
                {
                    Debug.Log($"Destroying enemy: {enemy.gameObject.name}");
                    enemy.Die();
                }
                else
                {
                    Debug.Log($"Skipping boss: {enemy.gameObject.name}");
                }
            }

            if (boss != null)
            {
                boss.SetActive(true);
                audioManager.playBossAudio();
                Debug.Log("Boss activated.");
            }
            else
            {
                Debug.LogWarning("Boss is not assigned in GameManager!");
            }
        }
    }

    public void IncreaseBulletDamage(float amount)
    {
        bulletDamage += amount;
    }

    public float GetBulletDamage()
    {
        return bulletDamage;
    }

    public float GetEnemyHp()
    {
        return baseEnemyHp;
    }

    public void NextWave()
    {
        currentWave++;
        Debug.Log($"Starting Wave {currentWave}");
        UpdateCountWave();
        UpdateCountWaveIn();
        baseEnemyHp += 50f;
        Debug.Log($"Enemy HP increased to: {baseEnemyHp}");

        // Hồi máu cho Player khi sang wave mới
        Player player = Object.FindFirstObjectByType<Player>(FindObjectsInactive.Exclude);
        if (player != null)
        {
            player.gameObject.SetActive(true); // Đảm bảo Player được bật để có thể đặt lại vị trí
            player.ResetPosition();
            Debug.Log("Player position reset to initial position on game over.");
            player.ResetHealth();
            Debug.Log("Player health reset for new wave.");
        }
        else
        {
            Debug.LogWarning("Player not found in the scene!");
        }

        if (boss != null)
        {
            BossEnemy bossEnemy = boss.GetComponent<BossEnemy>();
            if (bossEnemy != null)
            {
                bossEnemy.IncreaseHealth(bossHealthIncreasePerWave);
                bossEnemy.ResetHealth();
            }
            else
            {
                Debug.LogWarning("BossEnemy component not found on boss!");
            }
        }
        else
        {
            Debug.LogWarning("Boss is not assigned in GameManager!");
        }
        audioManager.playDefaultAudio();
        ResetGame();
    }

    private void ResetGame()
    {
        Player player = Object.FindFirstObjectByType<Player>(FindObjectsInactive.Include);
        if (player != null)
        {
            player.gameObject.SetActive(true); // Đảm bảo Player được bật để có thể đặt lại vị trí
            player.ResetPosition();
            Debug.Log("Player position reset to initial position on game over.");
        }
        Debug.Log("Resetting game for new wave...");

        currentEnergy = 0;
        bulletDamage = 10f;
        bossCalled = false;

        if (boss != null)
        {
            boss.SetActive(false);
            Debug.Log("Boss deactivated.");
        }

        if (enemySpawner != null)
        {
            enemySpawner.SetActive(true);
            Debug.Log("Enemy spawner enabled.");
        }

        Enemy[] remainingEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in remainingEnemies)
        {
            enemy.Die();
        }
        Debug.Log($"Cleared {remainingEnemies.Length} remaining enemies.");
    }

    private void UpdateCountBoom()
    {
        if (countBoom != null)
        {
            countBoom.text = currentEnergy.ToString();
        }
    }

    private void UpdateCountWave()
    {
        if (countWave != null)
        {
            countWave.text = currentWave.ToString();
        }
    }

    private void UpdateCountWaveIn()
    {
        if (countWaveIn != null)
        {
            countWaveIn.text = currentWave.ToString();
        }
    }

    public void EnableMapUI()
    {
        if (countBoom != null)
        {
            countBoom.gameObject.SetActive(true);
            UpdateCountBoom();
        }
        if (countWave != null)
        {
            countWave.gameObject.SetActive(true);
            UpdateCountWave();
        }
        if (countWaveIn != null)
        {
            countWaveIn.gameObject.SetActive(true);
            UpdateCountWaveIn();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;

        // Đặt lại vị trí của Player ngay khi game over
        Player player = Object.FindFirstObjectByType<Player>(FindObjectsInactive.Include);
        if (player != null)
        {
            player.gameObject.SetActive(true); // Đảm bảo Player được bật để có thể đặt lại vị trí
            player.ResetPosition();
            Debug.Log("Player position reset to initial position on game over.");
            player.gameObject.SetActive(false); // Tắt lại Player sau khi đặt lại vị trí
        }
        else
        {
            Debug.LogWarning("Player not found in the scene! Attempting to find in MenuManager...");
            MenuManager menuManager = FindObjectOfType<MenuManager>();
            if (menuManager != null)
            {
                GameObject playerObject = menuManager.GetPlayer();
                if (playerObject != null)
                {
                    player = playerObject.GetComponent<Player>();
                    if (player != null)
                    {
                        player.gameObject.SetActive(true);
                        player.ResetPosition();
                        Debug.Log("Player found via MenuManager, position reset on game over.");
                        player.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                Debug.LogError("Player not found in the scene and MenuManager not found!");
            }
        }

        // Hiển thị UI game over
        if (gameOverUi != null)
        {
            gameOverUi.SetActive(true);
        }
        else
        {
            Debug.LogWarning("GameOver UI is not assigned in GameManager!");
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");

        // Đặt lại các biến trạng thái, nhưng giữ currentWave
        isGameOver = false;
        currentEnergy = 0;
        bulletDamage = 10f;
        baseEnemyHp = 10f; // Reset HP của enemy về giá trị ban đầu
        bossCalled = false;

        // Đặt lại Time.timeScale
        Time.timeScale = 1;

        // Ẩn UI Game Over
        if (gameOverUi != null)
        {
            gameOverUi.SetActive(false);
            Debug.Log("Game Over UI hidden.");
        }
        else
        {
            Debug.LogWarning("GameOver UI is not assigned in GameManager!");
        }

        // Đặt lại trạng thái của boss
        if (boss != null)
        {
            boss.SetActive(false);
            BossEnemy bossEnemy = boss.GetComponent<BossEnemy>();
            if (bossEnemy != null)
            {
                bossEnemy.ResetHealth();
                Debug.Log("Boss health reset.");
            }
        }
        else
        {
            Debug.LogWarning("Boss is not assigned in GameManager!");
        }

        // Đặt lại trạng thái của enemy spawner
        if (enemySpawner != null)
        {
            EnemySpawner spawner = enemySpawner.GetComponent<EnemySpawner>();
            if (spawner != null)
            {
                spawner.StopSpawn(); // Dừng sinh enemy
                enemySpawner.SetActive(true); // Kích hoạt lại spawner
                spawner.StartSpawn(); // Khởi động lại coroutine
                Debug.Log("Enemy spawner reset and restarted.");
            }
        }
        else
        {
            Debug.LogWarning("EnemySpawner is not assigned in GameManager!");
        }

        // Xóa tất cả enemy còn lại
        Enemy[] remainingEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in remainingEnemies)
        {
            enemy.Die();
        }
        Debug.Log($"Cleared {remainingEnemies.Length} remaining enemies.");

        // Xóa các object Energy và Bomb còn lại
        GameObject[] energyObjects = GameObject.FindGameObjectsWithTag("Energy");
        foreach (GameObject energy in energyObjects)
        {
            Destroy(energy);
        }

        GameObject[] bombObjects = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject bomb in bombObjects)
        {
            Destroy(bomb);
        }

        GameObject[] hpObjects = GameObject.FindGameObjectsWithTag("hp");
        foreach (GameObject hp in hpObjects)
        {
            Destroy(hp);
        }

        // Hồi máu và đưa Player về vị trí ban đầu
        Player player = Object.FindFirstObjectByType<Player>(FindObjectsInactive.Include);
        if (player != null)
        {
            player.gameObject.SetActive(true); // Đảm bảo Player được bật
            player.ResetHealth();
            player.ResetPosition();
            Debug.Log("Player activated, health and position reset for restart.");
        }
        else
        {
            Debug.LogWarning("Player not found in the scene! Attempting to find in MenuManager...");
            MenuManager menuManager = FindObjectOfType<MenuManager>();
            if (menuManager != null)
            {
                GameObject playerObject = menuManager.GetPlayer();
                if (playerObject != null)
                {
                    player = playerObject.GetComponent<Player>();
                    if (player != null)
                    {
                        player.gameObject.SetActive(true);
                        player.ResetHealth();
                        player.ResetPosition();
                        Debug.Log("Player found via MenuManager, activated, health and position reset for restart.");
                    }
                }
            }
            else
            {
                Debug.LogError("Player not found in the scene and MenuManager not found!");
            }
        }

        // Bật lại UI của map
        EnableMapUI();

        // Cập nhật UI
        UpdateCountBoom();
        UpdateCountWave();
        UpdateCountWaveIn();
    }

    public void BackToMenu()
    {
        Debug.Log("Returning to menu...");
        pause.SetActive(false);
        // Đặt lại các biến trạng thái
        isGameOver = false;
        currentEnergy = 0;
        bulletDamage = 10f;
        currentWave = 1; // Reset wave về 1 khi quay lại menu
        baseEnemyHp = 10f;
        bossCalled = false;

        // Đặt lại Time.timeScale
        Time.timeScale = 1;

        // Ẩn UI Game Over
        if (gameOverUi != null)
        {
            gameOverUi.SetActive(false);
            Debug.Log("Game Over UI hidden.");
        }

        // Ẩn các UI element của map
        if (countBoom != null) countBoom.gameObject.SetActive(false);
        if (countWave != null) countWave.gameObject.SetActive(false);
        if (countWaveIn != null) countWaveIn.gameObject.SetActive(false);

        // Đặt lại trạng thái của boss
        if (boss != null)
        {
            boss.SetActive(false);
            BossEnemy bossEnemy = boss.GetComponent<BossEnemy>();
            if (bossEnemy != null)
            {
                bossEnemy.ResetHealth();
                Debug.Log("Boss health reset.");
            }
        }
        else
        {
            Debug.LogWarning("Boss is not assigned in GameManager!");
        }

        // Đặt lại trạng thái của enemy spawner
        if (enemySpawner != null)
        {
            EnemySpawner spawner = enemySpawner.GetComponent<EnemySpawner>();
            if (spawner != null)
            {
                spawner.StopSpawn(); // Dừng sinh enemy
                enemySpawner.SetActive(false); // Tắt spawner
                Debug.Log("Enemy spawner stopped and disabled.");
            }
        }
        else
        {
            Debug.LogWarning("EnemySpawner is not assigned in GameManager!");
        }

        // Xóa tất cả enemy còn lại
        Enemy[] remainingEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in remainingEnemies)
        {
            enemy.Die();
        }
        Debug.Log($"Cleared {remainingEnemies.Length} remaining enemies.");

        // Xóa các object Energy và Bomb còn lại
        GameObject[] energyObjects = GameObject.FindGameObjectsWithTag("Energy");
        foreach (GameObject energy in energyObjects)
        {
            Destroy(energy);
        }

        GameObject[] bombObjects = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject bomb in bombObjects)
        {
            Destroy(bomb);
        }

        GameObject[] hpObjects = GameObject.FindGameObjectsWithTag("hp");
        foreach (GameObject hp in hpObjects)
        {
            Destroy(hp);
        }

        // Tìm MenuManager và gọi backToMenu()
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        if (menuManager != null)
        {
            menuManager.backToMenu(); // Gọi backToMenu để quay lại menu khởi động
            Debug.Log("MenuManager found, returning to menu.");
        }
        else
        {
            Debug.LogWarning("MenuManager not found in the scene! Cannot return to menu.");
        }

        // Cập nhật UI (đảm bảo các UI element không hiển thị)
        UpdateCountBoom();
        UpdateCountWave();
        UpdateCountWaveIn();
    }

    public bool getBoss()
    {
        return bossCalled;
    }

    public void PauseGameMenu()
    {
        pause.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pause.SetActive(false);
        Time.timeScale = 1f;
    }
}