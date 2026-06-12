using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsToDuelScene : MonoBehaviour
{
    [SerializeField] private GameObject textObject;
    [SerializeField] private float delaySeconds = 3f;
    private bool canContinue = false;

    void Start()
    {
        textObject.SetActive(false);
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(delaySeconds);
        textObject.SetActive(true);
        canContinue = true;
    }

    void Update()
    {
        if(canContinue && Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
