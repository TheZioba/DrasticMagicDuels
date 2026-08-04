using UnityEngine;

public class PlayerControllerManager : MonoBehaviour
{
    private IPlayerController movement;
    private IViewfinderController viewfinder;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            movement = GetComponent<AndroidPlayerController>();
            viewfinder = GetComponent<AndroidViewfinderController>();
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            movement = GetComponent<WindowsPlayerController>();
            //viewfinder = GetComponent<WindowsViewfinderController>();
        }
    }

    private void Update()
    {
        movement?.MovePlayer();
        viewfinder?.MoveViewfinder();
    }
}

