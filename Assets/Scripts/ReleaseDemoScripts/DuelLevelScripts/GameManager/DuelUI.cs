using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DuelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text duelLabel;

    private PlayerInputActions input;

    void Awake()
    {
        input = InputManager.Instance.Input;
        input.Enable();
    }

    void OnEnable()
    {
        input.UI.Enable();
        input.UI.Exit.performed += OnExitPerformed;
    }

    void OnDisable()
    {
        input.UI.Exit.performed -= OnExitPerformed;
        input.UI.Disable();       
    }

    private void OnExitPerformed(InputAction.CallbackContext ctx)
    {
        
    }

    public void ShowWinMessage(string name)
    {
        duelLabel.gameObject.SetActive(true);
        duelLabel.text = $"{name} wins.\r\nPress Esc to return to main menu";

        Time.timeScale = 0f;
    }

    void Update()
    {
        if(Time.timeScale == 0f && input.UI.Exit.WasPressedThisFrame())
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
