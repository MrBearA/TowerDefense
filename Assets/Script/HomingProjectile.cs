using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed = 20f; // How fast the missile moves
    public float rotateSpeed = 500f; // How quickly it turns toward the target
    public float lifetime = 5f; // Max time before destroying itself
    private Transform target;

    void Start()
    {
        Destroy(gameObject, lifetime);
        FindTarget();
    }

    void Update()
    {
        if (target == null)
        {
            FindTarget(); // Reacquire a target if lost
            if (target == null) return; // Still no target? Do nothing.
        }

        // Rotate towards target
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);

        // Move forward
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void FindTarget()
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

        target = closestEnemy;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Destroy the enemy
            Destroy(gameObject); // Destroy the missile
        }
    }
}
