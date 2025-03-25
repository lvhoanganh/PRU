using UnityEngine;

public class BombCollision : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab; // Prefab hiệu ứng nổ (Explosion)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Va chạm với một quả bom khác
        if (collision.CompareTag("Bomb"))
        {
            // Tạo hiệu ứng nổ tại vị trí của quả bom này
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                // Tự hủy hiệu ứng nổ sau một thời gian (ví dụ: 1 giây)
                Destroy(explosion, 1f);
            }

            // Hủy quả bom khác
            Destroy(collision.gameObject);

            // Hủy quả bom này
            Destroy(gameObject);
        }
        // Va chạm với enemy
        else if (collision.CompareTag("Enemy"))
        {
            // Tạo hiệu ứng nổ
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                // Tự hủy hiệu ứng nổ sau một thời gian
                Destroy(explosion, 1f);
            }

            // Hủy quả bom
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Boss"))
        {
            // Tạo hiệu ứng nổ
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                // Tự hủy hiệu ứng nổ sau một thời gian
                Destroy(explosion, 1f);
            }

            // Hủy quả bom
            Destroy(gameObject);
        }
    }
}