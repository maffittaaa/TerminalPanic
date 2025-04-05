using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [field:Header("Player")]
    [field: Header("   Anxiety Bar")]
    [field: SerializeField] public AudioSource heartbeat { get; set; }
    [field: SerializeField] public AudioSource heartbeat2 { get; set; }
    
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
