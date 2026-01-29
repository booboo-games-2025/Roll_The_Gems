using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;

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

    [SerializeField] private RectTransform floatingRv;
    [SerializeField] private ParticleSystem ringDestroyEffect;

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
        StartCoroutine(FloatingRvTimer());
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
            rings[i].SetParameters(Mathf.Lerp(1f,3f,(float)i/(_totalActiavtedRings-1)),ringColors[i],initialVal);
            rings[i].gameObject.SetActive(true);
            initialVal += incrementVal;
        }
    }

    void UpdateHealthUi()
    {
        double currentHealth = rings[_currentRingIndex].GetHealth();
        if (currentHealth <= 0)
        {
            PlayRingDestroyParticle(ringColors[_currentRingIndex],rings[_currentRingIndex].GetRadius());
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
                Invoke(nameof(Setup),2f);
                Invoke(nameof(CheckAndEnableBallSpawner),2f);
            }
        }

        if (_currentRingIndex < _totalActiavtedRings)
        {
            _healthText.text = NumberFormatter.FormatNumberSmall(rings[_currentRingIndex].GetHealth());
            _fillBar.color = ringColors[_currentRingIndex];
            _fillBar.fillAmount = (float)(rings[_currentRingIndex].GetHealth() / healthList[_currentRingIndex]);
        }
    }

    void PlayRingDestroyParticle(Color newColor, float newRadius)
    {
        var main = ringDestroyEffect.main;
        main.startColor = newColor;

        var shape = ringDestroyEffect.shape;
        shape.radius = newRadius;
        
        ringDestroyEffect.Play();
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

    readonly Color CriticalOrange = new Color(1f, 0.5f, 0f);
    public void AddMoneyOnCollide(double money, Vector3 pos, bool IsCriticalHit)
    {
        if (TwoxIncomeRv.IsActive)
        {
            money *= 2;
        }
        EconomyManager.instance.IncreaseEconomy(money);
        Vector3 spawnPos = _cam.WorldToScreenPoint(pos);
        GameObject obj = ObjectPooling.Instance.Get("float_text",spawnPos);
        string moneyText = NumberFormatter.FormatNumberSmall(money);
        Color textColor = IsCriticalHit ? CriticalOrange : Color.white;
        obj.GetComponent<FloatingText>().Show(moneyText,textColor);
    }

    #region FloatingRv

    IEnumerator FloatingRvTimer()
    {
        yield return new WaitForSeconds(180f);
        floatingRv.gameObject.SetActive(true);
        floatingRv.anchoredPosition = new Vector2(Random.Range(-250, 250), 700);
    }

    public void ClickedOnFloatingRv()
    {
        floatingRv.gameObject.SetActive(false);
        EconomyManager.instance.IncreaseEconomy(Random.Range(100,1000));
        StartCoroutine(FloatingRvTimer());
    }

    #endregion
}
