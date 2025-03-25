using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject playMenu;    // Menu khởi động (chứa nút Play)
    [SerializeField] GameObject mapSelectionMenu1; // Nút Map 1
    [SerializeField] GameObject mapSelectionMenu2; // Nút Map 2
    [SerializeField] GameObject map1;        // GameObject cho Map 1
    [SerializeField] GameObject map2;        // GameObject cho Map 2
    [SerializeField] GameObject gameMenu;    // Container chính của menu
    [SerializeField] EnemySpawner enemySpawner; // Tham chiếu đến EnemySpawner
    [SerializeField] GameObject player;      // Tham chiếu trực tiếp đến Player
    void Start()
    {
        // Ban đầu chỉ hiển thị menu khởi động
        playMenu.SetActive(true);
        mapSelectionMenu1.SetActive(false);
        mapSelectionMenu2.SetActive(false);
        map1.SetActive(false);
        map2.SetActive(false);
        gameMenu.SetActive(true);
        
        // Tắt EnemySpawner lúc đầu
        if (enemySpawner != null)
        {
            enemySpawner.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("EnemySpawner is not assigned in MenuManager! Attempting to find it...");
            enemySpawner = FindObjectOfType<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.gameObject.SetActive(false);
                Debug.Log("EnemySpawner found and deactivated at start.");
            }
            else
            {
                Debug.LogError("EnemySpawner not found in the scene!");
            }
        }

        // Tắt Player lúc đầu
        if (player != null)
        {
            player.SetActive(false);
            Debug.Log("Player deactivated at start.");
        }
        else
        {
            Debug.LogWarning("Player is not assigned in MenuManager! Attempting to find it...");
            Player playerComponent = FindObjectOfType<Player>();
            if (playerComponent != null)
            {
                player = playerComponent.gameObject;
                player.SetActive(false);
                Debug.Log("Player found and deactivated at start.");
            }
            else
            {
                Debug.LogError("Player not found in the scene!");
            }
        }
    }

    public void PlayGame()
    {
        // Ẩn menu khởi động và hiển thị menu chọn map
        playMenu.SetActive(false);
        mapSelectionMenu1.SetActive(true);
        mapSelectionMenu2.SetActive(true);
    }

    public void PlayMap1()
    {
        mapSelectionMenu1.SetActive(false);
        mapSelectionMenu2.SetActive(false);
        gameMenu.SetActive(false);
        map1.SetActive(true);
        map2.SetActive(false);
        // Bật Player khi chơi Map 1
        if (player != null)
        {
            player.SetActive(true);
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.ResetHealth();
                // Bỏ gọi ResetPosition() để giữ vị trí đã thiết lập trong scene
                Debug.Log("Player activated for Map 1 at scene position.");
            }
        }
        else
        {
            Debug.LogWarning("Player is not assigned in MenuManager! Attempting to find it...");
            Player playerComponent = FindObjectOfType<Player>();
            if (playerComponent != null)
            {
                player = playerComponent.gameObject;
                player.SetActive(true);
                playerComponent.ResetHealth();
                // Bỏ gọi ResetPosition() để giữ vị trí đã thiết lập trong scene
                Debug.Log("Player found and activated for Map 1 at scene position.");
            }
            else
            {
                Debug.LogError("Player not found in the scene!");
            }
        }

        // Bật UI của map
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.EnableMapUI();
        }

        // Bật EnemySpawner khi chọn Map 1
        if (enemySpawner != null)
        {
            enemySpawner.gameObject.SetActive(true);
            enemySpawner.StartSpawn();
        }
        else
        {
            Debug.LogWarning("EnemySpawner is not assigned in MenuManager! Attempting to find it...");
            enemySpawner = FindObjectOfType<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.gameObject.SetActive(true);
                enemySpawner.StartSpawn();
                Debug.Log("EnemySpawner found and activated for Map 1.");
            }
            else
            {
                Debug.LogError("EnemySpawner not found in the scene!");
            }
        }
    }

    public void PlayMap2()
    {
        mapSelectionMenu1.SetActive(false);
        mapSelectionMenu2.SetActive(false);
        gameMenu.SetActive(false);
        map1.SetActive(false);
        map2.SetActive(true);
        // Bật Player khi chơi Map 2
        if (player != null)
        {
            player.SetActive(true);
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.ResetHealth();
                // Bỏ gọi ResetPosition() để giữ vị trí đã thiết lập trong scene
                Debug.Log("Player activated for Map 2 at scene position.");
            }
        }
        else
        {
            Debug.LogWarning("Player is not assigned in MenuManager! Attempting to find it...");
            Player playerComponent = FindObjectOfType<Player>();
            if (playerComponent != null)
            {
                player = playerComponent.gameObject;
                player.SetActive(true);
                playerComponent.ResetHealth();
                // Bỏ gọi ResetPosition() để giữ vị trí đã thiết lập trong scene
                Debug.Log("Player found and activated for Map 2 at scene position.");
            }
            else
            {
                Debug.LogError("Player not found in the scene!");
            }
        }

        // Bật UI của map
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.EnableMapUI();
        }
        // Bật EnemySpawner khi chọn Map 2
        if (enemySpawner != null)
        {
            enemySpawner.gameObject.SetActive(true);
            enemySpawner.StartSpawn();
        }
        else
        {
            Debug.LogWarning("EnemySpawner is not assigned in MenuManager! Attempting to find it...");
            enemySpawner = FindObjectOfType<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.gameObject.SetActive(true);
                enemySpawner.StartSpawn();
                Debug.Log("EnemySpawner found and activated for Map 2.");
            }
            else
            {
                Debug.LogError("EnemySpawner not found in the scene!");
            }
        }
    }

    // Hàm để kiểm tra Map 1 có đang active không
    public bool IsMap1Active()
    {
        return map1.activeInHierarchy;
    }

    // Hàm để kiểm tra Map 2 có đang active không
    public bool IsMap2Active()
    {
        return map2.activeInHierarchy;
    }

    public void backToMenu()
    {
        playMenu.SetActive(true);
        mapSelectionMenu1.SetActive(false);
        mapSelectionMenu2.SetActive(false);
        map1.SetActive(false);
        map2.SetActive(false);
        gameMenu.SetActive(true);
        // Tắt EnemySpawner khi quay lại menu
        if (enemySpawner != null)
        {
            enemySpawner.gameObject.SetActive(false);
            Debug.Log("EnemySpawner disabled when returning to menu.");
        }
        else
        {
            Debug.LogWarning("EnemySpawner is not assigned in MenuManager! Attempting to find it...");
            enemySpawner = FindObjectOfType<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.gameObject.SetActive(false);
                Debug.Log("EnemySpawner found and deactivated when returning to menu.");
            }
            else
            {
                Debug.LogError("EnemySpawner not found in the scene!");
            }
        }

        // Tắt Player khi quay lại menu
        if (player != null)
        {
            player.SetActive(false);
            Debug.Log("Player deactivated when returning to menu.");
        }
        else
        {
            Debug.LogWarning("Player is not assigned in MenuManager! Attempting to find it...");
            Player playerComponent = FindObjectOfType<Player>();
            if (playerComponent != null)
            {
                player = playerComponent.gameObject;
                player.SetActive(false);
                Debug.Log("Player found and deactivated when returning to menu.");
            }
            else
            {
                Debug.LogError("Player not found in the scene!");
            }
        }
    }

    // Hàm để lấy Player từ MenuManager
    public GameObject GetPlayer()
    {
        if (player != null)
        {
            return player;
        }
        else
        {
            Debug.LogWarning("Player is not assigned in MenuManager! Attempting to find it...");
            Player playerComponent = FindObjectOfType<Player>();
            if (playerComponent != null)
            {
                player = playerComponent.gameObject;
                return player;
            }
            else
            {
                Debug.LogError("Player not found in the scene!");
                return null;
            }
        }
    }
}