using UnityEngine;
using System.Collections;

public class GoldCoinUI : MonoBehaviour
{
    private Transform targetUI;
    private GoldManager goldManager;
    public float dropHeight = 1f;
    public float dropDuration = 0.5f;
    public float waitBeforeMoving = 1f;
    public float moveDuration = 1f;

    private Vector3 startPos;
    private float elapsedTime;

    public void SetTarget(Transform target, GoldManager manager)
    {
        targetUI = target;
        goldManager = manager;
        startPos = transform.position;
        StartCoroutine(DropThenMove());
    }

    IEnumerator DropThenMove()
    {
        Vector3 dropTarget = startPos - new Vector3(0, dropHeight, 0);

        // Drop the coin with EaseOutQuad
        elapsedTime = 0;
        while (elapsedTime < dropDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / dropDuration;
            t = 1 - (1 - t) * (1 - t); // EaseOutQuad
            transform.position = Vector3.Lerp(startPos, dropTarget, t);
            yield return null;
        }

        // Wait before moving to UI
        yield return new WaitForSeconds(waitBeforeMoving);

        // Move towards gold counter with EaseInOutQuad
        elapsedTime = 0;
        Vector3 moveStart = transform.position;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            t = t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2; // EaseInOutQuad
            transform.position = Vector3.Lerp(moveStart, targetUI.position, t);
            yield return null;
        }

        // Add gold when it reaches the counter
        goldManager.AddGold(5);
        Destroy(gameObject);
    }
}
