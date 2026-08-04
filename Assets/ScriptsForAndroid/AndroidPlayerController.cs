using UnityEngine;

public class AndroidPlayerController : BasePlayerController
{
    [Header("Movement Settings")]
    public VirtualJoystick joystick;

    void Update()
    {
        if (joystick == null) return; 
        MovePlayer();
    }

    public override void MovePlayer()
    {
        Vector2 movementInput = joystick.GetInputDirection() * moveSpeed * Time.deltaTime;

        Vector2 newPosition = (Vector2)transform.position + movementInput;

        newPosition.x = Mathf.Clamp(newPosition.x, xmin, xmax);
        newPosition.y = Mathf.Clamp(newPosition.y, ymin, ymax);

        transform.position = newPosition;
    }
}