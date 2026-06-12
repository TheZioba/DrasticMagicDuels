using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using TMPro;
using System.Net;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text hostIpLabel;
    [SerializeField] private TMP_InputField hostIpInput;
    [SerializeField] private TMP_Text portLabel;
    [SerializeField] private TMP_InputField portInput;
    [SerializeField] private TMP_Text nicknameLabel;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private GameObject startHostButton;
    [SerializeField] private GameObject goBackButton;
    [SerializeField] private GameObject startClientButton;
    [SerializeField] private string duelSceneName = "DuelLevelSceneMultiplayer";

    private ushort defaultPort = 7777;

    void Start()
    {
        goBackButton.SetActive(false);

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted += () => Debug.Log("[HOST] Server started");
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += id => Debug.Log($"[NET] Disconnected {id}");
        }
    }

    void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    // ------------------ START HOST ------------------
    public void StartHost()
    {
        // Leggi nickname dal field (se c'è qualcosa)
        if (nicknameInput != null && !string.IsNullOrWhiteSpace(nicknameInput.text))
            NicknameStore.Current = nicknameInput.text.Trim();

        var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

        const ushort gamePort = 7777;
        utp.SetConnectionData("0.0.0.0", gamePort);

        NetworkManager.Singleton.StartHost();

        string localIp = LocalIP.GetLocalIPAddress();

        startHostButton.SetActive(false);
        SetHostInterfaceActive(false);
        goBackButton.SetActive(true);

        Debug.Log($"[HOST] Avviato da {NicknameStore.Current} su {localIp}:{gamePort}");
    }

    // ------------------ START CLIENT ------------------
    public void StartClient()
    {
        // Leggi nickname anche lato client
        if (nicknameInput != null && !string.IsNullOrWhiteSpace(nicknameInput.text))
            NicknameStore.Current = nicknameInput.text.Trim();

        var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

        (bool ok, string ip, ushort port) = ReadIpPort(hostIpInput, portInput, defaultPort);
        if (!ok) { Debug.LogError("IP non valido."); return; }

        utp.SetConnectionData(ip, port);
        NetworkManager.Singleton.StartClient();

        Debug.Log($"[CLIENT] {NicknameStore.Current} tenta la connessione a {ip}:{port}");
    }

    // ------------------ UTILITY ------------------
    (bool ok, string ip, ushort port) ReadIpPort(TMP_InputField ipField, TMP_InputField portField, ushort defaultPort)
    {
        string ipText = ipField?.text?.Trim() ?? "";
        string portText = portField?.text?.Trim() ?? "";

        if (ipText.Contains(":"))
        {
            var parts = ipText.Split(':');
            if (parts.Length == 2)
            {
                ipText = parts[0].Trim();
                portText = string.IsNullOrWhiteSpace(portText) ? parts[1].Trim() : portText;
            }
        }

        ushort port = defaultPort;
        if (!string.IsNullOrWhiteSpace(portText) && !ushort.TryParse(portText, out port))
            port = defaultPort;

        if (!IPAddress.TryParse(ipText, out _))
            return (false, null, 0);

        return (true, ipText, port);
    }

    // ------------------ ON CLIENT CONNECTED ------------------
    private void OnClientConnected(ulong clientId)
    {
        var count = NetworkManager.Singleton.ConnectedClientsIds.Count;
        Debug.Log($"[NET] Connected {clientId}. Clients now: {count}");

        // 🔹 manda subito il nickname scelto
        var lobby = FindFirstObjectByType<LobbyState>();
        if (lobby != null && NetworkManager.Singleton.IsClient)
        {
            string nick = string.IsNullOrWhiteSpace(nicknameInput.text)
                ? $"Player{NetworkManager.Singleton.LocalClientId}"
                : nicknameInput.text.Trim();

            lobby.SubmitNickServerRpc(nick);
        }

        // logica avvio match
        if (NetworkManager.Singleton.IsServer && count >= 2)
        {
            Debug.Log("[HOST] Starting match → loading duel scene...");
            NetworkManager.Singleton.SceneManager.LoadScene(duelSceneName, LoadSceneMode.Single);
        }
    }

    public void GoBack()
    {
        // Se stai facendo da host/server → chiudi
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            NetworkManager.Singleton.Shutdown();

        // Riporta la UI allo stato iniziale
        startHostButton.SetActive(true);
        SetHostInterfaceActive(true);
        goBackButton.SetActive(false);

        Debug.Log("[HOST] Sessione annullata, tornato alla lobby.");
    }

    private void SetHostInterfaceActive(bool isActive)
    {
        hostIpLabel.gameObject.SetActive(isActive);
        hostIpInput.gameObject.SetActive(isActive);
        portLabel.gameObject.SetActive(isActive);
        portInput.gameObject.SetActive(isActive);
        nicknameLabel.gameObject.SetActive(isActive);
        nicknameInput.gameObject.SetActive(isActive);
    }
}





