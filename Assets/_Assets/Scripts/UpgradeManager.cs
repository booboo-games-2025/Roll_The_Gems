using System;
using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    
    [Header("MainUpgrades")]
    public BallUpgrade[] _ballUpgrades;
    [SerializeField] private UpgradeUi[] upgradeUis;
    [SerializeField] private double[] _baseCost;
    [SerializeField] private double[] _baseIncome;
    public static Action<int> OnFirstTimeUpgrade;

    private void OnEnable()
    {
        EconomyManager.OnCoinChanged += UpdateAvailability;
        PowerupsManager.OnPowerUp += UpgradeIncome;
        TwoxIncomeRv.OnActive += UpdateAllIncomeUi;
    }

    private void OnDisable()
    {
        EconomyManager.OnCoinChanged -= UpdateAvailability;
        PowerupsManager.OnPowerUp -= UpgradeIncome;
        TwoxIncomeRv.OnActive -= UpdateAllIncomeUi;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadModesData();
        for (int i = 0; i < upgradeUis.Length; i++)
        {
            int index = i;
            upgradeUis[index].upgradeBtn.clickEvent.AddListener(()=>
            {
                Upgrade(index);
            });
        }
        UpdateAvailability();
        StartCoroutine(Save());
    }

    void LoadModesData()
    {
        if (PlayerPrefs.HasKey(MyConstants.BALL_UPGRADE_DATA))
        {
            // LOAD
            string json = PlayerPrefs.GetString(MyConstants.BALL_UPGRADE_DATA);
            UpgradeData data = JsonUtility.FromJson<UpgradeData>(json);

            _ballUpgrades = data.ballUpgrades;

            for (int i = 0; i < upgradeUis.Length; i++)
            {
                TriggerBallSpawnerEventBasedOnLevel(i);
                upgradeUis[i].UpdateCostAndIncome(_ballUpgrades[i].cost,_ballUpgrades[i].income, _ballUpgrades[i].level);
            }
            Debug.Log("Ball Upgrade Data Loaded");
        }
        else
        {
            // CREATE DEFAULT + SAVE
            CreateDefaultData();
            SaveModesData();

            Debug.Log("Default Ball Upgrade Data Created & Saved");
        }
    }
    
    IEnumerator Save()
    {
        while (true)
        {
            SaveModesData();
            yield return new WaitForSeconds(5);
        }
    }

    void SaveModesData()
    {
        UpgradeData data = new UpgradeData
        {
            ballUpgrades = _ballUpgrades
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(MyConstants.BALL_UPGRADE_DATA, json);
        PlayerPrefs.Save();
    }

    void CreateDefaultData()
    {
        int upgradeCount = 8; // change as needed

        _ballUpgrades = new BallUpgrade[upgradeCount];

        for (int i = 0; i < upgradeCount; i++)
        {
            _ballUpgrades[i] = new BallUpgrade
            {
                level = 0,
                income = 0,
                cost = i == 0 ? 0 : _baseCost[i]
            };
            upgradeUis[i].UpdateCostAndIncome(_ballUpgrades[i].cost,_ballUpgrades[i].income, _ballUpgrades[i].level);
        }
    }

    void Upgrade(int index)
    {
        double previousCost = _ballUpgrades[index].cost;
        _ballUpgrades[index].cost = _baseCost[index] * Math.Pow(1.17f,_ballUpgrades[index].level);
        int twentyFiveMul = _ballUpgrades[index].level / 25;
        double incrementMul = Math.Pow(2, twentyFiveMul);
        double newIncome = _baseIncome[index] * incrementMul;
        //newIncome *= Math.Pow(3f/2f,PowerupsManager.instance.GetLevel(index,PowerType.IncomeSingle));
        _ballUpgrades[index].income += newIncome;
        _ballUpgrades[index].level++;
        if (_ballUpgrades[index].level % 25 == 0)
        {
            _ballUpgrades[index].income *= 2;
        }
        upgradeUis[index].UpdateCostAndIncome(_ballUpgrades[index].cost,_ballUpgrades[index].income, _ballUpgrades[index].level);
        upgradeUis[index].iconAnim.Play();
        TriggerBallSpawnerEventBasedOnLevel(index);
        EconomyManager.instance.DecreaseEconomy(previousCost);
        Achievements.OnAchievementsUpdated?.Invoke(1,AchievementType.BuyUpgradesXTimes);
    }

    // for Powerups Income increase
    public void UpgradeIncome(int ballIndex, float multiplier)
    {
        _ballUpgrades[ballIndex].income *= multiplier;
        upgradeUis[ballIndex].UpdateCostAndIncome(_ballUpgrades[ballIndex].cost,_ballUpgrades[ballIndex].income, _ballUpgrades[ballIndex].level);
    }
    
    // for 2x Income Booster
    void UpdateAllIncomeUi()
    {
        for (int i = 0; i < upgradeUis.Length; i++)
        {
            upgradeUis[i].UpdateCostAndIncome(_ballUpgrades[i].cost, _ballUpgrades[i].income, _ballUpgrades[i].level);
        }
    }

    public void TriggerBallSpawnerEventBasedOnLevel(int ballUpgradeIndex)
    {
        if (_ballUpgrades[ballUpgradeIndex].level >= 1)
        {
            OnFirstTimeUpgrade?.Invoke(ballUpgradeIndex);
            if (ballUpgradeIndex+1 < _ballUpgrades.Length)
            {
                upgradeUis[ballUpgradeIndex+1].content.SetActive(true);
            }
        }
    }

    void UpdateAvailability()
    {
        for (int i = 0; i < _ballUpgrades.Length; i++)
        {
            upgradeUis[i].SwitchButton(EconomyManager.instance.coinCount >= _ballUpgrades[i].cost);
        }
    }
}

[Serializable]
public class UpgradeData
{
    public BallUpgrade[] ballUpgrades;
}

[Serializable]
public class BallUpgrade
{
    public int level;
    public double income;
    public double cost;
}
