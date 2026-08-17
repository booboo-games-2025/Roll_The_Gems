using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _zOffset = 0f;
    [SerializeField] private Ease _moveEase;

    private Vector3 _focusPoint;
    private Vector3 _originalPosition;

    private void Awake()
    {
        _originalPosition = transform.position;
    }

    public void MoveToOriginalPosition()
    {
        _focusPoint = _originalPosition;

        ZoomToTarget();
    }

    public void FocusOn(Vector3 pTargetPosition)
    {
        _focusPoint = pTargetPosition;
        _focusPoint.z = transform.position.z + _zOffset;

        float distance = Vector3.Distance(transform.position, _focusPoint);

        float duration = distance / _moveSpeed;
        transform.DOMove(_focusPoint, duration).SetEase(_moveEase);

        ZoomToTarget();
    }

    private void ZoomToTarget()
    {
        transform.DOKill();
        float distance = Vector3.Distance(transform.position, _focusPoint);

        float duration = distance / _moveSpeed;
        transform.DOMove(_focusPoint, duration).SetEase(_moveEase);
    }
}
