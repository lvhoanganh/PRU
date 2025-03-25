using UnityEngine;

public class PlayerCollections : MonoBehaviour
{
    private float timer = 0f;
    private bool isCollidingWithBlock = false;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameUI audioManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = GetComponent<Player>();
        if (collision.CompareTag("EnemyBullet"))
        {
            player.TakeDamage(10f);
        }
        else if (collision.CompareTag("Energy"))
        {
            gameManager.AddEnergy();
            Destroy(collision.gameObject);
            audioManager.playEnergySound();
        }
        else if (collision.CompareTag("hp"))
        {
            player.HpRe(5);
            gameManager.AddEnergy();
            gameManager.IncreaseBulletDamage(2);
            Destroy(collision.gameObject);
            audioManager.playEnergySound();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            isCollidingWithBlock = true;
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                gameManager.CallBoss();
                timer = 0f;
            }
        }
    }

}
