using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MousePositionChecker : MonoBehaviour
{
    private readonly List<RaycastResult> _results = new();

    private void Update()
    {
        if (Mouse.current == null || EventSystem.current == null)
            return;

        PointerEventData pointerData = new(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        _results.Clear();
        EventSystem.current.RaycastAll(pointerData, _results);

        if (_results.Count > 0)
        {
            //Debug.Log($"UI under mouse: {_results[0].gameObject.name}");
        }
    }
}