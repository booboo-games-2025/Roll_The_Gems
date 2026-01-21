using UnityEngine;
using System;
using System.Collections.Generic;

public class PowerupsManager : MonoBehaviour
{
    public static PowerupsManager instance;
    
    private string savePath;
    public PowerSaveData saveData;
    private bool isPanelOpened = false;
    public UpgradesUisForBall[] upgradesUisForBalls;
    [SerializeField] PowerupBaseCostSo[] powerupBaseCosts;

    public static Action<int> OnPowerUp;

    private const int TOTAL_BALLS = 3;
    
    private void OnEnable()
    {
        EconomyManager.OnCoinChanged += UpdateAvailability;
        UpgradeManager.OnFirstTimeUpgrade += ActivePowerUisForBall;
    }

    private void OnDisable()
    { 
        EconomyManager.OnCoinChanged -= UpdateAvailability;
        UpgradeManager.OnFirstTimeUpgrade -= ActivePowerUisForBall;
    }

    private void Awake()
    {
        instance = this;
        Load();
    }

    void CreateDefaultData()
    {
        saveData = new PowerSaveData();

        for (int i = 0; i < TOTAL_BALLS; i++)
        {
            BallPowerData ball = new BallPowerData();
            ball.ballId = i;

            foreach (PowerType type in System.Enum.GetValues(typeof(PowerType)))
            {
                ball.powers.Add(new PowerData
                {
                    powerType = type,
                    level = 0,
                    baseCost = powerupBaseCosts[i].baseCosts[(int)type],
                    costMultiplier = 1.5f
                });
                
                UpdateUi(i,type, ball.powers[(int)type].baseCost);
            }

            saveData.balls.Add(ball);
        }
    }

    public BallPowerData GetBall(int ballId)
    {
        return saveData.balls[ballId];
    }

    public void UpgradePower(int ballId, PowerType type)
    {
        PowerData power = GetBall(ballId).GetPower(type);
        double previousCost = power.GetUpgradeCost();
        OnPowerUp?.Invoke(ballId);
        power.Upgrade();
        UpdateUi(ballId, type, power.GetUpgradeCost());
        EconomyManager.instance.DecreaseEconomy(previousCost);
    }

    public int GetLevel(int ballIndex, PowerType type)
    {
        return GetBall(ballIndex).GetPower(type).level;
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(MyConstants.POWERUPS_UPGRADE_DATA, json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(MyConstants.POWERUPS_UPGRADE_DATA))
        {
            string json = PlayerPrefs.GetString(MyConstants.POWERUPS_UPGRADE_DATA);
            saveData = JsonUtility.FromJson<PowerSaveData>(json);
            UpdateAllUi();
        }
        else
        {
            CreateDefaultData();
            Save();
        }
    }

    void UpdateAllUi()
    {
        for (int i = 0; i < TOTAL_BALLS; i++)
        {
            foreach (PowerType type in Enum.GetValues(typeof(PowerType)))
            {
                PowerData power = GetBall(i).GetPower(type);
                UpdateUi(i, type, power.GetUpgradeCost());
            }
        }
    }

    void UpdateUi(int ballId, PowerType type, double cost)
    {
        upgradesUisForBalls[ballId].powerUpgradesUi[(int)type].UpdateUi(cost);
    }
    
    void UpdateAvailability()
    {
        if(!isPanelOpened) return;
        
        double currentMoney = EconomyManager.instance.coinCount;
        for (int i = 0; i < TOTAL_BALLS; i++)
        {
            foreach (PowerType type in Enum.GetValues(typeof(PowerType)))
            {
                PowerData power = GetBall(i).GetPower(type);
                upgradesUisForBalls[i].powerUpgradesUi[(int)type].SwitchButton(currentMoney >= power.GetUpgradeCost());
            }
        }
    }

    public void ActivePowerUisForBall(int ballId)
    {
        if(ballId > 2) return;
        for (int i = 0; i < 6; i++)
        {
            upgradesUisForBalls[ballId].powerUpgradesUi[i].Active(true);
        }
    }
}

[System.Serializable]
public class PowerSaveData
{
    public List<BallPowerData> balls = new List<BallPowerData>();
}


[System.Serializable]
public class BallPowerData
{
    public int ballId;
    public List<PowerData> powers = new List<PowerData>();

    public PowerData GetPower(PowerType type)
    {
        return powers.Find(p => p.powerType == type);
    }
}
[System.Serializable]
public class PowerData
{
    public PowerType powerType;
    public int level;
    public double baseCost;
    public float costMultiplier;

    public Double GetUpgradeCost()
    {
        return baseCost * Mathf.Pow(costMultiplier, level);
    }

    public void Upgrade()
    {
        level++;
    }
}

[Serializable]
public class UpgradesUisForBall
{
    public PowerupsUpgradeUi[] powerUpgradesUi;
}


public enum PowerType
{
    IncomeSingle, CriticalHitChance, CriticalHitPower, Speed, BallCreationSpeed, Durability
}
