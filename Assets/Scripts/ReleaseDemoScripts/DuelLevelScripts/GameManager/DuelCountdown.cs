using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class DuelCountdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        // congela tutto il gioco
        InputManager.Instance.Input.Disable();
        Time.timeScale = 0f;

        countdownText.gameObject.SetActive(true);

        yield return Show("3", 1f);
        yield return Show("2", 1f);
        yield return Show("1", 1f);
        yield return Show("DUEL!", 0.6f);

        countdownText.gameObject.SetActive(false);

        Time.timeScale = 1f;
        InputManager.Instance.Input.Enable();
    }

    private IEnumerator Show(string text, float seconds)
    {
        countdownText.text = text;
        yield return new WaitForSecondsRealtime(seconds); 
    }
}

