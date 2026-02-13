using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public bool isActive;
    private int _currDurability;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _speed;
    public Action _OnBallDisable;
    internal int BallIndex;
    [SerializeField] private TrailRenderer trail;

    [SerializeField] private Sprite[] skins;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Awake()
    {
    }

    void ChangeSkin(int ballIndex,int skinIndex)
    {
        if (ballIndex == BallIndex)
        {
            spriteRenderer.sprite = skins[skinIndex];
        }
    }

    public void Init()
    {
        trail.emitting = false;
        isActive = true;
        transform.position = new Vector2(Random.Range(-0.2f,0.2f),0);
        trail.Clear();
        trail.emitting = true;
        _rigidbody2D.linearVelocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        _currDurability = (int)UpgradeManager.instance.GetValue(BallIndex,UpgradeType.Durability);
        //var finalSpeed = (_speed + (_speed * PowerupsManager.instance.GetLevel(BallIndex,UpgradeType.Speed)/10f));
        var finalSpeed = (float)UpgradeManager.instance.GetValue(BallIndex, UpgradeType.Speed);
        
        // =======================================
        // if related Rv or IAP Active
        if (UpgradeManager.SpeedMultiplierActive)
        {
            finalSpeed *= UpgradeManager.SpeedMultiplier;
        }
        // =======================================
        
        _rigidbody2D.AddForce(Random.insideUnitCircle.normalized * finalSpeed, ForceMode2D.Impulse);
    }

    public double Damage()
    {
        if (!DurabilityInfiniteRv.IsActive)
        {
            _currDurability--;
        }

        double money = UpgradeManager.instance.GetValue(BallIndex,UpgradeType.Income);
        int rand = Random.Range(1, 100);
        bool criticalHit = false;
        if (rand <= (int)UpgradeManager.instance.GetValue(BallIndex,UpgradeType.CriticalHitChance))
        {
            double hitPower = UpgradeManager.instance.GetValue(BallIndex, UpgradeType.CriticalHitPower);
            
            // =======================================
            // if related Rv or IAP Active
            if (UpgradeManager.CriticalPowerMultiplierActive)
            {
                hitPower *= UpgradeManager.CriticalPowerMultiplier;
            }
            // =======================================
            
            money *= hitPower/100f;
            Achievements.OnAchievementsUpdated?.Invoke(1,AchievementType.GetCriticalIncomeXTime);
            criticalHit = true;
        }
        GameManager.Instance.AddMoneyOnCollide(money, transform.position,criticalHit);
        if (_currDurability == 0)
        {
            isActive = false;
            gameObject.SetActive(false);
            _OnBallDisable?.Invoke();
        }
        return money;
    }
}
