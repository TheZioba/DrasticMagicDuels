using UnityEngine;

[CreateAssetMenu(menuName = "Game/DuelStats")]
public class DuelStats : ScriptableObject
{
    public float moveSpeed = 5f;
    public float timeIntervalBetweenTwoStrikandoCast = 0.5f;
    public float timeIntervalBetweenTwoDefendoCast = 3f;
    public float timeChargingMultipleStrikando = 1f;

    public enum Owner
    {
        FirstLeftPlayerMage,
        SecondLeftPLayerMage,
        ThirdLeftPlayerMage,
        FirstRightPlayerMage,
        SecondRightPlayerMage,
        ThirdRightPlayerMage
    }
}
