using UnityEngine;

public class AIDangerSense : MonoBehaviour
{
    [SerializeField] float dodgeChance = 0.35f;   // 35%
    [SerializeField] float parryChance = 0.25f;   // 25%
    [SerializeField] float decisionCooldown = 0.25f;

    float nextDecisionTime;

    Dash dash;
    DefendoCaster defendoCaster;
    private ActionLock actionLock;

    void Awake()
    {
        dash = GetComponentInParent<Dash>();
        defendoCaster = GetComponentInParent<DefendoCaster>();
        actionLock = GetComponent<ActionLock>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (actionLock != null && actionLock.IsLocked)
        {
            return;
        }

        var spell = other.GetComponent<StrikandoStraight>();

        if (other.gameObject.layer != LayerMask.NameToLayer("StrikandoStraight") || spell.owner == Owner.FirstRightPlayerMage) return; 
        if (Time.time < nextDecisionTime) return;
        nextDecisionTime = Time.time + decisionCooldown;

        float r = Random.value;

        // prima prova a schivare, poi a parare
        if (r < dodgeChance)
        {
            // dash in direzione "lontano dal bullet"
            Vector2 away = r < 0.175f ? Vector2.up : Vector2.down;
            dash.TryDash(away);
        }
        else if (r < dodgeChance + parryChance)
        {
            defendoCaster.TryCastDefendo(transform.Find("Castpoint"), true);
        }
    }
}

