using System.Collections;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class StrikandoNetworkCaster : NetworkBehaviour
{
    [SerializeField]
    private float noseOffset = 2f;

    [SerializeField]
    private GameObject strikandoLogicPrefab; // registrato in NetworkPrefabs

    [SerializeField]
    private GameObject proxyPrefab;

    [SerializeField]
    private float rateOfCastStrikando = 0.5f;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip shootClip;

    private int lastShootId = 0;

    bool canCastStrikando = true;

    private Transform castpoint;

    public Transform Castpoint => castpoint;

    public override void OnNetworkSpawn()
    {
        if (!castpoint)
            castpoint = transform.Find("VisualAndCollider").Find("Castpoint");
        if (!castpoint)
            Debug.LogError("CastPoint mancante sul player!");
    }

    void Update()
    {
        if (DuelGameManager.Instance != null && DuelGameManager.Instance.IsFrozen) return;

        if (IsOwner && canCastStrikando && Input.GetMouseButtonDown(0))
        {
            Vector2 view = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // LOGICO: se questo client gioca “specchiato”, invia il viewfinder già ribaltato
            // (asse X=0; cambia se ti serve un altro asse)
            if (/* questo client ha vista specchiata */ !IsServer)
                view.x = -view.x;

            var shooterRef = new NetworkObjectReference(NetworkObject);
            StartCoroutine(CastStrikando(shooterRef, view));
        }
    }

    IEnumerator CastStrikando(NetworkObjectReference shooterRef, Vector2 view)
    {
        canCastStrikando = false;
        CastStrikandoServerRpc(shooterRef, view);
        yield return new WaitForSeconds(rateOfCastStrikando);
        canCastStrikando = true;
    }

    [ServerRpc]
    void CastStrikandoServerRpc(NetworkObjectReference shooterRef, Vector2 viewPos)
    {
        if (!shooterRef.TryGet(out var shooterNo)) return;

        var caster = shooterNo.GetComponent<StrikandoNetworkCaster>();
        var cp = caster.Castpoint; // <-- Assicurati di avere: public Transform Castpoint => castpoint;
        if (!cp) { Debug.LogError("Castpoint mancante"); return; }

        // --- LOGICO (server) ---
        Vector2 dir = (viewPos - (Vector2)cp.position).normalized;
        Vector2 originTip = (Vector2)cp.position + dir * noseOffset;
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        var go = Instantiate(strikandoLogicPrefab, originTip, Quaternion.Euler(0, 0, ang));
        var sl = go.GetComponent<StrikandoLogic>();
        int shotId = ++lastShootId;
        sl.Init(dir, caster, shotId);

        // --- PROXY (grafica) ---
        float speed = sl.GetSpeed();
        float t0 = (float)NetworkManager.ServerTime.Time;

        ulong hostId = NetworkManager.ServerClientId;

        // A) HOST soltanto → NON mirrorato
        var toHost = new ClientRpcParams
        {
            Send = new ClientRpcSendParams { TargetClientIds = new[] { hostId } }
        };
        SpawnProxyClientRpc(originTip, dir, speed, t0, shotId, toHost);

        // B) Tutti i NON-host (client) → MIRROR su X
        var nonHost = new List<ulong>();
        foreach (var id in NetworkManager.Singleton.ConnectedClientsIds)
            if (id != hostId) nonHost.Add(id);

        if (nonHost.Count > 0)
        {
            var toClients = new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = nonHost.ToArray() }
            };
            Vector2 originMir = new Vector2(-originTip.x, originTip.y);
            Vector2 dirMir = new Vector2(-dir.x, dir.y);
            SpawnProxyClientRpc(originMir, dirMir, speed, t0, shotId, toClients);
        }

    }

    [ClientRpc]
    void SpawnProxyClientRpc(Vector2 origin, Vector2 dir, float speed, float serverT0, int shotId,
                         ClientRpcParams rpcParams = default)
    {
        if (StrikandoProxy.Active.ContainsKey(shotId)) return; // anti-dup
        var go = Instantiate(proxyPrefab, origin, Quaternion.identity);
        go.GetComponent<StrikandoProxy>().Init(shotId, origin, dir, speed, serverT0);

        var src = go.GetComponent<AudioSource>();
        if (src && shootClip)
            src.PlayOneShot(shootClip);
    }

    public void DestroyStrikandoProxy(int shotId, Vector2 hitPoint)
    {
        DestroyStrikandoClientRpc(shotId, hitPoint);
    }

    [ClientRpc]
    private void DestroyStrikandoClientRpc(int shotId, Vector2 hitPoint)
    {
        if (StrikandoProxy.Active.TryGetValue(shotId, out var proxy))
        {
            proxy.transform.position = hitPoint; // opzionale snap finale
            Destroy(proxy.gameObject);
        }
    }
}



