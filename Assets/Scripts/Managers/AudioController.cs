using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioController Audio;
    GameController controller;

    [SerializeField] AudioMixer Mixer;
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider MusicSlider;

    const string MixerMaster = "MasterVolume";
    const string MixerSfx = "SfxVolume";
    const string MixerMusic = "MusicVolume";

    void Awake()
    {
        if (Audio == null) Audio = this;
    }

    void Start()
    {
        controller = GameController.controller;

        float master = PlayerPrefs.GetFloat("MasterVolume", 0f);
        float sfx = PlayerPrefs.GetFloat("SfxVolume", 0f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 0f);

        MasterSlider.value = master;
        SFXSlider.value = sfx;
        MusicSlider.value = music;

        Mixer.SetFloat(MixerMaster, master);
        Mixer.SetFloat(MixerSfx, sfx);
        Mixer.SetFloat(MixerMusic, music);

        MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMasterVolume(float value)
    {
        Mixer.SetFloat(MixerMaster, value);
        if (controller != null) controller.Master = value;

        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        Mixer.SetFloat(MixerSfx, value);
        if (controller != null) controller.SFX = value;

        PlayerPrefs.SetFloat("SfxVolume", value);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        Mixer.SetFloat(MixerMusic, value);
        if (controller != null) controller.Music = value;

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }
}