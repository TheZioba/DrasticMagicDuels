using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class DuelGameManager : NetworkBehaviour
{

    public bool IsFrozen { get; private set; }

    [SerializeField]
    private AudioClip clock;

    [SerializeField]
    private AudioClip gong;

    [SerializeField]
    private AudioSource audioSource;

    public static DuelGameManager Instance;

    private void Awake()
    {
        Instance = this;        
    }

    public void FinishMatch(ulong loserId)
    {
        if (!IsServer) return;
        IsFrozen = true;
        FreezeClientRpc();
    }

    [ClientRpc]
    void FreezeClientRpc()
    {
        Time.timeScale = 0f;  // ferma animazioni, Update, fisica
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Quando la scena è inizializzata dal server → parte il timer
            Debug.Log("IsFrozen è " + IsFrozen);
            StartCoroutine(StartCountdown());
        }
    }

    private IEnumerator StartCountdown()
    {
        IsFrozen = true; // blocco input e logica di gioco

        // 3
        UpdateCountdownClientRpc("3");
        audioSource.PlayOneShot(clock);
        yield return new WaitForSeconds(1f);

        // 2
        UpdateCountdownClientRpc("2");
        audioSource.PlayOneShot(clock);
        yield return new WaitForSeconds(1f);

        //1
        UpdateCountdownClientRpc("1");
        audioSource.PlayOneShot(clock);
        yield return new WaitForSeconds(1f);

        // Start Duel!
        UpdateCountdownClientRpc("Start Duel!");
        audioSource.PlayOneShot(gong);
        yield return new WaitForSeconds(1f);

        // nascondi UI
        HideCountdownClientRpc();

        // ora si gioca
        IsFrozen = false;
        StartMatch();
    }

    private void StartMatch()
    {
        Debug.Log("Il duello è iniziato!");
        // qui puoi abilitare input, movimento, ecc.
    }

    [ClientRpc]
    private void UpdateCountdownClientRpc(string text)
    {
        var ui = FindFirstObjectByType<CountdownUI>();
        if (ui != null) ui.SetText(text);
    }

    [ClientRpc]
    private void HideCountdownClientRpc()
    {
        var ui = FindFirstObjectByType<CountdownUI>();
        if (ui != null) ui.Hide();
    }
}

