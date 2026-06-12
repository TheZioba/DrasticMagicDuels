using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (Application.platform == RuntimePlatform.Android)
        {
            Screen.SetResolution(1080, 2340, true);
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Screen.SetResolution(1920, 1080, true);
        }
    }
}


