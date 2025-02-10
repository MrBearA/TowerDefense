using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public Turret turret; // Assign in Inspector
    public GoldManager goldManager; // Assign in Inspector
    public int upgradeCost = 5;
    public Button fireRateButton, rangeButton, bulletDistanceButton, homingMissileButton;

    private int fireRateLevel = 0;
    private int rangeLevel = 0;
    private int bulletDistanceLevel = 0;
    private const int maxLevel = 5;

    void Start()
    {
        if (fireRateButton != null) fireRateButton.onClick.AddListener(UpgradeFireRate);
        if (rangeButton != null) rangeButton.onClick.AddListener(UpgradeRange);
        if (bulletDistanceButton != null) bulletDistanceButton.onClick.AddListener(UpgradeBulletDistance);
        if (homingMissileButton != null) homingMissileButton.onClick.AddListener(UpgradeToHomingMissiles);
    }

    public void UpgradeFireRate()
    {
        if (fireRateLevel < maxLevel && goldManager.SpendGold(upgradeCost))
        {
            turret.UpgradeFireRate();
            fireRateLevel++;
            Debug.Log($"🔥 Fire Rate upgraded to Level {fireRateLevel}");
        }
        else
        {
            Debug.LogWarning("⚠️ Cannot upgrade Fire Rate (Max Level or Not Enough Gold)");
        }
    }

    public void UpgradeRange()
    {
        if (rangeLevel < maxLevel && goldManager.SpendGold(upgradeCost))
        {
            turret.UpgradeRange();
            rangeLevel++;
            Debug.Log($"📡 Range upgraded to Level {rangeLevel}");
        }
        else
        {
            Debug.LogWarning("⚠️ Cannot upgrade Range (Max Level or Not Enough Gold)");
        }
    }

    public void UpgradeBulletDistance()
    {
        if (bulletDistanceLevel < maxLevel && goldManager.SpendGold(upgradeCost))
        {
            turret.UpgradeBulletDistance();
            bulletDistanceLevel++;
            Debug.Log($"🔫 Bullet Distance upgraded to Level {bulletDistanceLevel}");
        }
        else
        {
            Debug.LogWarning("⚠️ Cannot upgrade Bullet Distance (Max Level or Not Enough Gold)");
        }
    }

    public void UpgradeToHomingMissiles()
    {
        if (goldManager.SpendGold(10)) // Homing missile upgrade costs 10 gold
        {
            turret.UnlockHomingMissiles();
            Debug.Log("🚀 Homing Missiles Unlocked!");
        }
        else
        {
            Debug.LogWarning("⚠️ Not enough gold to unlock Homing Missiles!");
        }
    }
}
