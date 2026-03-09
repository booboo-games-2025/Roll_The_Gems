using UnityEngine;

public class SafeAreaFitter : MonoBehaviour
{
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _minAnchorMaxY;

    private void Start()
    {
        Rect safeArea = Screen.safeArea;

        RectTransform rectTransform = GetComponent<RectTransform>();

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height + _offset.y;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height + _offset.y;

        anchorMax.y = Mathf.Min(_minAnchorMaxY, anchorMax.y);

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
