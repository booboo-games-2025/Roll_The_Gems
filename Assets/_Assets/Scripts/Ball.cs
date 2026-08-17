using UnityEngine;
using System;
using Random = UnityEngine.Random;
using TMPro;

public class Ball : MonoBehaviour
{
    private const float SPAWN_X_OFFSET = 0.2f;
    private const float MIN_SPEED = 3f;
    private const float MAX_SPEED = 10f;
    private const float MAX_CRIT_CHANCE = 75f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Action OnBallDisabled;

    private int _ballIndex;
    private bool _isActive;
    private int _currentDurability;

    private void ResetVisuals()
    {
        _trail.emitting = false;

        transform.position = new Vector2(Random.Range(-SPAWN_X_OFFSET, SPAWN_X_OFFSET), 0f);

        _trail.Clear();
        _trail.emitting = true;
    }

    private void ResetPhysics()
    {
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
    }

    private int GetStartingDurability()
    {
        int durability = (int)UpgradeManager.instance.GetValue(_ballIndex, UpgradeType.Durability);

        if (UpgradeManager.DurabilityActive)
        {
            durability *= UpgradeManager.DurabilityMultiplier;
        }

        return durability;
    }

    private float GetLaunchSpeed()
    {
        float speed = (float)UpgradeManager.instance.GetValue(_ballIndex, UpgradeType.Speed);

        if (UpgradeManager.SpeedMultiplierActive)
        {
            speed *= UpgradeManager.SpeedMultiplier;
        }

        return Mathf.Clamp(speed, MIN_SPEED, MAX_SPEED);
    }

    private void ConsumeDurability()
    {
        if (!DurabilityInfiniteRv.IsActive)
        {
            _currentDurability--;
        }
    }

    private double CalculateIncome(out bool pIsCriticalHit)
    {
        double income = UpgradeManager.instance.GetValue(_ballIndex, UpgradeType.Income);

        income *= GetAchievementMultiplier();

        pIsCriticalHit = IsCriticalHit();

        if (!pIsCriticalHit)
        {
            return income;
        }

        double critMultiplier = GetCriticalPowerMultiplier();

        income *= critMultiplier;

        Achievements.OnAchievementsUpdated?.Invoke(1, AchievementType.GetCriticalIncomeXTime);

        return income;
    }

    private double GetAchievementMultiplier()
    {
        return 1 + (Achievements.progress * 3) / 100f;
    }

    private bool IsCriticalHit()
    {
        float critChance = (float)UpgradeManager.instance.GetValue(_ballIndex, UpgradeType.CriticalHitChance);

        if (UpgradeManager.CriticalChanceMultiplierActive)
        {
            critChance *= UpgradeManager.CriticalChanceMultiplier;
        }

        critChance = Mathf.Clamp(critChance, 0, MAX_CRIT_CHANCE);

        return Random.value <= critChance / 100f;
    }

    private double GetCriticalPowerMultiplier()
    {
        double critPower = UpgradeManager.instance.GetValue(_ballIndex, UpgradeType.CriticalHitPower);

        if (UpgradeManager.CriticalPowerMultiplierActive)
        {
            critPower *= UpgradeManager.CriticalPowerMultiplier;
        }

        return critPower / 100f;
    }

    private void CheckForDisable()
    {
        if (_currentDurability > 0)
        {
            return;
        }

        _isActive = false;

        gameObject.SetActive(false);

        OnBallDisabled?.Invoke();
    }

    public void DisableBall()
    {
        _isActive = false;
    }

    public bool IsActive() => _isActive;

    public int GetBallIndex() => _ballIndex;

    public void SetBallIndex(int pIndex)
    {
        _ballIndex = pIndex;
    }

    public void Init()
    {
        ResetVisuals();
        ResetPhysics();

        _isActive = true;
        _currentDurability = GetStartingDurability();

        float launchSpeed = GetLaunchSpeed();

        _rigidbody.AddForce(
            Random.insideUnitCircle.normalized * launchSpeed,
            ForceMode2D.Impulse);
    }

    public double ProcessHit()
    {
        ConsumeDurability();

        double moneyEarned = CalculateIncome(out bool criticalHit);

        GameManager.Instance.AddMoneyOnCollide(
            moneyEarned,
            transform.position,
            criticalHit);

        AudioManager.instance.PlaySFX(SFXType.ballCollision);

        CheckForDisable();

        return moneyEarned;
    }
}