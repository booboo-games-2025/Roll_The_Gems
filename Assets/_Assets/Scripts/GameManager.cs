using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;
using System;

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
    [SerializeField] private ParticleSystem ringDestroyEffect;
    [SerializeField] private Material[] ringMaterials;

    [Header("Background")]
    [SerializeField] Image background;
    [SerializeField] Sprite[] backgroundSprites;
    int bgIndex;
 

    private void OnEnable()
    {
        SkinButton.OnSkinChanged += ChangeSkin;
        Ring.OnHealthChanged += UpdateHealthUi;
        UpgradeManager.OnFirstTimeUpgrade += EnableBallSpawner;
    }

    private void OnDisable()
    {
        SkinButton.OnSkinChanged -= ChangeSkin;
        Ring.OnHealthChanged -= UpdateHealthUi;
        UpgradeManager.OnFirstTimeUpgrade -= EnableBallSpawner;
    }

    void ChangeSkin(int materialIndex)
    {
        for (int i = 0; i < rings.Length; i++)
        {
            rings[i].SwitchMaterial(ringMaterials[materialIndex]);
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        _cam = Camera.main;
    }

    private void Start()
    {
        Setup();
        EnableAllBallSpawner();
    }

    void Setup()
    {
        _currentRingIndex = 0;
        healthList.Clear();
        Shuffle(ringColors);
        CalculateRingData();
        for (int i = 0; i < ballSpawners.Length; i++)
        {
            ballSpawners[i].SetBallIndex(i);
        }
        UpdateHealthUi();
        healthUi.SetActive(true);
    }
    
    public static void Shuffle<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
        }
    }

    private int ringSet = 0;
    void CalculateRingData()
    {
        ringSet = PlayerPrefs.GetInt(MyConstants.RING_LEVEL, 1);
        _totalActiavtedRings = 5 + (ringSet/4);
        _totalActiavtedRings = Mathf.Clamp(_totalActiavtedRings, 5, rings.Length);
        int x = ringSet - 1;
        double incrementVal = 5 * Math.Pow(4f, x);
        double initialVal = 50;
        if (ringSet > 1)
        {
            initialVal = 37 * Math.Pow(x, 4) - 170 * Math.Pow(x, 2) + 295 * x - 104;
        }

        for (int i = 0; i < _totalActiavtedRings; i++)
        {
            healthList.Add(initialVal);
            rings[i].gameObject.SetActive(true);
            rings[i].SetParameters(Mathf.Lerp(1f,3f,(float)i/(_totalActiavtedRings-1)),ringColors[i],initialVal);
            initialVal += incrementVal;
        }
        ChangeBg();
    }

    void ChangeBg()
    {
        if (ringSet % 5 == 0)
        {
            background.sprite = backgroundSprites[bgIndex];
            bgIndex++;
            if (bgIndex == backgroundSprites.Length)
            {
                bgIndex = 0;
            }
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
                Invoke(nameof(EnableAllBallSpawner),2f);
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
        ParticleSystem[] systems = ringDestroyEffect.GetComponentsInChildren<ParticleSystem>();

        foreach (var ps in systems)
        {
            var main = ps.main;
            main.startColor = newColor;

            var shape = ps.shape;
            shape.radius = newRadius;
        }

        ringDestroyEffect.Play();
    }

    void EnableAllBallSpawner()
    {
        for (int i = 0; i < 8; i++)
        {
            EnableBallSpawner(i);
        }
    }
    
    void EnableBallSpawner(int ballSpawnerIndex)
    {
        ballSpawners[ballSpawnerIndex].Spawn();
    }

    readonly Color CriticalOrange = new Color(1f, 0.5f, 0f);
    public void AddMoneyOnCollide(double money, Vector3 pos, bool IsCriticalHit)
    {
        // =======================================
        // if related Rv or IAP Active
        if (UpgradeManager.IncomeMultiplierActive)
        {
            money *= UpgradeManager.IncomeMultiplier;
        }
        // ========================================
        EconomyManager.instance.IncreaseEconomy(money);
        Vector3 spawnPos = _cam.WorldToScreenPoint(pos);
        GameObject obj = ObjectPooling.Instance.Get("float_text",spawnPos);
        string moneyText = NumberFormatter.FormatNumberSmall(money);
        Color textColor = IsCriticalHit ? CriticalOrange : Color.white;
        obj.GetComponent<FloatingText>().Show(moneyText,textColor);
    }
}
