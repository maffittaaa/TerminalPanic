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
    [SerializeField] private Vector3 newPosition;
    [SerializeField] private float time;

    private void Start()
    {
        newPosition = viewPoint.transform.position;
        time = 0f;
    }

    private void FixedUpdate()
    {
        if (!anxietyBar.interacted)
        {
            if (playerMovementScript.CurrentType() != BehaviorType.Jumping && playerMovementScript.CurrentType() != BehaviorType.Idleing)
            {
                if (playerMovementScript.CurrentType() == BehaviorType.Crouching && speed != crouchSpeed)
                {
                    speed = crouchSpeed;
                }
                else if (playerMovementScript.CurrentType() == BehaviorType.Runing && speed != runSpeed)
                {
                    speed = runSpeed;
                }
                else if (playerMovementScript.CurrentType() == BehaviorType.Walking && speed != walkSpeed)
                {
                    speed = walkSpeed;
                }
                DetorEffect();
            }
            else
            {
                time = 0f;

                newPosition.y = Mathf.Lerp(transform.position.y, viewPoint.transform.position.y, Time.fixedDeltaTime * speed);
                newPosition.z = Mathf.Lerp(transform.position.z, viewPoint.transform.position.z, Time.fixedDeltaTime * speed);
                transform.position = new Vector3(viewPoint.transform.position.x, newPosition.y, newPosition.z);
            }
        }
    }

    private void DetorEffect()
    {
        time += Time.fixedDeltaTime;
        newPosition.y = Mathf.Lerp(newPosition.y, viewPoint.transform.position.y + headDetor * Mathf.Sin(time * speed), Time.fixedDeltaTime * speed);
        newPosition.z = Mathf.Lerp(newPosition.z, viewPoint.transform.position.z + headDetor * Mathf.Cos(time * speed), Time.fixedDeltaTime * speed);
        transform.position = new Vector3(viewPoint.transform.position.x, newPosition.y, newPosition.z);
    }
}
