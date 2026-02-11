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
        SkinButton.OnSkinChanged += ChangeSkin;
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
        _currDurability = (int)PowerupsManager.instance.GetValue(BallIndex,UpgradeType.Durability);
        //var finalSpeed = (_speed + (_speed * PowerupsManager.instance.GetLevel(BallIndex,UpgradeType.Speed)/10f));
        var finalSpeed = (float)PowerupsManager.instance.GetValue(BallIndex, UpgradeType.Speed);
        if (BallSpeedIncreaseRv.IsActive)
        {
            finalSpeed *= 1.5f;
        }
        _rigidbody2D.AddForce(Random.insideUnitCircle.normalized * finalSpeed, ForceMode2D.Impulse);
    }

    public double Damage()
    {
        if (!DurabilityInfiniteRv.IsActive)
        {
            _currDurability--;
        }

        double money = PowerupsManager.instance.GetValue(BallIndex,UpgradeType.Income);
        int rand = Random.Range(1, 100);
        bool criticalHit = false;
        if (rand <= (int)PowerupsManager.instance.GetValue(BallIndex,UpgradeType.CriticalHitChance))
        {
            money *= PowerupsManager.instance.GetValue(BallIndex,UpgradeType.CriticalHitPower)/100f;
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
