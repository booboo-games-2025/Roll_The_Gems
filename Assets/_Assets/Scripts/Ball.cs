using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public bool isActive;
    private int _currDurability;
    public int _maxDurability;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _speed;
    public Action _OnBallDisable;
    internal int BallIndex;
    [SerializeField] private TrailRenderer trail;

    public void Init()
    {
        trail.emitting = false;
        isActive = true;
        transform.position = new Vector2(Random.Range(-0.2f,0.2f),0);
        trail.Clear();
        trail.emitting = true;
        _rigidbody2D.linearVelocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        _currDurability = _maxDurability + PowerupsManager.instance.GetLevel(BallIndex,PowerType.Durability);
        _rigidbody2D.AddForce(Random.insideUnitCircle.normalized * (_speed + (_speed * PowerupsManager.instance.GetLevel(BallIndex,PowerType.Speed)/10f)), ForceMode2D.Impulse);
    }

    public double Damage()
    {
        _currDurability--;
        double money = UpgradeManager.Instance._ballUpgrades[BallIndex].income;
        int rand = Random.Range(1, 100);
        bool criticalHit = false;
        if (rand <= PowerupsManager.instance.GetLevel(BallIndex,PowerType.CriticalHitChance))
        {
            float increment = 2 * (PowerupsManager.instance.GetLevel(BallIndex, PowerType.CriticalHitPower)/10f);
            money *= (2 + increment);
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
