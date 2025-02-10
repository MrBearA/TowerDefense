using UnityEngine;
using TMPro;
using System.Collections;

public class GoldManager : MonoBehaviour
{
    public int gold = 0;
    public TextMeshProUGUI goldText;
    public GameObject goldCoinPrefab; // UI Coin Prefab
    public Transform goldCounterUI; // Target position
    public float easeDuration = 0.5f;

    private Coroutine currentRoutine;

    public void AddGold(int amount)
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        int previousGold = gold;
        gold += amount;
        currentRoutine = StartCoroutine(AnimateGoldUI(previousGold, gold));
    }

    IEnumerator AnimateGoldUI(int startValue, int endValue)
    {
        float elapsed = 0;
        while (elapsed < easeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / easeDuration;
            goldText.text = "Gold: " + Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));
            yield return null;
        }

        goldText.text = "Gold: " + endValue;
    }

    // ✅ **Fix: Add the missing `SpendGold()` function**
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateUI();
            return true;
        }
        return false; // Not enough gold
    }

    void UpdateUI()
    {
        goldText.text = "Gold: " + gold;
    }

    public void SpawnGoldCoinUI(Vector3 enemyPosition)
    {
        GameObject coin = Instantiate(goldCoinPrefab, enemyPosition, Quaternion.identity);
        GoldCoinUI coinScript = coin.GetComponent<GoldCoinUI>();
        coinScript.SetTarget(goldCounterUI, this);
    }
}
