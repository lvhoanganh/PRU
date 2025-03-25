using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 100f;
    private GameUI gameUI;
    void Awake()
    {
        // Tìm GameUI trong scene
        gameUI = FindObjectOfType<GameUI>();
        if (gameUI == null)
        {
            Debug.LogWarning("GameUI not found in the scene!");
        }
    }
    void Start()
    {
        if (gameUI != null)
        {
            gameUI.playExSound();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (collision.CompareTag("Enemy"))
        {
            enemy.TakeDamage(damage);
            GameUI audioManager = gameObject.GetComponent<GameUI>();
            audioManager.playExSound();
        }
        if (collision.CompareTag("Boss"))
        {
            enemy.TakeDamage(damage);
            GameUI audioManager = gameObject.GetComponent<GameUI>();
            audioManager.playExSound();
        }
    }

    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
