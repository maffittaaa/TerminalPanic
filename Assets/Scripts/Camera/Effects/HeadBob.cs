using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private float headDetor;
    [SerializeField] private float speed;
    [SerializeField] private GameObject viewPoint;
    private float initialYPosition;
    private float currentYPosition;


    private void Start()
    {
        initialYPosition = viewPoint.transform.position.y;
        currentYPosition = initialYPosition;
    }

    private void Update()
    {
        DetorEffect();
    }

    private void DetorEffect()
    {
        if (currentYPosition > initialYPosition + headDetor) 
        {
            speed *= -1;
        }
        else if(currentYPosition < initialYPosition - headDetor)
        {
            speed *= -1;
        }

        currentYPosition = currentYPosition + speed;

        viewPoint.transform.position = new Vector3(viewPoint.transform.position.x, currentYPosition);
    }
}
