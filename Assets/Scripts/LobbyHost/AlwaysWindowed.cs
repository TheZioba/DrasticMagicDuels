// AlwaysWindowed.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlwaysWindowed : MonoBehaviour
{
    [SerializeField] int w = 1280, h = 720;
    int framesLeft;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += (_, __) => framesLeft = 90; // ~1.5s a 60fps
        framesLeft = 90; // anche per la prima scena
    }

    void Update() {
        if (framesLeft-- <= 0) return;
        if (Screen.fullScreen || Screen.fullScreenMode != FullScreenMode.Windowed) {
            Screen.fullScreenMode = FullScreenMode.Windowed;            // API moderna
            Screen.fullScreen = false;                                  // compat
            Screen.SetResolution(w, h, FullScreenMode.Windowed);        // overload moderno
        }
    }
}

