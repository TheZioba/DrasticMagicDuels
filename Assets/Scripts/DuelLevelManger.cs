using UnityEngine;

public class DuelLevelManger : MonoBehaviour
{
    [SerializeField] GameObject winnerPlayer;
    [SerializeField] ParticleSystem victoryParticles;

    public void OnShowVictory(GameObject winnerPlayer)
    {
        if (victoryParticles != null)
        {
            victoryParticles.Play();
        }
    }
    
}
