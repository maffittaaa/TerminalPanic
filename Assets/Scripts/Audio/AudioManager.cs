using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;

public class AudioManager : MonoBehaviour
{
    [field:Header("Player")]
    [field: Header("   Anxiety Bar")]
    [field: SerializeField] public AudioSource heartbeat { get; set; }
    [field: SerializeField] public AudioSource heartbeat2 { get; set; }

    [field: Header("   Weapon")]
    [field: SerializeField] public AudioSource shoot { get; set; }

    [field: Header("   Movement")]
    [field: SerializeField] public AudioSource walkPlayer { get; set; }
    [field: SerializeField] public AudioSource runPlayer { get; set; }
    [field: SerializeField] public AudioSource crouchPlayer { get; set; }
    [field: SerializeField] public AudioSource stopPlayer { get; set; }

    [field: Header("   FlashLight")]
    [field: SerializeField] public AudioSource flashLightOn { get; set; }
    [field: SerializeField] public AudioSource flashLightOff { get; set; }

    [field: Header("Environment")]
    [field: Header("   People")]
    [field: SerializeField] public AudioSource crowd { get; set; }

    [field: Header("All Sounds")]

    [field: SerializeField] public List<AudioSource> allAudios { get; private set; }

    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    [field: Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer;

    protected virtual void Awake()
    {
        if (instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        SetAllAudiosList();


    }

    private void SetAllAudiosList()
    {
        allAudios = new List<AudioSource>();

        //Geral 
        allAudios.Add(heartbeat);
        allAudios.Add(heartbeat2);
        allAudios.Add(shoot);
        allAudios.Add(walkPlayer);
        allAudios.Add(runPlayer);
        allAudios.Add(crouchPlayer);
        allAudios.Add(stopPlayer);
        allAudios.Add(flashLightOn);
        allAudios.Add(flashLightOff);
        allAudios.Add(crowd);
    }

    public void AddToAudioList(AudioSource audio)
    {
        allAudios.Add(audio);
    }

    public void SetMasterVolume(float sliderValue)
    {
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
}
