using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 100f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (collision.CompareTag("Enemy"))
        {
            enemy.TakeDamage(damage);
        }
        if (collision.CompareTag("Boss"))
        {
            enemy.TakeDamage(damage);
        }
    }

    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
