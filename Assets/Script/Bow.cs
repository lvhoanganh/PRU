using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab; // Prefab của quả bom (Bow)
    [SerializeField] private GameObject explosionPrefab; // Prefab của hiệu ứng nổ (Explosion)
    [SerializeField] private float placeDelay = 0.5f; // Thời gian chờ giữa các lần đặt bom
    private float nextPlacement; // Thời gian cho lần đặt bom tiếp theo
    private int currentBombs; // Số lượng bom hiện tại
    private GameManager gameManager; // Tham chiếu đến GameManager

    void Start()
    {
        currentBombs = 0;
        gameManager = GameManager.Instance; // Lấy Singleton instance
        if (gameManager == null)
        {
            Debug.LogError("GameManager không được tìm thấy!");
        }
    }

    void Update()
    {
        PlaceBomb(); // Đặt bom tại vị trí chuột
        currentBombs = gameManager.getCurrentEnergy(); // Nạp lại số lượng bom
    }

    void PlaceBomb()
    {
        // Kiểm tra nếu nhấn phím E, còn bom và đã đủ thời gian chờ
        if (Input.GetKeyDown(KeyCode.E) && currentBombs > 0 && Time.time > nextPlacement)
        {
            nextPlacement = Time.time + placeDelay;

            // Lấy vị trí chuột
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0f;

            // Đặt quả bom tại vị trí chuột
            GameObject bomb = Instantiate(bombPrefab, worldPosition, Quaternion.identity);
            bomb.tag = "Bomb"; // Gán Tag "Bomb"

            // Tự hủy quả bom sau 10 giây nếu không va chạm
            Destroy(bomb, 10f);

            currentBombs--; // Giảm số lượng bom
            gameManager.SubEnergy(); // Giảm năng lượng sau khi đặt bom
        }
    }
}