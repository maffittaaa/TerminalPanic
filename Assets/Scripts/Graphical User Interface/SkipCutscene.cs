using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCutscene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SkipCut());
    }

    private IEnumerator SkipCut()
    {
        yield return new WaitForSeconds(44);
        SceneManager.LoadScene("MainScene");
    }
}
