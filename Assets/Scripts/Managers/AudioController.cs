using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;


public class AudioController : MonoBehaviour
{

    public static AudioController Audio;
    GameController controller;

    [SerializeField] AudioMixer Mixer;
    [SerializeField]  Slider MasterSlider;
    [SerializeField]  Slider SFXSlider;
    [SerializeField]  Slider MusicSlider;

  



    const string MixerMaster = "MasterVolume";
    const string MixerSfx = "SfxVolume";
    const string MixerMusic = "MusicVolume";


 
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

    void Update()
    {

    }

    public void SetMasterVolume(float value)
    {

        Debug.Log("Master salvo: " + value);

        Mixer.SetFloat(MixerMaster, value);
        controller.Master = value;

        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();

    }
    public void SetSFXVolume(float value)
    {
        Mixer.SetFloat(MixerSfx, value);
        controller.SFX = value;

        Mixer.SetFloat(MixerSfx, value);
        controller.SFX = value;
    }
    public void SetMusicVolume(float value)
    {
        Mixer.SetFloat(MixerMusic, value);
        controller.Music = value; 

        Mixer.SetFloat(MixerMusic, value);
        controller.Music = value;
    }

}
