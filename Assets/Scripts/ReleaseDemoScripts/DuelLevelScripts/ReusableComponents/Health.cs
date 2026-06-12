using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] int maxHp = 100;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private AudioSource hitSfx;
    [SerializeField] private DuelUI duelUI;

    private int hp;

    public System.Action<Health> OnDefeat;

    void Awake()
    {
        hp = maxHp;
    }

    void Start()
    {
        healthBar.SetSize(1f);
    }

    public void TakeDamage(int dmg)
    {
        hp = Mathf.Max(0, hp - dmg);
        healthBar.SetSize((float) hp / maxHp);

        hitSfx.Play();

        if(hp == 0)
        {
            OnDefeat?.Invoke(this);
        }
    }

    public string Name => characterName;
}
