using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class DepositedResource : MonoBehaviour
{
    [SerializeField] private Vector2 _arcHeightRange;
    [SerializeField] private float _punchScaleFactor;
    [SerializeField] private float _punchScaleDuration;
    [SerializeField] private int _punchScaleVibrato;
    [SerializeField] private float _punchScaleElasticity;

    [SerializeField] private RectTransform _targetAnchor;
    [SerializeField] private float _speed;

    private Action _onComplete;
    private Transform _target;
    private RectTransform _rect;
    private float _duration;
    private bool _isMoving;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void OnReached()
    {
        _target.DOPunchScale(Vector3.one * _punchScaleFactor, _punchScaleDuration, _punchScaleVibrato, _punchScaleElasticity);

        _onComplete?.Invoke();
    }

    public void Play(Vector2 pStartPosition, RectTransform pAnchor, Action pOnComplete)
    {
        _rect.anchoredPosition = pStartPosition;
        _targetAnchor = pAnchor;
        _onComplete = pOnComplete;
        _isMoving = true;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!_isMoving) return;

        _rect.anchoredPosition = Vector2.MoveTowards(
            _rect.anchoredPosition,
            _targetAnchor.anchoredPosition,
            _speed * Time.deltaTime
        );

        if (Vector2.Distance(_rect.anchoredPosition, _targetAnchor.anchoredPosition) < 5f)
        {
            _isMoving = false;
            OnReached();
        }
    }

    //public void Play(Vector2 pStartPosition, Vector2 pTargetPosition, float pJumpDuration, Action pOnComplete, RectTransform pAnchor)
    //{
    //    _rect.anchoredPosition = pStartPosition;
    //    _duration = pJumpDuration;
    //    _onComplete = pOnComplete;
    //    transform.localScale = Vector3.one;

    //    gameObject.SetActive(true);

    //    DOTween.To(
    //        () => _rect.anchoredPosition,
    //        x => _rect.anchoredPosition = x,
    //        pAnchor.anchoredPosition,
    //        _duration
    //    )
    //    .SetEase(Ease.OutQuad)
    //    .OnUpdate(() =>
    //    {
    //        _rect.anchoredPosition = Vector2.Lerp(
    //            _rect.anchoredPosition,
    //            pAnchor.anchoredPosition,
    //            0.2f
    //        );
    //    })
    //    .OnComplete(OnReached);

    //    //_rect.DOAnchorPos(pTargetPosition, _duration).SetEase(Ease.OutQuad).OnComplete(OnReached);
    //}
}
