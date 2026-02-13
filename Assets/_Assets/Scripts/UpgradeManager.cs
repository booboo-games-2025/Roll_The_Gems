using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    private string savePath;
    public PowerSaveData saveData;
    private bool isPanelOpened = false;
    public UpgradeUi[] UpgradeUis;
    [SerializeField] CanvasGroup upgradeCanvasGroup;
    [SerializeField] UpgradeBaseValues[] upgradeBaseCosts;
    [SerializeField] UpgradeBaseValues[] upgradeBaseValues;
    [SerializeField] Tab[] tabs;
    [SerializeField] private TMP_Text tabTitleText;

    [Space(5)]
    [Header("LockUI")] 
    [SerializeField] private GameObject lockUi;
    [SerializeField] private TMP_Text ballTitle;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private UiButton buyButton;
    
    public static Action<int, float> OnPowerUp;
    private const int TOTAL_BALLS = 8;
    public static int tabIndex;

    
    /// <summary>
    /// Multiplier Vairbales for Rv boosters or IAP.
    /// </summary>
    public static float IncomeMultiplier;
    public static bool IncomeMultiplierActive;
    
    public static float SpeedMultiplier;
    public static bool SpeedMultiplierActive;
    
    public static float CreationSpeedMuliplier;
    public static bool CreationSpeedMultiplierActive;

    public static float CriticalPowerMultiplier;
    public static bool CriticalPowerMultiplierActive;
    
    private void OnEnable()
    {
        EconomyManager.OnCoinChanged += UpdateAvailability;
        
        TwoxIncomeRv.OnActive += SetIncomeMultiplier;
        BallSpeedIncreaseRv.OnActive += SetSpeedMultiplier;
        TwoXBallCreationRv.OnActive += SetCreationSpeedMultiplier;
    }

    private void OnDisable()
    { 
        EconomyManager.OnCoinChanged -= UpdateAvailability;
        
       TwoxIncomeRv.OnActive -= SetIncomeMultiplier;
       BallSpeedIncreaseRv.OnActive -= SetSpeedMultiplier;
       TwoXBallCreationRv.OnActive -= SetCreationSpeedMultiplier;
    }

    public void SetIncomeMultiplier(float mul, bool active)
    {
        IncomeMultiplier = mul;
        IncomeMultiplierActive = active;
        UpdateAllUi();
    }
    
    public void SetSpeedMultiplier(float mul, bool active)
    {
        SpeedMultiplier = mul;
        SpeedMultiplierActive = active;
        UpdateAllUi();
    }
    
    public void SetCreationSpeedMultiplier(float mul, bool active)
    {
        CreationSpeedMuliplier = mul;
        CreationSpeedMultiplierActive = active;
        UpdateAllUi();
    }
    
    public void SetCriticalPowerMultiplier(float mul, bool active)
    {
        CriticalPowerMultiplier = mul;
        CriticalPowerMultiplierActive = active;
        UpdateAllUi();
    }

    private void Awake()
    {
        instance = this;
        AddButtonEvents();
        buyButton.clickEvent.AddListener(Unlock);
        Load();
    }
    
    private void Start()
    {
        SwitchTab(0);
        StartCoroutine(SaveRoutine());
    }
    
    void AddButtonEvents()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i;
            tabs[index].uiButton.clickEvent.AddListener(()=>
            {
                SwitchTab(index);
            });
        }
    }
    
    public void Load()
    {
        if (PlayerPrefs.HasKey(MyConstants.POWERUPS_UPGRADE_DATA))
        {
            string json = PlayerPrefs.GetString(MyConstants.POWERUPS_UPGRADE_DATA);
            saveData = JsonUtility.FromJson<PowerSaveData>(json);
        }
        else
        {
            CreateDefaultData();
            Save();
        }
    }
    
    public void SwitchTab(int selectedIndex)
    {
        tabIndex = selectedIndex;
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SwitchSprite(false);
        }
        tabs[selectedIndex].SwitchSprite(true);
        tabTitleText.text = GlobalvariableContainer.Instance.GetBallName(selectedIndex);
        UpdateAllUi();
        UpdateAvailability();
    }

    IEnumerator SaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            Save();
        }
    }

    void CreateDefaultData()
    {
        saveData = new PowerSaveData();

        for (int i = 0; i < TOTAL_BALLS; i++)
        {
            BallPowerData ball = new BallPowerData();
            ball.ballId = i;

            foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
            {
                ball.powers.Add(new PowerData
                {
                    upgradeType = type,
                    level = 0,
                    cost = upgradeBaseCosts[i].baseValues[(int)type],
                    value = upgradeBaseValues[i].baseValues[(int)type]
                });
            }
            saveData.balls.Add(ball);
        }
    }

    public BallPowerData GetBall(int ballId)
    {
        return saveData.balls[ballId];
    }

    public void Unlock()
    {
        saveData.balls[tabIndex].isUnlocked = true;
        OnFirstTimeUpgrade?.Invoke(tabIndex);
        double cost = tabIndex == 0 ? 0 : upgradeBaseCosts[tabIndex].baseValues[0];
        EconomyManager.instance.DecreaseEconomy(cost);
        AudioManager.instance.PlaySFX(SFXType.Unlock);
        Save();
    }

    public void Upgrade(int ballId, UpgradeType type)
    {
        PowerData power = GetBall(ballId).GetPower(type);
        power.level++;
        double previousCost = power.cost;
        if (power.upgradeType == UpgradeType.Income)
        {
             power.cost = previousCost * 1.5f;
             // if (ballId == 0 && power.level == 1)
             // {
             //     power.cost = upgradeBaseCosts[ballId].baseValues[(int)type];
             // }
             int twentyFiveMul = power.level / 25;
             double incrementMul = Math.Pow(2, twentyFiveMul);
             double newIncome = upgradeBaseValues[ballId].baseValues[(int)type] * incrementMul;
             power.value += newIncome;
             if (power.level % 25 == 0)
             {
                 power.value *= 2;
             }
        }
        else if (power.upgradeType == UpgradeType.CriticalHitChance)
        {
            power.cost = previousCost * 1.5f;
            power.value += 3; // 3% value added
        }
        else if (power.upgradeType == UpgradeType.CriticalHitPower)
        {
            power.cost = previousCost * 1.5f;
            power.value += 10f;// 10% value added
        }
        else if (power.upgradeType == UpgradeType.Speed)
        {
            power.cost = previousCost * 1.5f;
            power.value *= 1.1f; // 10% increase
        }
        else if (power.upgradeType == UpgradeType.BallCreationSpeed)
        {
            power.cost = previousCost * 1.5f;
            power.value *= 0.95f; // 5% decrease
        }
        else if (power.upgradeType == UpgradeType.Durability)
        {
            power.cost = previousCost * 1.5f;
            power.value ++;
        }
        //OnPowerUp?.Invoke(ballId, 1.5f);
        UpdateUi( type, power.cost, power.value, power.level);
        EconomyManager.instance.DecreaseEconomy(previousCost);
        Achievements.OnAchievementsUpdated?.Invoke(1,AchievementType.BuyUpgradesXTimes);
    }

    public int GetLevel(int ballIndex, UpgradeType type)
    {
        return GetBall(ballIndex).GetPower(type).level;
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(MyConstants.POWERUPS_UPGRADE_DATA, json);
        PlayerPrefs.Save();
    }

    void UpdateAllUi()
    {
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            PowerData power = GetBall(tabIndex).GetPower(type);
            UpdateUi(type, power.cost, power.value, power.level);
        }
    }

    void UpdateUi(UpgradeType type, double cost, double value, int level)
    {
        UpgradeUis[(int)type].UpdateUi(cost,value, level);
    }
    
    void UpdateAvailability()
    {
        double currentMoney = EconomyManager.instance.coinCount;
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            PowerData power = GetBall(tabIndex).GetPower(type);
            UpgradeUis[(int)type].SwitchButton(currentMoney >= power.cost);
        }
        
        UpdateLockUi(currentMoney);
        CheckForExclaimationMark(currentMoney);
    }

    void UpdateLockUi(double currentMoney)
    {
        // Update Lock Ui based on ball unlock state.
        bool isBallUnlocked = saveData.balls[tabIndex].isUnlocked;
        lockUi.SetActive(!isBallUnlocked);
        upgradeCanvasGroup.alpha = isBallUnlocked ?  1 : 0.2f;
        upgradeCanvasGroup.blocksRaycasts = isBallUnlocked;
        // ball is not locked then only update buyButton Interactbale state
        if (!isBallUnlocked)
        {
            double cost = upgradeBaseCosts[tabIndex].baseValues[0];
            if (tabIndex == 0)
            {
                cost = 0;
            }
            bool hasMoney = currentMoney >= cost;
            buyButton.Interactable = hasMoney;
            buyButton.image.sprite = hasMoney ? GlobalvariableContainer.Instance.enableSprite :  GlobalvariableContainer.Instance.disableSprite;
            ballTitle.text = "Unlock " + GlobalvariableContainer.Instance.GetBallName(tabIndex);
            costText.text = "<sprite=0> " + cost;
            if (tabIndex == 0)
            {
                costText.text = "<sprite=0> FREE";
            }
        }
    }

    void CheckForExclaimationMark(double currentMoney)
    {
        for (int i = 0; i < TOTAL_BALLS; i++)
        {
            tabs[i].exclaimMark.SetActive(false);
            if (tabIndex == i)
            {
                continue;
            }
            foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
            {
                PowerData power = GetBall(i).GetPower(type);
                if (currentMoney >= power.cost)
                {
                    tabs[i].exclaimMark.SetActive(true);
                    break;
                }
            }
        }
    }
    
    
    public static Action<int> OnFirstTimeUpgrade;

    public double GetValue(int ballIndex, UpgradeType type)
    {
        return saveData.balls[ballIndex].powers[(int)type].value;
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
    public bool isUnlocked;
    public List<PowerData> powers = new List<PowerData>();

    public PowerData GetPower(UpgradeType type)
    {
        return powers.Find(p => p.upgradeType == type);
    }
}

[System.Serializable]
public class PowerData
{
    public UpgradeType upgradeType;
    public int level;
    public double value;
    public double cost;
}

public enum UpgradeType
{
    Income ,CriticalHitChance, CriticalHitPower, Speed, BallCreationSpeed, Durability
}
