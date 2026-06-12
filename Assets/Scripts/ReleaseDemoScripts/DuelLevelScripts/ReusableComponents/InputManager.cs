using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public PlayerInputActions Input { get; private set; }

    private void Awake()
    {
        // Singleton classico
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Input = new PlayerInputActions();
        Input.Enable();
    }

    private void OnDestroy()
    {
        if (Input != null)
        {
            Input.Disable();
            Input.Dispose();
            Input = null;
        }
    }
}

