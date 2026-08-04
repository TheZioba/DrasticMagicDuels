using UnityEngine;

public class VirtualJoystick : MonoBehaviour
{
    public RectTransform joystickBackground, joystickHandle;
    public GameObject joystickPanel;
    public Canvas canvas;

    private Vector2 joystickCenter;
    private float joystickRadius;
    private int joystickFingerId = -1;

    private void Start()
    {
        joystickRadius = joystickBackground.sizeDelta.x / 2;
        joystickCenter = joystickBackground.anchoredPosition;
    }

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId != joystickFingerId && touch.phase == TouchPhase.Began && PointInsideJoystickBackground(touch.position))
            {
                joystickFingerId = touch.fingerId;
            }

            if (touch.fingerId == joystickFingerId)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, touch.position, canvas.worldCamera, out var localTouchPos);
                        Vector2 direction = localTouchPos - joystickCenter;
                        joystickHandle.anchoredPosition = joystickCenter + direction.normalized * Mathf.Clamp(direction.magnitude, 0, joystickRadius);
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        joystickFingerId = -1;
                        joystickHandle.anchoredPosition = joystickCenter;
                        break;
                }
            }
        }
    }

    private bool PointInsideJoystickBackground(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, screenPosition, canvas.worldCamera, out var localPos);
        return Vector2.Distance(localPos, joystickCenter) <= joystickRadius;
    }

    public Vector2 GetInputDirection() => (joystickHandle.anchoredPosition - joystickCenter).normalized;
}

































