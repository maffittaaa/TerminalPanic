using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }
    
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
    }
}
