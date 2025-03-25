using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] protected float maxHp = 100f;
    protected float currentHp;
    [SerializeField] private Image hpBar;
    private Vector3 initialPosition;
    private GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Start()
    {
        currentHp = maxHp;
        initialPosition = transform.position;
        UpdateHpBar();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;
        if (playerInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (playerInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        if (playerInput != Vector2.zero)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0)
        {
            Die();
            gameManager.GameOver();
        }
    }

    private void Die()
    {
        // Không hủy player, chỉ vô hiệu hóa
        gameObject.SetActive(false);
        Debug.Log("Player died and deactivated.");
    }

    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    public void HpRe(float hp)
    {
        if (currentHp + hp > maxHp)
        {
            currentHp = maxHp;
        }
        else
        {
            currentHp += hp;
        }
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        currentHp = maxHp;
        UpdateHpBar();
        Debug.Log($"Player health reset to: {currentHp}");
    }

    public void ResetPosition()
    {
        // Hard-code vị trí ban đầu thay vì dùng initialPosition
        transform.position = new Vector3(0.043599f, 0.152565f, 0f);
        Debug.Log($"Player position reset to: {transform.position}");
    }
}