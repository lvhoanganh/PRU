using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float timeDestroy = 0.5f;
    [SerializeField] private float damageFirst = 10f;
    private float damageCurrent;
    [SerializeField] GameObject bloodPrefabs;

    void Start()
    {
        Destroy(gameObject, timeDestroy);
        // Lấy damage từ GameManager thay vì dùng damageFirst
        damageCurrent = GameManager.Instance != null ? GameManager.Instance.GetBulletDamage() : damageFirst;
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageCurrent); // Dùng damageCurrent đã lấy từ GameManager
                GameObject blood = Instantiate(bloodPrefabs, transform.position, Quaternion.identity);
                Destroy(blood, 1f);
            }
            Destroy(gameObject);
        }
        if (collision.CompareTag("Boss"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageCurrent); // Dùng damageCurrent đã lấy từ GameManager
                GameObject blood = Instantiate(bloodPrefabs, transform.position, Quaternion.identity);
                Destroy(blood, 1f);
            }
            Destroy(gameObject);
        }
    }

    public void DameIns(float a)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IncreaseBulletDamage(a); // Cập nhật damage toàn cục
            damageCurrent = GameManager.Instance.GetBulletDamage(); // Cập nhật cho instance hiện tại
        }
    }
}