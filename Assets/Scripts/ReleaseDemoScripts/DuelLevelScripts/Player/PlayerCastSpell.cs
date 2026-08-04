using UnityEngine;

public class PlayerCastSpell : MonoBehaviour
{
    [SerializeField] private Transform castpoint;
    [SerializeField] private Transform viewfinder;
    [SerializeField] private DuelStats stats;

    private PlayerInputActions input;
    private StrikandoCaster strikandoCaster;
    private DefendoCaster defendoCaster;
    private ActionLock actionLock;
    private float pressTime;

    void Awake()
    {
        input = InputManager.Instance.Input;
        input.Enable();

        strikandoCaster = GetComponent<StrikandoCaster>();
        defendoCaster = GetComponent<DefendoCaster>();
        actionLock = GetComponent<ActionLock>();
    }

    void Update()
    {
        if (actionLock != null && actionLock.IsLocked)
            return;

        if (input.Player.CastStrikando.WasPressedThisFrame())
        {
            pressTime = Time.time;
        }

        if (input.Player.CastStrikando.WasReleasedThisFrame())
        {
            float heldDuration = Time.time - pressTime;

            if (heldDuration >= stats.timeChargingMultipleStrikando)
                strikandoCaster.TryCastStrikando(castpoint, viewfinder, true);
            else
                strikandoCaster.TryCastStrikando(castpoint, viewfinder, false);
        }

        if (input.Player.CastDefendo.WasPressedThisFrame())
        {
            defendoCaster.TryCastDefendo(castpoint, false);
        }
    }
}
