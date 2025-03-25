using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private GameObject bulletPrefabs; // Prefab của đạn
    [SerializeField] private Transform firePoint; // Điểm bắn đạn
    [SerializeField] private float speedDanThuong = 20f; // Tốc độ đạn thường
    [SerializeField] private float skillCooldown = 2f; // Cooldown giữa các skill
    [SerializeField] private float teleportCooldown = 5f; // Cooldown cho dịch chuyển
    [SerializeField] private float healAmount = 200f; // Lượng máu hồi mỗi lần

    private float nextSkillTime = 0f;
    private float nextTeleportTime = 0f;

    protected override void Start()
    {
        base.Start();
        maxHp = 1000f;
        currentHp = maxHp;
        UpdateHpBar();
    }

    protected override void Update()
    {
        base.Update();
        if (Time.time >= nextSkillTime)
        {
            SuDungSkill();
        }
    }

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

    private void BanDanThuong()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - firePoint.position;
            directionToPlayer.Normalize();
            GameObject bullet = Instantiate(bulletPrefabs, firePoint.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
            enemyBullet.SetMovementDirection(directionToPlayer * speedDanThuong);
            Debug.Log("Boss used BanDanThuong!");
        }
    }

    private void HoiMau()
    {
        // Hồi máu
        currentHp += healAmount;

        // Đảm bảo máu không vượt quá maxHp
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        // Cập nhật thanh máu
        UpdateHpBar();

        Debug.Log($"Boss healed! Current HP: {currentHp}/{maxHp}");
    }

    private void DichChuyen()
    {
        if (player == null) return; // Kiểm tra nếu player không tồn tại

        if (Time.time >= nextTeleportTime)
        {
            // Dịch chuyển tức thời đến vị trí của player
            transform.position = player.transform.position;

            // Cập nhật thời gian cho lần dịch chuyển tiếp theo
            nextTeleportTime = Time.time + teleportCooldown;

            Debug.Log("Boss teleported to player!");
        }
    }

    private void SuDungSkill()
    {
        nextSkillTime = Time.time + skillCooldown;
        // Chọn ngẫu nhiên một skill
        int randomSkill = Random.Range(0, 3); // Chọn ngẫu nhiên từ 0 đến 2 (3 skill)
        switch (randomSkill)
        {
            case 0:
                BanDanThuong();
                break;
            case 1:
                HoiMau();
                break;
            case 2:
                DichChuyen();
                break;
        }
    }

    public void IncreaseHealth(float amount)
    {
        maxHp += amount;
        currentHp = maxHp;
        UpdateHpBar();
        Debug.Log($"Boss health increased to: {maxHp}");
    }

    public void ResetHealth()
    {
        currentHp = maxHp;
        UpdateHpBar();
        Debug.Log($"Boss health reset to: {currentHp}");
    }

    public override void Die()
    {
        base.Die();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NextWave();
        }
    }
}