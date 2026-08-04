using UnityEngine;
using UnityEngine.InputSystem;

public class ViewfinderController : MonoBehaviour
{
    [Header("Camera Ratio Settings")]
    [SerializeField] private float yLowPixelHeightRatioLimit = 0.15f;
    [SerializeField] private float yHighPixelHeightRatioLimit = 0.85f;
    private Vector2 mousePosition;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        if (DuelGameManager.Instance != null && DuelGameManager.Instance.IsFrozen) return;

        MoveViewfinder();
    }
    public void MoveViewfinder()
    {
        mousePosition = Mouse.current.position.ReadValue();

        mousePosition.x = Mathf.Clamp(mousePosition.x, cam.pixelWidth / 2, cam.pixelWidth);
        mousePosition.y = Mathf.Clamp(mousePosition.y, cam.pixelHeight * yLowPixelHeightRatioLimit, cam.pixelHeight * yHighPixelHeightRatioLimit);
        
        Vector2 mouseWorldPosition = (Vector2)cam.ScreenToWorldPoint(mousePosition);
        transform.position = mouseWorldPosition;
    }
}

