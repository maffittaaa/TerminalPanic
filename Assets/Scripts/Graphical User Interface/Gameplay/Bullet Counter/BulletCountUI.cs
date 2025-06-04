using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletCountUI : MonoBehaviour
{
    [Header("Settings")]
    private TextMeshPro bulletCountText;
    private bool state = false;

    private void Start()
    {
        bulletCountText = GetComponent<TextMeshPro>();
    }

    public void SetBulletCountText(float num, float maxBullets)
    {
        bulletCountText.text = num + "/" + maxBullets;
    }
}
