using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Color mainColor;
    private Color currentColor;

    private bool lightCheat = false;

    void Start()
    {
        mainColor = light1.color;
        currentColor = mainColor;
        StartCoroutine(FlickerEffect());
    }

    private IEnumerator FlickerEffect()
    {
        while (true)
        {
            if (!lightCheat)
            {
                for (int i = 0; i < Random.Range(minFlickers, maxFlickers); i++)
                {
                    ChangeLightsSate(true);
                    yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
                    ChangeLightsSate(false);
                    yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
                }
                yield return new WaitForSeconds(Random.Range(minTimeOff, maxTimeOff));
            }
            else
            {
                ChangeLightsSate(true);
                yield return null;
            }
        }   
    }

    private void ChangeLightsSate(bool state)
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
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
        }
    }
}
