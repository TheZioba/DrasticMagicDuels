using UnityEngine;
using Unity.Netcode;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Animations;

public class LeftMageNetworkController : NetworkBehaviour
{
    private NetworkVariable<Vector2> leftMagePosition = new NetworkVariable<Vector2>(new Vector2(-5f, 0f), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private const float BASE_SPEED = 5f;

    private const float DODGE_SPEED = 16.67f;

    public float moveSpeed = 5f;

    [SerializeField] private Sprite ownerSprite;

    [SerializeField] private Transform visualAndCollider;

    private SpriteRenderer sr;

    private float lerpFactor = 0.1f;

    private bool canDodge = true;

    private bool isDodging = false;

    private float dodgeTime = 0.125f;

    private float rateOfDodge = 0.5f;

    private HealthBar healthBar;

    private int maxDP = 100;

    private int DP;

    [SerializeField]
    private AudioClip hitClip;

    private float horizontalRightLimit = -1f;
    private float horizontalLeftLimit = -10f;
    private float verticalUpperLimit = 5f;
    private float verticalLowerLimit = -4.15f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        var col = visualAndCollider.GetComponent<Collider2D>();
        if (col) col.enabled = IsServer; // collider spenti sui client, accesi solo sul server

        transform.position = new Vector3(leftMagePosition.Value.x, leftMagePosition.Value.y, 0f);

        sr = visualAndCollider.GetComponent<SpriteRenderer>();

        DP = maxDP;

        if (IsOwner)
        {
            sr.sprite = ownerSprite;
            sr.flipX = false;
            healthBar = GameObject.Find("LeftHealthBar").GetComponent<HealthBar>();
        }
        else if (IsClient && !IsOwner)
        {
            sr.sprite = ownerSprite;

            Transform visualAndCollider = transform.Find("VisualAndCollider");

            visualAndCollider.localScale = new Vector3(-1.85f, 1.85f, 1f);

            transform.position = new Vector3(-leftMagePosition.Value.x, leftMagePosition.Value.y, 0f);

            healthBar = GameObject.Find("RightHealthBar").GetComponent<HealthBar>();
        }
    }

    void Update()
    {
        //Debug.Log("FPS: " + 1/Time.deltaTime);
        if (DuelGameManager.Instance== null || DuelGameManager.Instance.IsFrozen) return;

        if (canDodge && !isDodging && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dodge());
        }

        MoveAndLimit();
    }
    private void MoveAndLimit()
    {
        if (IsOwner)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f).normalized * moveSpeed * Time.deltaTime;

            // Applica il movimento e poi clamp
            Vector3 pos = transform.position + movement;
            pos.x = Mathf.Clamp(pos.x, horizontalLeftLimit, horizontalRightLimit);
            pos.y = Mathf.Clamp(pos.y, verticalLowerLimit, verticalUpperLimit);

            transform.position = pos;
            leftMagePosition.Value = new Vector2(pos.x, pos.y);

        }

        else if (IsClient && !IsOwner)
        {
            Vector2 mirrored = new(-leftMagePosition.Value.x, leftMagePosition.Value.y);
            transform.position = Vector2.Lerp((Vector2)transform.position, mirrored, lerpFactor);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        DP = Mathf.Max(0, DP - damage);
        SetHealthBarSizeClientRpc(DP, maxDP);

        if (DP == 0)
        { 
            DuelGameManager.Instance.FinishMatch(OwnerClientId);
        }
    }

    [ClientRpc]
    public void SetHealthBarSizeClientRpc(int DP, int maxDP)
    {
        healthBar.SetSize((float)DP / maxDP);

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(hitClip);
    }

    IEnumerator Dodge()
    {
        canDodge = false;
        isDodging = true;

        moveSpeed = DODGE_SPEED;
        yield return new WaitForSeconds(dodgeTime);
        moveSpeed = BASE_SPEED;
        isDodging = false;

        yield return new WaitForSeconds(rateOfDodge);
        canDodge = true;
    }
}
















