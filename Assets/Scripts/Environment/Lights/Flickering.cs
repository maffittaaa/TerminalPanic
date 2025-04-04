using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlickerMode { On, Off}

public class Flickering : MonoBehaviour
{
    [SerializeField] private int minFlickers = 1;
    [SerializeField] private int maxFlickers = 10;
    [SerializeField] private float minFlickerSpeed = 0.1f;
    [SerializeField] private float maxFlickerSpeed = 1.0f;
    [SerializeField] private float minTimeOff = 0.1f;
    [SerializeField] private float maxTimeOff = 1.0f;

    [SerializeField] private Light light1;
    [SerializeField] private Light light2;
    [SerializeField] private Light light3;

    [SerializeField] private GameObject lightMaterial;

    [SerializeField] private FlickerMode mode;

    [field: SerializeField] public Color mainColor { get; set; }
    [field: SerializeField] public Color currentColor { get; set; }

    private bool lightCheat = false;

    void Start()
    {
        mainColor = light1.color;
        currentColor = mainColor;
     
        if(mode == FlickerMode.On)
        {
            StartCoroutine(FlickerEffect());
        }
    }

    private IEnumerator FlickerEffect()
    {
        while (true)
        {
            if (!lightCheat)
            {
                for (int i = 0; i < Random.Range(minFlickers, maxFlickers); i++)
                {
                    ChangeLightsState(true);
                    yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
                    ChangeLightsState(false);
                    yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
                }
                yield return new WaitForSeconds(Random.Range(minTimeOff, maxTimeOff));
            }
            else
            {
                ChangeLightsState(true);
                yield return null;
            }
        }   
    }

    private void ChangeLightsState(bool state)
    {
        light1.enabled = state;
        light2.enabled = state;
        light3.enabled = state;

        light1.color = currentColor;
        light2.color = currentColor;
        light3.color = currentColor;

        if (state)
        {
            lightMaterial.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
        }
        else
        {
            lightMaterial.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }

        lightMaterial.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    public void TurnOnAndOff()
    {
        if (!lightCheat)
        {
            currentColor = Color.white;
            lightCheat = true;
        }
        else
        {
            currentColor = mainColor;
            lightCheat = false;
        }

        light1.color = currentColor;
        light2.color = currentColor;
        light3.color = currentColor;
    }
}
