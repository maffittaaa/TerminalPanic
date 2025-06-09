using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheVoiceAudio : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private AudioClip hourAnouncement;
    [SerializeField] private AudioClip boarding;
    [SerializeField] private AudioClip finalCall;

    [SerializeField] private AudioClip[] clothes;

    [SerializeField] private AudioSource[] anouncementsMicrofones;

    private void Start()
    {
        SaveAudios();
        PlayAudio(hourAnouncement);
    }

    private void SaveAudios()
    {
        for (int i = 0; i < anouncementsMicrofones.Length; i++)
        {
            audioManager.AddToAudioList(anouncementsMicrofones[i]);
        }
    }

    public void PlayAudio(int i)
    {
        PlayAudio(clothes[i]);
    }

    private void PlayAudio(AudioClip audio)
    {
        for (int i = 0; i < anouncementsMicrofones.Length; i++)
        {
            anouncementsMicrofones[i].clip = audio;
            anouncementsMicrofones[i].Play();
        }
    }
}
