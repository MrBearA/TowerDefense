using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPointQuadratic;
    public Transform spawnPointCubic;
    public Transform targetLocation;
    public GameObject enemyPrefab;

    public float spawnInterval = 5f; // Time delay before spawning next enemy
    public int playerHP = 10;
    public TextMeshProUGUI hpText;
    public GameObject failUI;

    private bool isSpawning = false;

    void Start()
    {
        UpdateHPUI();
        failUI.SetActive(false);
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (playerHP > 0)
        {
            if (!isSpawning)
            {
                isSpawning = true;

                // Randomly choose between Quadratic and Cubic spawn points
                bool useQuadratic = Random.value > 0.5f;
                Transform spawnPoint = useQuadratic ? spawnPointQuadratic : spawnPointCubic;
                Enemy.MovementType movementType = useQuadratic ? Enemy.MovementType.Quadratic : Enemy.MovementType.Cubic;

                // Spawn the enemy
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                enemy.GetComponent<Enemy>().Init(targetLocation, movementType, this);

                // Wait before allowing another spawn
                yield return new WaitForSeconds(spawnInterval);
                isSpawning = false;
            }
            yield return null;
        }
    }

    public void TakeDamage()
    {
        playerHP--;
        UpdateHPUI();

        if (playerHP <= 0)
        {
            GameOver();
        }
    }

    void UpdateHPUI()
    {
        hpText.text = "HP: " + playerHP;
    }

    void GameOver()
    {
        failUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
