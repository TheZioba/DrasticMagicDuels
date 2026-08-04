using UnityEngine;

public class WindowsPlayerController : BasePlayerController
{ 
    private void Update()
    {
        MovePlayer();
    }

    public override void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

        float clampedX = Mathf.Clamp(transform.position.x + moveDirection.x * moveSpeed * Time.deltaTime, xmin, xmax);
        float clampedY = Mathf.Clamp(transform.position.y + moveDirection.y * moveSpeed * Time.deltaTime, ymin, ymax);

        transform.position = new Vector3(clampedX, clampedY, 0);
    }
}

