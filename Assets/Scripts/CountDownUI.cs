using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    public void SetText(string text)
    {
        countdownText.text = text;
        countdownText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        countdownText.gameObject.SetActive(false);
    }
}

