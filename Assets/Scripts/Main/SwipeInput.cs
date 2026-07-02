using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class SwipeInput : MonoBehaviour
{
    public UnityEvent<Vector2> OnSwipe;

    private Vector2 startPos;
    private Vector2 currentPos;
    private bool isDragging = false;
    private float minSwipeDistance = 50f;

    private bool initialized = false; // ← 起動直後のゴースト入力防止

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        // ============================
        // 起動直後のゴースト入力を1回だけ無視
        // ============================
        if (!initialized)
        {
            if (Mouse.current == null || Mouse.current.leftButton.isPressed == false)
                initialized = true;

            return;
        }

        // ============================
        // PC（マウス）用
        // ============================
        if (!Application.isMobilePlatform)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                startPos = Mouse.current.position.ReadValue();
                currentPos = startPos; // ← これが誤発火を完全に防ぐ
                isDragging = true;
            }
            else if (Mouse.current.leftButton.isPressed && isDragging)
            {
                currentPos = Mouse.current.position.ReadValue();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                if (isDragging)
                {
                    Vector2 diff = currentPos - startPos;
                    if (diff.magnitude >= minSwipeDistance)
                    {
                        OnSwipe?.Invoke(diff.normalized);
                    }
                }

                isDragging = false;
            }

            return;
        }

        // ============================
        // スマホ（タッチ）用
        // ============================
        var touches = Touch.activeTouches;

        if (touches.Count == 0)
        {
            isDragging = false;
            return;
        }

        var t = touches[0];

        if (t.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            startPos = t.screenPosition;
            currentPos = startPos;
            isDragging = true;
        }
        else if (t.phase == UnityEngine.InputSystem.TouchPhase.Moved && isDragging)
        {
            currentPos = t.screenPosition;
        }
        else if (t.phase == UnityEngine.InputSystem.TouchPhase.Ended && isDragging)
        {
            Vector2 diff = currentPos - startPos;
            if (diff.magnitude >= minSwipeDistance)
            {
                OnSwipe?.Invoke(diff.normalized);
            }

            isDragging = false;
        }
    }
}
