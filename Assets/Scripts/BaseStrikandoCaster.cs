using System.Collections;
using UnityEngine;

public abstract class BaseStrikandoCaster : MonoBehaviour, IStrikandoCaster
{
    [Header("Strikando Spell Settings")]
    public GameObject strikandoPrefab;
    public float strikandoSpeed = 20f;
    public float rateOfCastStrikando = 0.5f;
    

    protected Camera cam;
    protected Transform leftMageCastpoint;
    protected bool canCastStrikando = true;

    private void Start()
    {
        cam = Camera.main;
        leftMageCastpoint = transform.Find("Castpoint");
    }

    protected void HandleStrikandoCasting(Vector2 target)
    {
        if (canCastStrikando)
        {
            CastStrikando(target);
            StartCoroutine(CastStrikando(target));
        }
    }

    public IEnumerator CastStrikando(Vector2 target)
    {
        canCastStrikando = false;

        GameObject strikando = Instantiate<GameObject>(strikandoPrefab, leftMageCastpoint.position, Quaternion.identity);
        Vector2 direction = (target - (Vector2)leftMageCastpoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        strikando.transform.rotation = Quaternion.Euler(0, 0, angle);
        strikando.GetComponent<Rigidbody2D>().linearVelocity = direction * strikandoSpeed;

        yield return new WaitForSeconds(rateOfCastStrikando);

        canCastStrikando = true;
    }
}
