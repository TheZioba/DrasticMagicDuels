using UnityEngine;

public class ActionLock : MonoBehaviour
{
   private float lockedUntil;
   public bool IsLocked => Time.time < lockedUntil;

   public void LockFor(float seconds)
    {
        lockedUntil = Mathf.Max(lockedUntil, Time.time + seconds);
    }
}
