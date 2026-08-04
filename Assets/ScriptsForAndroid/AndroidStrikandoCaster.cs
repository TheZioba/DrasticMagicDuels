using UnityEngine;

public class AndroidStrikandoCaster : BaseStrikandoCaster
{
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 target = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
            HandleStrikandoCasting(target);
        }
    }
}

