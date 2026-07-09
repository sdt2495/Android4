using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class SwipeInput : MonoBehaviour
{
    [Header("イベント")]
    public UnityEvent<Vector2> OnSwipe;

    [Header("設定")]
    [SerializeField] private float minSwipeDistance = 50f;

    private Vector2 startPos;
    private Vector2 currentPos;

    private bool isDragging;
    private bool initialized;

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
#if UNITY_EDITOR

        // 起動直後のゴースト入力防止
        if (!initialized)
        {
            if (Mouse.current == null || !Mouse.current.leftButton.isPressed)
                initialized = true;

            return;
        }

        UpdateMouseInput();

#else

        UpdateTouchInput();

#endif
    }

    //==============================
    // エディタ・PC
    //==============================
    private void UpdateMouseInput()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPos = Mouse.current.position.ReadValue();
            currentPos = startPos;
            isDragging = true;
        }

        if (Mouse.current.leftButton.isPressed && isDragging)
        {
            currentPos = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            currentPos = Mouse.current.position.ReadValue();

            CheckSwipe();

            isDragging = false;
        }
    }

    //==============================
    // スマホ
    //==============================
    private void UpdateTouchInput()
    {
        if (Touch.activeTouches.Count == 0)
            return;

        Touch touch = Touch.activeTouches[0];

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            startPos = touch.screenPosition;
            currentPos = startPos;
            isDragging = true;
        }

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved && isDragging)
        {
            currentPos = touch.screenPosition;
        }

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended && isDragging)
        {
            currentPos = touch.screenPosition;

            CheckSwipe();

            isDragging = false;
        }
    }

    //==============================
    // スワイプ判定
    //==============================
    private void CheckSwipe()
    {
        Vector2 diff = currentPos - startPos;

        if (diff.magnitude >= minSwipeDistance)
        {
            OnSwipe?.Invoke(diff.normalized);
        }
    }
}