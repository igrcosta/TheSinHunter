using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AudioController : MonoBehaviour
{
    [SerializeField] AudioMixer Mixer;
    [SerializeField]  Slider MasterSlider;


    const string MixerMaster = "MasterVolume";
    const string MixerSfx = "SfxVolume";
    const string MixerMusic = "MusicVolume";



    void Awake()
    {
        MasterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            Debug.Log("Mouse pressionado");
        }
    }

    public void SetMasterVolume(float value)
    {

        Mixer.SetFloat(MixerMaster, value);

    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Arrastando");
    }
}
