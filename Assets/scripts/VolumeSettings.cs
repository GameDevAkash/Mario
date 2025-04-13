using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;

    public void SetMusicVolume()
    {
        float value = MusicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(value) * 20f);
    }

    public void SetSFXVolume()
    {
        float value = SFXSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(value) * 20f);
    }

}
