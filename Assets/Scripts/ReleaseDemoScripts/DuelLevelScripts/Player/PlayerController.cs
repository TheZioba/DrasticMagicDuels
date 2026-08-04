using UnityEngine;

[RequireComponent(typeof(Mover))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Dash dash;

    [Header("Movement Limits")]
    [SerializeField] private float xMinLimit = -9.5f;
    [SerializeField] private float xMaxLimit = -1f;
    [SerializeField] private float yMinLimit = -4f;
    [SerializeField] private float yMaxLimit = 5f;

    private PlayerInputActions input;
    private Mover mover;
    private ActionLock actionLock;

    private void Awake()
    {
        mover = GetComponent<Mover>();
        actionLock = GetComponent<ActionLock>();

        if (!dash)
            dash = GetComponent<Dash>();
    }

    private void OnEnable()
    {
        if (input == null)
            input = InputManager.Instance.Input;

        input.Player.Enable();
    }

    private void OnDisable()
    {
        if (input == null) return;

        input.Player.Disable();
    }

    private void Update()
    {
        if (actionLock != null && actionLock.IsLocked)
            return;

        Vector2 direction = Vector2.zero;

        if (input.Player.MoveUp.IsPressed()) direction.y += 1;
        if (input.Player.MoveDown.IsPressed()) direction.y -= 1;
        if (input.Player.MoveLeft.IsPressed()) direction.x -= 1;
        if (input.Player.MoveRight.IsPressed()) direction.x += 1;

        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        mover.MoveWithLimits(direction, xMinLimit, xMaxLimit, yMinLimit, yMaxLimit);

        if (dash != null && input.Player.Dash.WasPressedThisFrame())
            dash.TryDash(direction);
    }

    private void OnDestroy()
    {
        if (input == null) return;

        input.Disable();
        input.Dispose();
        input = null;
    }

}

