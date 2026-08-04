using UnityEngine;

public abstract class BasePlayerController : MonoBehaviour, IPlayerController
{
    [Header("Movement Settings")]
    protected float moveSpeed = 2.5f;

    [Header("Movement Limits")]
    public float xmin = -9.5f;
    public float xmax = -1f;
    public float ymin = -4f;
    public float ymax = 5f;

    public abstract void MovePlayer(); 
}

