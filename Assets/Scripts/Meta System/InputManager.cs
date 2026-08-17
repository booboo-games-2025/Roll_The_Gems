using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float _dragThreshold = 30f;

    public static event Action<GameObject> OnTouchPressed;
    public static event Action<Vector2> OnTouchHold;
    public static event Action OnTouchReleased;
    public static event Action<GameObject> OnTouchUI;
    public static event Action OnClicked;

    public static event Action<Vector2> OnDragStarted;
    public static event Action<Vector2> OnDragging;

    public event Action OnGoBack;

    private bool _wasPressedLastFrame = false;
    private bool _isInputOn = true;
    private bool _isTouchOverUI = false;

    private bool _isDragging;
    private Vector2 _pressPosition;

    private void Update()
    {
        if (!Application.isFocused)
        {
            _wasPressedLastFrame = false;
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OnGoBack?.Invoke();
        }

        if (!TryGetPointerInput(out Vector2 clickPosition, out bool isPressed, out bool pressedThisFrame, out bool releasedThisFrame))
        {
            //Debug.Log("No input whatsoever");
            return;
        }

        if (pressedThisFrame)
        {
            OnClicked?.Invoke();
        }

        if (IsPointerOverUI())
        {
            _isTouchOverUI = true;
            //return;
        }

        if (!isPressed && !_wasPressedLastFrame && !pressedThisFrame && !releasedThisFrame)
        {
            return;
        }

        if (_isInputOn)
        {
            if (pressedThisFrame)
            {
                GameObject uiObject = GetFirstUIObjectUnderPointer(clickPosition);

                OnTouchPressed?.Invoke(uiObject);

                _pressPosition = clickPosition;
                _isDragging = false;
            }
            else if (isPressed)
            {
                if (!_isDragging)
                {
                    if (Vector2.Distance(clickPosition, _pressPosition) >= _dragThreshold)
                    {
                        _isDragging = true;
                        OnDragStarted?.Invoke(clickPosition);
                    }
                    else
                    {
                        OnTouchHold?.Invoke(clickPosition);
                    }
                }
                else
                {
                    OnDragging?.Invoke(clickPosition);
                }
            }
            else if (releasedThisFrame)
            {
                OnTouchReleased?.Invoke();

                _isDragging = false;
            }
        }

        _wasPressedLastFrame = isPressed;
    }

    private GameObject GetFirstUIObjectUnderPointer(Vector2 pClickPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = pClickPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count > 0 ? results[0].gameObject : null;
    }

    private bool TryGetPointerInput(out Vector2 position, out bool isPressed, out bool pressedThisFrame, out bool releasedThisFrame)
    {
        position = default;
        isPressed = false;
        pressedThisFrame = false;
        releasedThisFrame = false;

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.isPressed ||
                touch.press.wasPressedThisFrame ||
                touch.press.wasReleasedThisFrame)
            {

                position = touch.position.ReadValue();
                isPressed = touch.press.isPressed;
                pressedThisFrame = touch.press.wasPressedThisFrame;
                releasedThisFrame = touch.press.wasReleasedThisFrame;

                return true;
            }
        }

        if (Mouse.current != null)
        {
            position = Mouse.current.position.ReadValue();
            isPressed = Mouse.current.leftButton.isPressed;
            pressedThisFrame = Mouse.current.leftButton.wasPressedThisFrame;
            releasedThisFrame = Mouse.current.leftButton.wasReleasedThisFrame;

            return true;
        }

        return false;
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.isPressed ||
                touch.press.wasPressedThisFrame ||
                touch.press.wasReleasedThisFrame)
            {
                return EventSystem.current.IsPointerOverGameObject(touch.touchId.ReadValue());
            }
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    public static void NotifyUIClick(GameObject pClickedButton)
    {
        OnTouchUI?.Invoke(pClickedButton);
    }

    public void ToggleInput(bool pToggle)
    {
        _isInputOn = pToggle;
    }
}
