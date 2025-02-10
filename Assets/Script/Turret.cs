using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject homingMissilePrefab;
    public Transform firePoint;

    public float turnSpeed = 10f;
    public float detectionRange = 10f; // Default range
    public float fireCooldown = 2f; // Default fire rate
    public float bulletSpeed = 20f; // Default bullet speed
    private bool canFire = true;

    public bool useHomingMissiles = false;

    void Update()
    {
        FindClosestEnemy();

        if (targetEnemy == null) return;

        Vector3 directionToEnemy = (targetEnemy.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime * 100f);

        if (Vector3.Distance(transform.position, targetEnemy.position) <= detectionRange && canFire)
        {
            FireProjectile();
        }
    }

    private Transform targetEnemy;

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        targetEnemy = closestEnemy;
    }

    void FireProjectile()
    {
        canFire = false;
        GameObject projectileToFire = useHomingMissiles ? homingMissilePrefab : projectilePrefab;
        GameObject projectile = Instantiate(projectileToFire, firePoint.position, firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        StartCoroutine(FireCooldownRoutine());
    }

    IEnumerator FireCooldownRoutine()
    {
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }

    // ✅ **Upgrade Functions**
    public void UpgradeFireRate()
    {
        fireCooldown = Mathf.Max(0.5f, fireCooldown - 0.3f); // Reduces cooldown but doesn't go below 0.5s
    }

    public void UpgradeRange()
    {
        detectionRange += 2f; // Increases detection range
    }

    public void UpgradeBulletDistance()
    {
        bulletSpeed += 5f; // Increases bullet speed so it travels farther
    }

    public void UnlockHomingMissiles()
    {
        useHomingMissiles = true;
    }
}
