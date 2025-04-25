using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightBob : MonoBehaviour
{
    [SerializeField] private float headDetor;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float flashlightSpeed;
    [SerializeField] private GameObject viewPoint;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private AnxietyBar anxietyBar;
    [SerializeField] private Vector3 initialDist;
    [SerializeField] private Vector3 currentInitialPosition;
    [SerializeField] private Vector3 newPosition;
    [SerializeField] private float time;

    private void Start()
    {
        newPosition = viewPoint.transform.position;
        initialDist = viewPoint.transform.position - transform.position;
        time = 0f;
    }

    private void FixedUpdate()
    {
        currentInitialPosition = viewPoint.transform.position + initialDist;

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
                newPosition.y = Mathf.Lerp(transform.position.y, currentInitialPosition.y, Time.fixedDeltaTime * speed);
                transform.position = new Vector3(currentInitialPosition.x, newPosition.y, currentInitialPosition.z);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 10);
            }
        }
    }

    private void DetorEffect()
    {
        time += Time.fixedDeltaTime;
        newPosition.y = Mathf.Lerp(newPosition.y, currentInitialPosition.y + headDetor * Mathf.Sin(time * speed), Time.fixedDeltaTime * speed);
        transform.position = new Vector3(currentInitialPosition.x, newPosition.y, currentInitialPosition.z);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(15, 0, 0), Time.fixedDeltaTime * speed);
    }
}
