using TMPro;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class LocalIP : MonoBehaviour
{
    [SerializeField] TMP_Text ipLabel;

    void Start()
    {
        string localIP = GetLocalIPAddress();
        ipLabel.text = "My IP: " + localIP;
    }

     public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        return "Nessun IPv4 trovato";
    }
}

