using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private float headDetor;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float speed;
    [SerializeField] private GameObject viewPoint;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private float newPosition;

    private void Start()
    {
        newPosition = viewPoint.transform.position.y;
    }

    private void Update()
    {
        if (playerMovementScript != null && playerMovementScript.CurrentType() != BehaviorType.Idleing && playerMovementScript.CurrentType() != BehaviorType.Jumping)
        {
            if(playerMovementScript.CurrentType() == BehaviorType.Walking)
            {
                speed = walkSpeed;
            }
            else if (playerMovementScript.CurrentType() == BehaviorType.Runing)
            {
                speed = runSpeed;
            }
            DetorEffect();
        }
        else if (playerMovementScript.CurrentType() != BehaviorType.Jumping)
        {
            newPosition = Mathf.Lerp(newPosition, viewPoint.transform.position.y, Time.deltaTime * 2f);
            transform.position = new Vector3(viewPoint.transform.position.x, newPosition, viewPoint.transform.position.z);
        }
    }

    private void DetorEffect()
    {
        newPosition = viewPoint.transform.position.y + headDetor * Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(viewPoint.transform.position.x, newPosition, viewPoint.transform.position.z);
    }

    private float MakePositive(float num)
    {
        return Mathf.Sqrt(Mathf.Pow(num, 2));
    }
}
