using UnityEngine;

public class IACPU : MonoBehaviour
{
    [SerializeField] private float decisionInterval = 0.5f;
    [SerializeField] private Transform castpoint;
    [SerializeField] private Transform viewfinder;
    [SerializeField] private DuelStats stats;

    [Header("Movement Limits")]
    [SerializeField] private float xMinLimit = -1f;
    [SerializeField] private float xMaxLimit = 9.5f;
    [SerializeField] private float yMinLimit = -4f;
    [SerializeField] private float yMaxLimit = 5f;

    [Header("Spellcasting Probabilities")]
    [SerializeField] private float castStrikandoStraight = 0.25f;
    [SerializeField] private float castMultipleStrikando = 0.05f;

    [Header("Viewfinder Movement")]
    [SerializeField] private float secondsIntervalBetweenViewfinderMovements = 2f;
    [SerializeField] private GameObject leftPlayerMage;

    private Mover mover;
    private StrikandoCaster strikandoCaster;
    private Vector2 currentDirection = Vector2.zero;
    private ActionLock actionLock;
    private float timer;
    private float viewfinderTimer;


    void Awake()
    {
        mover = GetComponent<Mover>();
        strikandoCaster = GetComponent<StrikandoCaster>();
        actionLock = GetComponent<ActionLock>();
    }

    void Update()
    {
        if (actionLock != null && actionLock.IsLocked)
            return;

        viewfinderTimer += Time.deltaTime;
        timer += Time.deltaTime;

        if (viewfinderTimer >= secondsIntervalBetweenViewfinderMovements)
        {
            viewfinder.transform.position = leftPlayerMage.transform.position;
            viewfinderTimer = 0;
        }

        if (timer >= decisionInterval)
        {
            ChooseMovementAction();
            ChooseSpellAction();
            timer = 0;
        }

        mover.MoveWithLimits(currentDirection, xMinLimit, xMaxLimit, yMinLimit, yMaxLimit);
    }

    private void ChooseMovementAction()
    {
        float r = Random.value;

        if (r < 0.20f)
        {
            currentDirection = Vector2.zero;
        }

        else
        {
            int dir = Random.Range(0, 4);

            switch (dir)
            {
                case 0:
                    currentDirection = Vector2.up;
                    break;
                case 1:
                    currentDirection = Vector2.down;
                    break;
                case 2:
                    currentDirection = Vector2.left;
                    break;
                case 3:
                    currentDirection = Vector2.right;
                    break;
            }
        }
    }

    private void ChooseSpellAction()
    {
        if (actionLock != null && actionLock.IsLocked) 
            return;

        float r = Random.value;

        if (r < castMultipleStrikando)
        {
            strikandoCaster.TryCastStrikando(castpoint, viewfinder, true);
        }

        else if (castMultipleStrikando < r && r < castStrikandoStraight)
        {
            int burstRemaining = 3;
            while (burstRemaining > 0)
            {
                if (actionLock != null && actionLock.IsLocked) 
                    break;
                strikandoCaster.TryCastStrikando(castpoint, viewfinder, false);
                burstRemaining--;
            }
        }
    }
}
