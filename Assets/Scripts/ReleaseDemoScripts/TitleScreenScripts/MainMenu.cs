using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;        
    }

    public void Play()
    {
        SceneManager.LoadScene("DuelLevelSceneVSCPUPC");        
    }

    public void Options()
    {
        optionsPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }    
}
