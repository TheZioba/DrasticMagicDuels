using UnityEngine;
using static MathUtils.MultipleStrikandoGeometry;

public class StrikandoCaster : MonoBehaviour
{
    [SerializeField] private DuelStats stats;
    [SerializeField] private GameObject strikandoPrefabStright;
    [SerializeField] private GameObject strikandoPrefabUp;
    [SerializeField] private GameObject strikandoPrefabDown;
    [SerializeField] private AudioSource strikandoCasterSfx;
    [SerializeField] private Owner owner;
    [SerializeField] private Transform initialSender;

    public enum StrikandoType
    {
        STRAIGHT,
        UP,
        DOWN
    }

    private float nextCastTime = 0;

    public void TryCastStrikando(Transform castpoint, Transform viewfinder, bool multpleStrikando)
    {
        if (Time.time < nextCastTime)
            return;

        CastStrikando(castpoint, viewfinder, StrikandoType.STRAIGHT);

        if (multpleStrikando)
        {
            CastStrikando(castpoint, viewfinder, StrikandoType.UP);
            CastStrikando(castpoint, viewfinder, StrikandoType.DOWN);
        }

        nextCastTime = Time.time + stats.timeIntervalBetweenTwoStrikandoCast;
    }

    private void CastStrikando(Transform castpoint, Transform viewfinder, StrikandoType strikandoType)
    {
        float theta = Theta(viewfinder.position, castpoint.position);

        if (strikandoType == StrikandoType.STRAIGHT)
        {
            Quaternion rot = Quaternion.Euler(0, 0, theta * Mathf.Rad2Deg);

            GameObject gameObj = Instantiate(strikandoPrefabStright, castpoint.position, rot);
            StrikandoStraight strikandoStright = gameObj.GetComponent<StrikandoStraight>();
            strikandoStright.Init(castpoint.position, viewfinder.position, owner, initialSender);
        }

        else if (strikandoType == StrikandoType.UP)
        {
            Quaternion rot = Quaternion.Euler(0, 0, (theta + PHI) * Mathf.Rad2Deg);

            GameObject gameObj = Instantiate(strikandoPrefabUp, castpoint.position, rot);
            StrikandoCurvilinear strikandoUp = gameObj.GetComponent<StrikandoCurvilinear>();
            strikandoUp.Init(castpoint.position, viewfinder.position);
        }

        else if (strikandoType == StrikandoType.DOWN)
        {
            Quaternion rot = Quaternion.Euler(0, 0, (theta - PHI) * Mathf.Rad2Deg);

            GameObject gameObj = Instantiate(strikandoPrefabDown, castpoint.position, rot);
            StrikandoCurvilinear strikandoDown = gameObj.GetComponent<StrikandoCurvilinear>();
            strikandoDown.Init(castpoint.position, viewfinder.position);
        }

        strikandoCasterSfx.Play();
    }
}
