using UnityEngine;

public class HealEnemy : Enemy
{
    [SerializeField] private GameObject hpObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(enterDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(stayDamage);
        }
    }

    public override void Die()
    {
        if (hpObject != null)
        {
            GameObject hp = Instantiate(hpObject, transform.position, Quaternion.identity);
            Destroy(hp, 5f);
        }
        base.Die();
    }
}
