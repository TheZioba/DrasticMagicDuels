using UnityEngine;

public class WindowsStrikandoCaster : BaseStrikandoCaster
{
    [Header("Strikando Spell Settings")]
    public Transform viewfinder;
    
    private void Awake()
    {
        if (viewfinder == null)
        {
            viewfinder = GameObject.Find("PlayerViewfinder")?.transform;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(viewfinder == null)
            {
                Debug.Log("Viewfinder is null on: " + gameObject.name);
            }            
            Vector2 target = (Vector2)viewfinder.position;
            HandleStrikandoCasting(target); // Chiama la funzione comune
        }
    }
}

