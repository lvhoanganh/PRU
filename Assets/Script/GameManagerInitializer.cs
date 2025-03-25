using UnityEngine;

public class GameManagerInitializer : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerPrefab; // Prefab của GameManager

    void Awake()
    {
        // Kiểm tra xem GameManager có tồn tại không
        if (GameManager.Instance == null)
        {
            // Nếu không, tạo một instance mới từ Prefab
            Instantiate(gameManagerPrefab);
            Debug.Log("GameManager created in scene: " + gameObject.scene.name);
        }
        else
        {
            Debug.Log("GameManager already exists in scene: " + gameObject.scene.name);
        }
    }
}