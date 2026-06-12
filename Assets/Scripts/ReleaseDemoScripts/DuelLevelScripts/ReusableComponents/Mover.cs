using UnityEngine;

public class Mover : MonoBehaviour
{
    public DuelStats stats;
    
    public void MoveWithLimits(Vector2 dir, float xMinLimit, float xMaxLimit, float yMinLimit, float yMaxLimit)
    {
        Vector3 pos = transform.position;
        pos += (Vector3)(dir * stats.moveSpeed * Time.deltaTime);

        pos.x = Mathf.Clamp(pos.x, xMinLimit, xMaxLimit);
        pos.y = Mathf.Clamp(pos.y, yMinLimit, yMaxLimit);

        transform.position = pos;
    }
}
