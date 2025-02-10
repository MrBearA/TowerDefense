using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Player movement speed
    public Transform turret; // Assign the turret in Inspector
    public GameObject upgradeUI; // Assign the Upgrade UI panel
    public float detectionRange = 3f; // Distance to show upgrade UI
    public float animationDuration = 0.3f; // Animation speed
    public AnimationCurve scaleCurve; // Ease-in/ease-out animation curve

    private bool isNearTurret = false;
    private Coroutine uiAnimationRoutine;

    void Update()
    {
        // Player movement using WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Check distance to turret
        float distanceToTurret = Vector3.Distance(transform.position, turret.position);
        bool shouldShowUI = distanceToTurret <= detectionRange;

        if (shouldShowUI && !isNearTurret)
        {
            isNearTurret = true;
            if (uiAnimationRoutine != null) StopCoroutine(uiAnimationRoutine);
            uiAnimationRoutine = StartCoroutine(AnimateUI(true)); // Show UI with animation
        }
        else if (!shouldShowUI && isNearTurret)
        {
            isNearTurret = false;
            if (uiAnimationRoutine != null) StopCoroutine(uiAnimationRoutine);
            uiAnimationRoutine = StartCoroutine(AnimateUI(false)); // Hide UI with animation
        }
    }

    IEnumerator AnimateUI(bool show)
    {
        float elapsedTime = 0;
        float startScale = show ? 0 : 1;
        float endScale = show ? 1 : 0;

        upgradeUI.SetActive(true); // Ensure UI is active before animation starts

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            float scale = Mathf.Lerp(startScale, endScale, scaleCurve.Evaluate(t));
            upgradeUI.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        if (!show) upgradeUI.SetActive(false); // Hide UI after animation completes
    }
}
