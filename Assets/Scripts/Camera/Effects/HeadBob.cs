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
    [SerializeField] private AnxietyBar anxietyBar;
    [SerializeField] private float newPosition;
    [SerializeField] private float time;

    private void Start()
    {
        newPosition = viewPoint.transform.position.y;
        time = 0f;
    }

    private void Update()
    {
        if (!anxietyBar.interacted)
        {
            if (playerMovementScript.CurrentType() != BehaviorType.Jumping && playerMovementScript.CurrentType() != BehaviorType.Idleing)
            {
                if (playerMovementScript.CurrentType() == BehaviorType.Crouching)
                {
                    speed = crouchSpeed;
                }
                else if (playerMovementScript.CurrentType() == BehaviorType.Runing)
                {
                    speed = runSpeed;
                }
                else if (playerMovementScript.CurrentType() == BehaviorType.Walking)
                {
                    speed = walkSpeed;
                }
                DetorEffect();
            }
            else if (playerMovementScript.CurrentType() == BehaviorType.Idleing)
            {
                time = 0f;

                newPosition = Mathf.Lerp(transform.position.y, viewPoint.transform.position.y, Time.deltaTime * speed);
                transform.position = new Vector3(viewPoint.transform.position.x, newPosition, viewPoint.transform.position.z);
            }
        }
    }

    private void DetorEffect()
    {
        time += Time.deltaTime;
        newPosition = Mathf.Lerp(newPosition, viewPoint.transform.position.y + headDetor * Mathf.Sin(time * speed), Time.deltaTime * speed);
        transform.position = new Vector3(viewPoint.transform.position.x, newPosition, viewPoint.transform.position.z);
    }
}
