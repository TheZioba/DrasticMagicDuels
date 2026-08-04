using UnityEngine;

public abstract class BaseViewfinderController : MonoBehaviour, IViewfinderController
{
    [Header("Camera Ratio Settings")]
    public float yLowPixelHeightRatioLimit = 0.15f;
    public float yHighPixelHeightRatioLimit = 0.85f;

    public abstract void MoveViewfinder();
}
