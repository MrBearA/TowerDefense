using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Assign the Pause UI panel
    public float animationDuration = 0.3f; // Animation time
    public AnimationCurve easeCurve; // Easing for smooth animation
    private bool isPaused = false;
    private bool isAnimating = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isAnimating)
        {
            if (isPaused)
                ResumeGame(); // Resume game when ESC is pressed
            else
                StartCoroutine(AnimatePauseMenu(true)); // Open pause menu
        }
    }

    public void ResumeGame()
    {
        if (!isPaused || isAnimating) return;

        isPaused = false; // ✅ Mark as unpaused immediately
        StartCoroutine(AnimatePauseMenu(false)); // Start closing animation
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in Unity Editor
#else
            Application.Quit(); // Quits game in build
#endif
    }

    IEnumerator AnimatePauseMenu(bool show)
    {
        isAnimating = true;
        float elapsedTime = 0;
        float startScale = show ? 0 : 1;
        float endScale = show ? 1 : 0;

        if (show)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1; // ✅ Fix: Resume game **before animation ends**
        }

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // ✅ Fix: Ensure animation runs when paused
            float t = elapsedTime / animationDuration;
            float scale = Mathf.Lerp(startScale, endScale, easeCurve.Evaluate(t));
            pauseMenu.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        if (!show)
        {
            pauseMenu.SetActive(false);
        }

        isAnimating = false;

        if (show)
        {
            isPaused = true;
            Time.timeScale = 0; //  Pause game **only after animation completes**
        }
    }
}
