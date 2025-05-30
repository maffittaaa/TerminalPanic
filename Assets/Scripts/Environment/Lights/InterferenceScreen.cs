using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterferenceScreen : MonoBehaviour
{
    [SerializeField] private float timeBetweenChanging = 0.1f;
    [SerializeField] private GameObject[] interferences;
    private float index = 0;
    private bool currentState = true;

    private void Start()
    {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true) 
        {
            yield return new WaitForSeconds(timeBetweenChanging);
            if (currentState)
            {
                for (int i = 0; i < interferences.Length; i++)
                {
                    if (i == index)
                    {
                        interferences[i].SetActive(true);
                    }
                    else
                    {
                        interferences[i].SetActive(false);
                    }
                }

                index++;
                index = Mathf.Repeat(index, 3);
            }
        }
    }

    public void ScreenState(bool state)
    {
        for (int i = 0; i < interferences.Length; i++)
        {
            interferences[i].SetActive(state);
        }

        currentState = state;
    }
}
