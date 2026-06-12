using Unity.Netcode;
using Unity.Collections;
using UnityEngine;
using TMPro;

public class PlayerNickname : NetworkBehaviour
{
    [SerializeField] TMP_Text label;

    // nickname replicato a tutti
    public NetworkVariable<FixedString32Bytes> Nick =
        new NetworkVariable<FixedString32Bytes>(writePerm: NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        // aggiorna la label quando il valore cambia (o subito se già presente)
        Nick.OnValueChanged += (_, v) => { if (label) label.text = v.ToString(); };
        if (label) label.text = Nick.Value.ToString();

        // solo il proprietario invia al server il proprio nick scelto localmente
        if (IsOwner)
        {
            var desired = string.IsNullOrWhiteSpace(NicknameStore.Current)
                ? $"Player{OwnerClientId}"
                : NicknameStore.Current;

            SubmitNickServerRpc(desired);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitNickServerRpc(string nick)
    {
        if (string.IsNullOrWhiteSpace(nick)) nick = $"Player{OwnerClientId}";
        if (nick.Length > 32) nick = nick.Substring(0, 32);
        Nick.Value = new FixedString32Bytes(nick);
    }
}

