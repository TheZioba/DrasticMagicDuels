using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource introAudioSource;
    [SerializeField] private AudioSource loopAudioSource;
    [SerializeField] private AudioClip introBattleClip;
    [SerializeField] private AudioClip loopBattleClip;

    void Start()
    {
        introAudioSource.clip = introBattleClip;
        introAudioSource.loop = false;

        loopAudioSource.clip = loopBattleClip;
        loopAudioSource.loop = true;

        double startTime = AudioSettings.dspTime + 0.05;

        introAudioSource.PlayScheduled(startTime);
        loopAudioSource.PlayScheduled(startTime + introBattleClip.length);
    }
}
