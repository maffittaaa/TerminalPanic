using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFainting : MonoBehaviour
{
    private Image blackScreen;
    
    [Header("Time")]
    [SerializeField] private int timeToFadeSeconds = 3;
    [SerializeField] private float timeBetweenOpacityChanged = 0.1f;
    [SerializeField] private float timeBetweenFades;
    [SerializeField] private float currentTimeSeconds;
    [SerializeField] private float opacityChangePerTick;
    [SerializeField] private bool fadeEnded = false;

    void Start()
    {
        opacityChangePerTick = 1 / (timeToFadeSeconds / timeBetweenOpacityChanged);
        blackScreen = GetComponent<Image>();
    }

    public IEnumerator ChangeBlackScreenOpacityUp()
    {
        fadeEnded = false;
        while (currentTimeSeconds < timeToFadeSeconds)
        {
            blackScreen.color = new Color(0, 0, 0, Mathf.Clamp(blackScreen.color.a + opacityChangePerTick, 0f, 1f));
            yield return new WaitForSeconds(timeBetweenOpacityChanged);
            currentTimeSeconds += timeBetweenOpacityChanged;
        }
        currentTimeSeconds = 0;
        fadeEnded = true;
        StartCoroutine(ChangeBlackScreenOpacityDown());
    }

    public IEnumerator ChangeBlackScreenOpacityDown()
    {
        yield return new WaitForSeconds(timeBetweenFades);
        while (currentTimeSeconds < timeToFadeSeconds)
        {
            blackScreen.color = new Color(0, 0, 0, Mathf.Clamp(blackScreen.color.a - opacityChangePerTick, 0f, 1f));
            yield return new WaitForSeconds(timeBetweenOpacityChanged);
            currentTimeSeconds += timeBetweenOpacityChanged;
        }
        currentTimeSeconds = 0;
    }

    public void ResetAndStartFade()
    {
        currentTimeSeconds = 0;
        StartCoroutine(ChangeBlackScreenOpacityUp());
    }

    public bool FadeEnded()
    {
        return fadeEnded;
    }
}
