using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class AudioOptions : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("UI")]
    [SerializeField] private TMP_Text musicValue;
    [SerializeField] private TMP_Text sfxValue;

    public void OnMusicSliderChanged(float value)
    {
        SetMixerDb("MusicVolume", value);
        musicValue.text = Mathf.RoundToInt(value * 100f).ToString();
    }

    public void OnSfxSliderChanged(float value)
    {
        SetMixerDb("SfxVolume", value);
        sfxValue.text = Mathf.RoundToInt(value * 100f).ToString();
    }

    private void SetMixerDb(string param, float value01)
    {
        float v = Mathf.Max(value01, 0.0001f);
        mixer.SetFloat(param, Mathf.Log10(v) * 20f);
    }
}


