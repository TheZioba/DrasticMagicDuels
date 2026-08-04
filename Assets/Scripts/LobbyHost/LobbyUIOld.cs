using UnityEngine;
using TMPro;
using System.Text;

public class LobbyUIOld : MonoBehaviour
{
    [SerializeField] private TMP_Text playersLabel;
    private LobbyState lobby;

    void Start()
    {
        lobby = FindFirstObjectByType<LobbyState>();
        if (lobby != null)
        {
            lobby.Players.OnListChanged += _ => Refresh();
            Refresh();
        }
    }

    void OnDestroy()
    {
        if (lobby != null)
            lobby.Players.OnListChanged -= _ => Refresh();
    }

    void Refresh()
    {
        if (lobby == null || playersLabel == null) return;

        var sb = new StringBuilder();
        foreach (var p in lobby.Players)
            sb.AppendLine(p.Nick.ToString());
        playersLabel.text = sb.ToString();
    }
}


