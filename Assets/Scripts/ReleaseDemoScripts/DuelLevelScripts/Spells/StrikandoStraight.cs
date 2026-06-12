using UnityEngine;

public class StrikandoStraight : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 3;
    [SerializeField] 

    private float spaceOnParallelTrajectory;
    public Owner owner;
    private Transform sender;
    private Vector2 a;
    private Vector2 b;

    public void Init(Vector2 castpoint, Vector2 viewfinder, Owner initialOwner, Transform InitialSender)
    {
        a = castpoint;
        b = viewfinder;
        spaceOnParallelTrajectory = 0;
        owner = initialOwner;
        sender = InitialSender;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        MoveAlongTrajectory(a, b);
    }

    private void MoveAlongTrajectory(Vector2 a, Vector2 b)
    {
        spaceOnParallelTrajectory += speed * Time.deltaTime;
        transform.position = (Vector3)(a + spaceOnParallelTrajectory * (b - a).normalized);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("RightMage") || other.gameObject.layer == LayerMask.NameToLayer("LeftMage"))
        {
            if(!other.TryGetComponent<Health>(out var health))
                return;

            health.TakeDamage(damage);
            Destroy(gameObject);
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("Defendo"))
        {
            if (!other.TryGetComponent<Defendo>(out var defendo))
                return;

            owner = defendo.owner;
            
            spaceOnParallelTrajectory = 0;
            a = transform.position;
            b = sender.position;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2((b - a).y, (b - a).x) * Mathf.Rad2Deg);
        }
    }
}
