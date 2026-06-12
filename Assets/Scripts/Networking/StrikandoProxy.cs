using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class StrikandoProxy : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 3f;

    public static readonly Dictionary<int, StrikandoProxy> Active = new();

    public int ShotId { get; private set; }

    private Vector2 dir;
    private float speed;
    private double serverT0;



    public void Init(int id, Vector2 origin, Vector2 dir, float speed, double serverT0)
    {
        ShotId = id;
        Active[ShotId] = this; // registra nel dizionario

        this.dir = dir.normalized;
        this.speed = speed;
        this.serverT0 = serverT0;

        // rotazione fissa
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, ang);

        // posizione compensata al tempo del server
        double age = NetworkManager.Singleton.ServerTime.Time - serverT0;
        transform.position = origin + this.dir * this.speed * (float)age;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    void OnDestroy()
    {
        // toglilo dal registro solo se questo proxy è ancora lì
        if (Active.TryGetValue(ShotId, out var proxy) && proxy == this)
            Active.Remove(ShotId);
    }
}



