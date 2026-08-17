using UnityEngine;

public class WorldToUIAnchor : MonoBehaviour
{
    [SerializeField] private Camera _worldCamera;
    [SerializeField] private RectTransform _canvasRect;

    private Transform _worldTarget;
    private RectTransform _anchorRect;

    private void Awake()
    {
        _anchorRect = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (!_worldTarget) return;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
            _worldCamera,
            _worldTarget.position
        );

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect,
            screenPoint,
            _worldCamera,
            out Vector2 localPoint
        );

        _anchorRect.anchoredPosition = localPoint;
    }

    public void SetTarget(Transform pTarget)
    {
        _worldTarget = pTarget;
    }
}
