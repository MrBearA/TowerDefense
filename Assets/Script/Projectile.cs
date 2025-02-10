using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 3f;

    private Transform target;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Reward Gold when enemy is killed
            GoldManager goldManager = FindObjectOfType<GoldManager>();
            if (goldManager != null)
            {
                goldManager.SpawnGoldCoinUI(other.transform.position);
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
