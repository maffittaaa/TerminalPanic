using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletCountUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]    

    private TextMeshProUGUI bulletCountText;
    private bool state = false;

    private void Start()
    {
        bulletCountText = GetComponent<TextMeshProUGUI>();
    }

    public void SetBulletCountText(float num, float maxBullets)
    {
        bulletCountText.text = num + "/" + maxBullets;
    }

    public void ChangeTextOpacity()
    {
        if (state)
        {

        }
        else
        {

        }
    }

    private IEnumerator ChangeOpacityUp()
    {
        while (true) 
        { 
            //yield return WaitForSeconds();
        }
    }
}
