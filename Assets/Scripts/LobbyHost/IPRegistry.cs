using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class IpRegistry : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        var utp = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;
        if (utp == null) { Debug.LogWarning("No UnityTransport."); return; }

        NetworkManager.Singleton.OnClientConnectedCallback += cid =>
        {
            string label = TryGetAddressCompat(utp, cid);
            Debug.Log($"Client {cid} → {label}");
            // TODO: salva label nella tua LobbyState per mostrarla in UI
        };
    }

    private string TryGetAddressCompat(UnityTransport utp, ulong clientId)
    {
        // 1) UTP 2.x/3.x: GetRemoteEndpoint(ulong) -> NetworkEndpoint (ToString contiene ip:port)
        var m = utp.GetType().GetMethod("GetRemoteEndpoint");
        if (m != null)
        {
            var endpoint = m.Invoke(utp, new object[] { clientId });
            return endpoint?.ToString() ?? "unknown";
        }

        // 2) UTP 1.x: GetClientAddress(ulong) -> string ip
        m = utp.GetType().GetMethod("GetClientAddress");
        if (m != null)
        {
            var ip = m.Invoke(utp, new object[] { clientId }) as string;
            return string.IsNullOrEmpty(ip) ? "unknown" : ip;
        }

        // 3) Nessuna API pubblica disponibile in questa versione
        return "unavailable (UTP version hides client IP)";
    }
}



