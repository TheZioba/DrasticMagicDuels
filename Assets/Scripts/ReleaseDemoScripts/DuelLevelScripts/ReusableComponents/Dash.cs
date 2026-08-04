using System;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 16.67f;
    [SerializeField] float dashDuration = 0.125f;
    [SerializeField] float dashCooldown = 0.5f;

    private ActionLock actionLock;
    private float moveSpeed;
    private bool isDashing;
    private float dashEndTime;
    private float nextDashTime;

    Vector2 dashDir;

    void Awake()
    {
        actionLock = GetComponent<ActionLock>();
    }

    void Update()
    {
        if (!isDashing) return;

        if (isDashing && Time.time >= dashEndTime)
        {
            isDashing = false;
        }

        transform.position += (Vector3)(dashDir * dashSpeed * Time.deltaTime);
    }

    public bool TryDash(Vector2 inputDir)
    {
        if (actionLock != null && actionLock.IsLocked)
            return false;

        if (isDashing)
            return false;

        if (Time.time < nextDashTime)
            return false;

        if (inputDir.sqrMagnitude < 0.0001f)
            return false;

        dashDir = inputDir.normalized;

        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        nextDashTime = Time.time + dashCooldown;

        return true;
    }
}
