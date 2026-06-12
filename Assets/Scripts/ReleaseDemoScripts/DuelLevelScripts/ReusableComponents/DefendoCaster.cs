using UnityEngine;

public class DefendoCaster : MonoBehaviour
{
    [SerializeField] private DuelStats stats;
    [SerializeField] private GameObject defendoPrefab;
    [SerializeField] private float lifetime = 1.5f;
    [SerializeField] private AudioSource defendoCasterSfx;
    [SerializeField] private Vector3 localOffset = new Vector3(0, -1f, 0);
    [SerializeField] private Owner owner;
    [SerializeField] private ActionLock actionLock;

    private float nextCastTime = 0;
    
    public void TryCastDefendo(Transform castpoint, bool rightPlayerMageCaster)
    {
        if (Time.time < nextCastTime)
            return;

        CastDefendo(castpoint, rightPlayerMageCaster);
        nextCastTime = Time.time + stats.timeIntervalBetweenTwoDefendoCast;
    }

    private void CastDefendo(Transform castpoint, bool rightPlayerMageCaster)
    {
        GameObject gameObj = Instantiate(defendoPrefab, castpoint.position + localOffset, Quaternion.Euler(0, 0, 0));

        if (rightPlayerMageCaster)
            gameObj.transform.localScale = new Vector3(-1.815f, 1.815f, 1f);

        Defendo defendo = gameObj.GetComponent<Defendo>();
        defendo.Init(lifetime, owner);

        actionLock.LockFor(lifetime);

        defendoCasterSfx.Play();
    }
}
