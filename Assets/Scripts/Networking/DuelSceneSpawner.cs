using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class DuelSceneSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;                 // deve avere NetworkObject
    [SerializeField] private string duelSceneName = "DuelLevelSceneMultiplayer";

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;                                        // solo server
        var sm = NetworkManager?.SceneManager;
        if (sm != null) sm.OnSceneEvent += OnSceneEvent;
    }

    public override void OnNetworkDespawn()
    {
        var sm = NetworkManager?.SceneManager;
        if (sm != null) sm.OnSceneEvent -= OnSceneEvent;
    }

    private void OnSceneEvent(SceneEvent e)
    {
        if (!IsServer) return;
        if (!string.Equals(e.SceneName, duelSceneName)) return;

        if (e.SceneEventType == SceneEventType.LoadComplete)
        {
            ulong clientId = e.ClientId;

            // evita doppio spawn se c'è già un PlayerObject
            if (NetworkManager.ConnectedClients.TryGetValue(clientId, out var cc) && cc.PlayerObject != null)
                return;

            if (playerPrefab == null)
            {
                Debug.LogError("[DuelSceneSpawner] playerPrefab non assegnato!");
                return;
            }

            var pos = GetSpawnPosFor(clientId);
            var go  = Instantiate(playerPrefab, pos, Quaternion.identity);

            var no = go.GetComponent<NetworkObject>();
            if (no == null)
            {
                Debug.LogError("[DuelSceneSpawner] Il prefab non ha un NetworkObject.");
                Destroy(go);
                return;
            }

            no.SpawnAsPlayerObject(clientId);
        }
    }

    private Vector3 GetSpawnPosFor(ulong clientId)
    {
        // indice deterministico per clientId
        var ordered = NetworkManager.ConnectedClientsIds.OrderBy(id => id).ToArray();
        int index   = System.Array.IndexOf(ordered, clientId);
        if (index < 0) index = 0;

        // alterna sinistra/destra
        return (index % 2 == 0) ? new Vector3(-3f, 0f, 0f) : new Vector3(3f, 0f, 0f);
    }
}


