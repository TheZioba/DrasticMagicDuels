using UnityEngine;
using static MathUtils.MultipleStrikandoGeometry;
using static MathUtils.MultipleStrikandoTrajectory;

public class StrikandoCurvilinear : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 3;
    [SerializeField] private StrikandoCaster.StrikandoType strikandoType;

    private Vector2 castpoint;
    private Vector2 viewfinder;
    private float spaceOnParallelTrajectory;

    public void Init(Vector2 castpoint, Vector2 viewfinder)
    {
        this.castpoint = castpoint;
        this.viewfinder = viewfinder;
        spaceOnParallelTrajectory = 0;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        MoveAndRotateAlongTrajectory();
    }

    private void MoveAndRotateAlongTrajectory()
    {
        if (strikandoType == StrikandoCaster.StrikandoType.UP)
        {
            Vector2 tangentVersor = TangentVersorAlongCurve(viewfinder, castpoint, -1f, speed, spaceOnParallelTrajectory);

            if (spaceOnParallelTrajectory < (viewfinder - castpoint).magnitude)
            {
                spaceOnParallelTrajectory += speed * Time.deltaTime;
                transform.position = (Vector3)CurveParameterizationAlongCV(viewfinder, castpoint, 1f, spaceOnParallelTrajectory);

                float angleDeg = Mathf.Atan2(tangentVersor.y, tangentVersor.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
            }

            else
            {
                transform.position += (Vector3)(speed * tangentVersor * Time.deltaTime);
            }
        }

        else if (strikandoType == StrikandoCaster.StrikandoType.DOWN)
        {
            Vector2 tangentVersor = TangentVersorAlongCurve(viewfinder, castpoint, 1f, speed, spaceOnParallelTrajectory);

            if (spaceOnParallelTrajectory < (viewfinder - castpoint).magnitude)
            {
                spaceOnParallelTrajectory += speed * Time.deltaTime;
                transform.position = (Vector3)CurveParameterizationAlongCV(viewfinder, castpoint, -1f, spaceOnParallelTrajectory);

                float angleDeg = Mathf.Atan2(tangentVersor.y, tangentVersor.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
            }

            else
            {
                transform.position += (Vector3)(speed * tangentVersor * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("RightMage") || other.gameObject.layer == LayerMask.NameToLayer("LeftMage"))
        {
            if (!other.TryGetComponent<Health>(out var health))
                return;

            health.TakeDamage(damage);
            Destroy(gameObject);
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("Defendo"))
        {
            Destroy(gameObject);
        }
    }
}
