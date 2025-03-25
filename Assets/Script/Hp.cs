using UnityEngine;

public class Hp : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerBullet playerBullet;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = GetComponent<Player>();
        if (collision.CompareTag("EnemyBullet") && player != null)
        {
            player.TakeDamage(10f);
        }
        else if (collision.CompareTag("Energy") && player != null && gameManager != null)
        {
            player.HpRe(5);
            gameManager.AddEnergy();
            gameManager.IncreaseBulletDamage(2);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerStay2D called with: {collision.gameObject.name}, Tag: {collision.tag}");

        if (collision.CompareTag("Block") && gameManager != null)
        {
            Debug.Log("Colliding with Block, incrementing timer...");
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                Debug.Log("Calling CallBoss...");
                gameManager.CallBoss();
                timer = 0f;
            }
        }
        else
        {
            if (!collision.CompareTag("Block"))
            {
                Debug.LogWarning($"Collision object {collision.gameObject.name} does not have tag 'Block'.");
            }
            if (gameManager == null)
            {
                Debug.LogWarning("GameManager is not assigned in Hp!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            timer = 0f;
        }
    }
}