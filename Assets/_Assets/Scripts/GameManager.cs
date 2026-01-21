using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Ring[] rings;
    [SerializeField] private Color[] ringColors;
    [SerializeField] private BallSpawner[] ballSpawners;
    [SerializeField] private GameObject healthUi;
    [SerializeField] TMP_Text _healthText;
    [SerializeField] Image _fillBar;
    private int _currentRingIndex = 0;
    private int _totalActiavtedRings;
    
    private List<Double> healthList = new List<Double>();
    private Camera _cam;

    private void OnEnable()
    {
        Ring.OnHealthChanged += UpdateHealthUi;
        UpgradeManager.OnFirstTimeUpgrade += EnableBallSpawner;
    }

    private void OnDisable()
    {
        Ring.OnHealthChanged -= UpdateHealthUi;
        UpgradeManager.OnFirstTimeUpgrade -= EnableBallSpawner;
    }

    private void Awake()
    {
        Instance = this;
        _cam = Camera.main;
    }

    private void Start()
    {
        Setup();
    }

    void Setup()
    {
        _currentRingIndex = 0;
        healthList.Clear();
        CalculateRingData();
        for (int i = 0; i < ballSpawners.Length; i++)
        {
            ballSpawners[i].SetBallIndex(i);
        }
        UpdateHealthUi();
        healthUi.SetActive(true);
    }

    private int ringSet = 0;
    void CalculateRingData()
    {
        ringSet = PlayerPrefs.GetInt(MyConstants.RING_LEVEL, 1);
        _totalActiavtedRings = 5 + (ringSet/4);
        _totalActiavtedRings = Mathf.Clamp(_totalActiavtedRings, 5, rings.Length);
        int x = ringSet - 1;
        double incrementVal = 5 * Math.Pow(2.5f, x);
        double initialVal = 10;
        if (ringSet > 1)
        {
            initialVal = 37 * Math.Pow(x, 3) - 170 * Math.Pow(x, 2) + 295 * x - 104;
        }

        for (int i = 0; i < _totalActiavtedRings; i++)
        {
            healthList.Add(initialVal);
            rings[i].SetParameters(ringColors[i],initialVal);
            rings[i].gameObject.SetActive(true);
            initialVal += incrementVal;
        }
    }

    void UpdateHealthUi()
    {
        double currentHealth = rings[_currentRingIndex].GetHealth();
        if (currentHealth <= 0)
        {
            _currentRingIndex++;
            if (_currentRingIndex ==_totalActiavtedRings)
            {
                for (int i = 0; i < ballSpawners.Length; i++)
                {
                    ballSpawners[i].StopSpawning();
                }

                ringSet++;
                PlayerPrefs.SetInt(MyConstants.RING_LEVEL, ringSet);
                healthUi.SetActive(false);
                Invoke(nameof(Setup),1f);
                Invoke(nameof(CheckAndEnableBallSpawner),1f);
            }
        }

        if (_currentRingIndex < _totalActiavtedRings)
        {
            _healthText.text = NumberFormatter.FormatNumberSmall(rings[_currentRingIndex].GetHealth());
            _fillBar.color = ringColors[_currentRingIndex];
            _fillBar.fillAmount = (float)(rings[_currentRingIndex].GetHealth() / healthList[_currentRingIndex]);
        }
    }

    void CheckAndEnableBallSpawner()
    {
        for (int i = 0; i < ballSpawners.Length; i++)
        {
            UpgradeManager.Instance.TriggerBallSpawnerEventBasedOnLevel(i);
        }
    }
    
    void EnableBallSpawner(int ballSpawnerIndex)
    {
        ballSpawners[ballSpawnerIndex].Spawn();
    }

    public void AddMoneyOnCollide(double money, Vector3 pos)
    {
        EconomyManager.instance.IncreaseEconomy(money);
        Vector3 spawnPos = _cam.WorldToScreenPoint(pos);
        GameObject obj = ObjectPooling.Instance.Get("float_text",spawnPos);
        string moneyText = NumberFormatter.FormatNumberSmall(money);
        obj.GetComponent<FloatingText>().Show(moneyText);
    }
}
