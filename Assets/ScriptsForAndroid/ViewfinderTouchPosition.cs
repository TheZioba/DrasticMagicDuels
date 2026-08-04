using UnityEngine;

public class AndroidViewfinderController : MonoBehaviour, IViewfinderController
{
    public float yPixelHeightRatio = 0.15f;
    private Camera cam;
    public static int viewfinderFingerId = -1; // Variabile statica per il joystick

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            // Se il tocco è nella parte destra dello schermo ed è un nuovo tocco
            if (touch.phase == TouchPhase.Began &&
                touch.position.x > cam.pixelWidth / 2 &&
                touch.position.y > cam.pixelHeight * yPixelHeightRatio &&
                viewfinderFingerId == -1) // Solo se il mirino non è già assegnato a un dito
            {
                viewfinderFingerId = touch.fingerId; // Assegna il tocco al mirino
            }

            // Se il tocco assegnato al mirino si muove, aggiorniamo la posizione
            if (touch.fingerId == viewfinderFingerId &&
                (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
            {
                Vector2 touchWorldPosition = cam.ScreenToWorldPoint(touch.position);
                transform.position = touchWorldPosition;
            }

            // Se il tocco assegnato al mirino finisce, liberiamo l'ID
            if (touch.fingerId == viewfinderFingerId &&
                (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                viewfinderFingerId = -1;
            }
        }
    }

    public void MoveViewfinder() 
    {

    }
}