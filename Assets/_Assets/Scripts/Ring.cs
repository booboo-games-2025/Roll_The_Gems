using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] private float _radius;
    [SerializeField] EdgeCollider2D _edgeCollider2D;
    private Transform _lineTransform;
    private int _layerIndex;
    private double _currHealth;
    
    public static Action OnHealthChanged;

    private void OnEnable()
    {
        RingHealthHalfRv.OnActive += ActiveHalfRingHealth;
    }

    private void OnDisable()
    {
        RingHealthHalfRv.OnActive -= ActiveHalfRingHealth;
    }

    private void Awake()
    {
        _lineTransform = _lineRenderer.transform;
        //_steps = (int)(_radius * 25f);
        _layerIndex = LayerMask.NameToLayer("Balls");
    }
    

    void ActiveHalfRingHealth()
    {
        _currHealth /= 2;
        OnHealthChanged?.Invoke();
    }

    public void SetParameters(float radius,Color color, double health)
    {
        _radius = radius;
        DrawCircle(_radius);
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
        _currHealth = health;
        if (RingHealthHalfRv.IsActive)
        {
            _currHealth /= 2;
        }
    }

    public void SwitchMaterial(Material newMat)
    {
        _lineRenderer.sharedMaterial = newMat;
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == _layerIndex)
        {
            Ball ball = other.collider.GetComponent<Ball>();
            _currHealth -= ball.Damage(); // this function damage the ball and return double value which is income or damage give to ring  
            OnHealthChanged?.Invoke();
            if (_currHealth <= 0)
            {
                gameObject.SetActive(false);
                Achievements.OnAchievementsUpdated?.Invoke(1,AchievementType.DestroyRings);
                AudioManager.instance.PlaySFX(SFXType.RingDestroySound);
            }
        }
    }

    void DrawCircle(float radius)
    {
        int steps = (int)(radius * 25f);
        List<Vector2> points = new List<Vector2>();
        //_lineRenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;
            
            float radians = circumferenceProgress * 2 * Mathf.PI;
            
            float x = radius * Mathf.Cos(radians);
            float y = radius * Mathf.Sin(radians);
            
            Vector3 currenPosition = new Vector3(x, y, 0);
            points.Add(currenPosition);
           // _lineRenderer.SetPosition(currentStep, currenPosition);
        }
        points.Add(points[0]);
        _edgeCollider2D.points = points.ToArray();
        StartCoroutine(GenerateLineSmooth(points));
    }
    
    IEnumerator GenerateLineSmooth(List<Vector2> points)
    {
        _lineRenderer.positionCount = 0;

        for (int i = 0; i < points.Count; i++)
        {
            // Add point to LineRenderer
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(i, points[i]);

            yield return null; // wait one frame
        }
    }

    [SerializeField] private float rotationSpeed;
    private void Update()
    {
        _lineTransform.Rotate(Vector3.back, Time.deltaTime * rotationSpeed);
    }

    public double GetHealth()
    {
        return _currHealth;
    }

    public float GetRadius()
    {
        return _radius;
    }
}
