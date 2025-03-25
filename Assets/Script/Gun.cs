using UnityEngine;

public class Gun : MonoBehaviour
{
    private float rotateOffSet = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.15f;
    private float nextShot;
    private bool isAutoFire = false;
    [SerializeField] private GameUI audioManager;
    void Update()
    {
        RotateGun();
        Shoot();
        ToggleFireMode();
    }

    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(-displacement.y, -displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffSet);
        if (angle < -90 || angle > 90)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
    }

    void Shoot()
    {
        if (isAutoFire)
        {
            if (Time.time > nextShot)
            {
                nextShot = Time.time + shotDelay;
                Quaternion reversedRotation = Quaternion.Euler(0, 0, firePos.rotation.eulerAngles.z + 180f);
                Instantiate(bulletPrefabs, firePos.position, reversedRotation);
                audioManager.playShootSound();
            }           
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && Time.time > nextShot)
            {
                nextShot = Time.time + shotDelay;
                Quaternion reversedRotation = Quaternion.Euler(0, 0, firePos.rotation.eulerAngles.z + 180f);
                Instantiate(bulletPrefabs, firePos.position, reversedRotation);
                audioManager.playShootSound();
            }
            
        }
    }

    void ToggleFireMode()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAutoFire = !isAutoFire;
            Debug.Log("Fire mode: " + (isAutoFire ? "Automatic" : "Single"));
            audioManager.playReLoadSound();
        }
        
    }
}