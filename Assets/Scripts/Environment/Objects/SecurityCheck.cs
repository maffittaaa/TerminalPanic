using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCheck : MonoBehaviour
{
    [SerializeField] private AudioSource alarm;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Renderer checkLight1;
    [SerializeField] private Renderer checkLight2;
    private bool alarming;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.AddToAudioList(alarm);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alarming)
        {
            alarming = true;
            StartCoroutine(FlickerRed());
        }
    }

    private IEnumerator FlickerRed()
    {
        alarm.Play();
        checkLight1.material.SetColor("_EmissionColor", Color.red * 3);
        checkLight2.material.SetColor("_EmissionColor", Color.red * 3);
        yield return new WaitForSeconds(0.8f);

        checkLight1.material.SetColor("_EmissionColor", Color.black);
        checkLight2.material.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(0.1f);

        checkLight1.material.SetColor("_EmissionColor", Color.red * 3);
        checkLight2.material.SetColor("_EmissionColor", Color.red * 3);
        yield return new WaitForSeconds(0.8f);

        checkLight1.material.SetColor("_EmissionColor", Color.black);
        checkLight2.material.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(0.1f);

        checkLight1.material.SetColor("_EmissionColor", Color.red * 3);
        checkLight2.material.SetColor("_EmissionColor", Color.red * 3);
        yield return new WaitForSeconds(0.8f);

        checkLight1.material.SetColor("_EmissionColor", Color.black);
        checkLight2.material.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(0.1f);

        checkLight1.material.SetColor("_EmissionColor", Color.red * 3);
        checkLight2.material.SetColor("_EmissionColor", Color.red * 3);
        yield return new WaitForSeconds(0.8f);

        checkLight1.material.SetColor("_EmissionColor", Color.black);
        checkLight2.material.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(0.1f);

        checkLight1.material.SetColor("_EmissionColor", Color.red * 3);
        checkLight2.material.SetColor("_EmissionColor", Color.red * 3);
        yield return new WaitForSeconds(0.8f);

        checkLight1.material.SetColor("_EmissionColor", Color.black);
        checkLight2.material.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(0.1f);

        checkLight1.material.SetColor("_EmissionColor", Color.green * 3);
        checkLight2.material.SetColor("_EmissionColor", Color.green * 3);
        alarming = false;
    }
}
