using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenUIElement : MonoBehaviour
{
    private Vector3 _targetScale;

    private void OnEnable()
    {
        _targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(_targetScale, 0.2f).SetEase(Ease.InOutQuart);
    }
}
