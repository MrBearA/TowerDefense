using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum MovementType { Quadratic, Cubic }
    private Transform target;
    private float t;
    private MovementType movementType;
    private EnemySpawner spawner;
    private Vector3 startPos;

    public float speed = 2f;

    public void Init(Transform targetLocation, MovementType type, EnemySpawner spawnerRef)
    {
        target = targetLocation;
        movementType = type;
        spawner = spawnerRef;
        startPos = transform.position;
    }

    void Update()
    {
        t += Time.deltaTime * speed / Vector3.Distance(startPos, target.position);

        if (movementType == MovementType.Quadratic)
        {
            transform.position = Vector3.Lerp(startPos, target.position, EaseInOutQuad(t));
        }
        else if (movementType == MovementType.Cubic)
        {
            transform.position = Vector3.Lerp(startPos, target.position, EaseInOutCubic(t));
        }

        if (t >= 1f)
        {
            spawner.TakeDamage();
            Destroy(gameObject);
        }
    }

    // ✅ Easing Functions for Smoother Motion
    private float EaseInOutQuad(float x)
    {
        return x < 0.5f ? 2f * x * x : 1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f;
    }

    private float EaseInOutCubic(float x)
    {
        return x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
    }
}
