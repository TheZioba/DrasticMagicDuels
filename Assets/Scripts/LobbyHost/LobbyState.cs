using System;                 // IEquatable
using Unity.Netcode;
using Unity.Collections;

public struct PlayerInfo : INetworkSerializable, IEquatable<PlayerInfo>
{
    public ulong ClientId;
    public FixedString32Bytes Nick;

    public void NetworkSerialize<T>(BufferSerializer<T> s) where T : IReaderWriter
    {
        s.SerializeValue(ref ClientId);
        s.SerializeValue(ref Nick);
    }

    public bool Equals(PlayerInfo other) => ClientId == other.ClientId && Nick.Equals(other.Nick);
    public override bool Equals(object obj) => obj is PlayerInfo other && Equals(other);
    public override int GetHashCode() => unchecked(ClientId.GetHashCode() * 397) ^ Nick.GetHashCode();
}


public class LobbyState : NetworkBehaviour
{
    public NetworkList<PlayerInfo> Players;

    void Awake() => Players = new NetworkList<PlayerInfo>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        NetworkManager.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
        // registra subito l’host se già presente
        foreach (var id in NetworkManager.ConnectedClientsIds) AddOrUpdate(id, $"Player{id}");
    }
    public override void OnDestroy()
    {
        if (IsServer && NetworkManager != null)
        {
            NetworkManager.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.OnClientDisconnectCallback -= OnClientDisconnected;
        }
        base.OnDestroy();
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsClient && clientId == NetworkManager.Singleton.LocalClientId)
        {
            var lobby = FindFirstObjectByType<LobbyState>();
            if (lobby != null)
            {
                var nick = string.IsNullOrWhiteSpace(NicknameStore.Current)
                    ? $"Player{clientId}"
                    : NicknameStore.Current;

                lobby.SubmitNickServerRpc(nick);
            }
        }
    }

    void OnClientDisconnected(ulong cid) { for (int i = Players.Count - 1; i >= 0; i--) if (Players[i].ClientId == cid) Players.RemoveAt(i); }

    [ServerRpc(RequireOwnership = false)]
    public void SubmitNickServerRpc(string nick, ServerRpcParams p = default)
    {
        var cid = p.Receive.SenderClientId;
        if (string.IsNullOrWhiteSpace(nick)) nick = $"Player{cid}";
        if (nick.Length > 32) nick = nick[..32];
        AddOrUpdate(cid, nick);
    }


    void AddOrUpdate(ulong cid, string nick)
    {
        var info = new PlayerInfo { ClientId = cid, Nick = nick };
        for (int i = 0; i < Players.Count; i++) if (Players[i].ClientId == cid) { Players[i] = info; return; }
        Players.Add(info);
    }
}

