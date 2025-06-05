using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletCountUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextMeshPro pocketCountText;
    private TextMeshPro bulletCountText;

    private void Start()
    {
        bulletCountText = GetComponent<TextMeshPro>();
        bulletCountText.transform.parent.parent.gameObject.SetActive(false);
    }

    public void SetBulletCountText(float num, float maxBullets)
    {
        bulletCountText.text = num + "/" + maxBullets;
    }

    public void SetPocketCountText(float num, float maxBullets)
    {
        pocketCountText.text = num.ToString();
    }
}
