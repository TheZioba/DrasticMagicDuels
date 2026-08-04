using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject androidUI;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            androidUI.SetActive(false);
        }
    }
}

