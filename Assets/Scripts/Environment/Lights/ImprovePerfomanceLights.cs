using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ImprovePerfomanceLights : MonoBehaviour
{
    [SerializeField] private int minDist = 80;
    private Light lightComponent;
    private GameObject player;

    private void Start()
    {
        lightComponent = GetComponent<Light>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 parentPos = new Vector3(transform.parent.position.x, 0, transform.parent.position.z);

        if(Vector3.Distance(playerPos, parentPos) > minDist)
        {
            if(lightComponent.renderMode != LightRenderMode.Auto)
            {
                lightComponent.renderMode = LightRenderMode.Auto;
            }
        }
        else
        {
            if (lightComponent.renderMode != LightRenderMode.ForcePixel)
            {
                lightComponent.renderMode = LightRenderMode.ForcePixel;
            }
        }
    }
}
