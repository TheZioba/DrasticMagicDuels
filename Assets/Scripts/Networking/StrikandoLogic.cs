using UnityEngine;
using Unity.Netcode;

public class StrikandoLogic : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;

    private Vector2 dir;
    private Rigidbody2D rb;
    private float t;

    private int shotId;
    private StrikandoNetworkCaster caster;

    public float GetSpeed()
    {
        return speed;
    }

    public void Init(Vector2 dirWS, StrikandoNetworkCaster caster, int shotId)
    {
        this.caster = caster;
        this.shotId = shotId;
        dir = dirWS.normalized;
    }

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        t += Time.fixedDeltaTime;
        if (t >= lifeTime)
        {
            Destroy(gameObject);
            caster.DestroyStrikandoProxy(shotId, transform.position);
            return;
        }

        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        other.GetComponentInParent<LeftMageNetworkController>()?.TakeDamage(3);
        Destroy(gameObject);

        caster.DestroyStrikandoProxy(shotId, transform.position);

        Debug.Log($"Collisione avvenuta tra {this} e {other}");
    }
}



