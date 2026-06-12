using UnityEngine;

public class DodgeButtons : MonoBehaviour
{
    public RectTransform dodgeButtonUp;
    public RectTransform dodgeButtonDown;
    public Canvas canvas;

    private void Update()
    {
        if (IsMouseOverButton(dodgeButtonUp))
        {
            if (Input.GetMouseButtonDown(0))
                Debug.Log("DodgeButtonUp pressed");
        }

        if (IsMouseOverButton(dodgeButtonDown))
        {
           if (Input.GetMouseButtonDown(0))
                Debug.Log("DodgeButtonDown pressed");
        }
    }

    private bool IsMouseOverButton(RectTransform button)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(button, Input.mousePosition, canvas.worldCamera);
    }
}


