using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    protected float enemyMoveSpeed = 2.0f;
    protected Player player;
    [SerializeField] protected float maxHp = 10f; // Máu mặc định cho enemy thường
    protected float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] protected float enterDamage = 10f;
    protected float stayDamage = 0.1f;

    protected virtual void Start()
    {
        // Tìm player, sử dụng API cho Unity 2022 hoặc cũ hơn
        player = Object.FindFirstObjectByType<Player>(FindObjectsInactive.Exclude);

        // Chỉ lấy maxHp từ GameManager nếu không phải là boss
        if (GameManager.Instance != null && !gameObject.CompareTag("Boss"))
        {
            maxHp = GameManager.Instance.GetEnemyHp();
        }

        currentHp = maxHp;
        UpdateHpBar();
    }

    protected virtual void Update()
    {
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyMoveSpeed * Time.deltaTime);
            FlipEnemy();
        }
    }

    protected void FlipEnemy()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (gameObject.CompareTag("Boss"))
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}