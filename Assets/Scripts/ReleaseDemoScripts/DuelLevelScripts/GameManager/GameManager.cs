using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Health player;
    [SerializeField] private Health opponentCPU;
    [SerializeField] private DuelUI duelUI;
    [SerializeField] private AudioSource fanfaraAudioSource;

    void Awake()
    {
        player.OnDefeat += OnCharacterDefeat;
        opponentCPU.OnDefeat += OnCharacterDefeat;
    }

    void OnCharacterDefeat(Health defeated)
    {
        musicSource.Stop();
        fanfaraAudioSource.Play();

        if (defeated == player)
        {
            duelUI.ShowWinMessage(opponentCPU.Name);
        }

        else if (defeated == opponentCPU)
        {
            duelUI.ShowWinMessage(player.Name);
        }
    }
}
