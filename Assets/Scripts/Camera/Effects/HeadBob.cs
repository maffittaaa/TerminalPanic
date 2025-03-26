using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private float headDetor;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float speed;
    [SerializeField] private GameObject viewPoint;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private float newPosition;
    [SerializeField] private float time;

    private void Start()
    {
        newPosition = viewPoint.transform.position.y;
        time = 0f;
    }

    private void Update()
    {
        if (playerMovementScript.CurrentType() != BehaviorType.Idleing && playerMovementScript.CurrentType() != BehaviorType.Jumping)
        {
            if (playerMovementScript.CurrentType() == BehaviorType.Walking)
            {
                speed = walkSpeed;
            }
            else if (playerMovementScript.CurrentType() == BehaviorType.Runing)
            {
                speed = runSpeed;
            }
            else if (playerMovementScript.CurrentType() == BehaviorType.Crouching)
            {
                speed = crouchSpeed;
            }
            DetorEffect();
        }
        else if (playerMovementScript.CurrentType() != BehaviorType.Jumping)
        {
            time = 0f;

            newPosition = Mathf.Lerp(newPosition, viewPoint.transform.position.y, Time.deltaTime * 2f);
            transform.position = new Vector3(viewPoint.transform.position.x, newPosition, viewPoint.transform.position.z);
        }
    }

    private void DetorEffect()
    {
        time += Time.deltaTime;
        newPosition = viewPoint.transform.position.y + headDetor * Mathf.Sin(time * speed);
        transform.position = new Vector3(viewPoint.transform.position.x, newPosition, viewPoint.transform.position.z);
    }
}
