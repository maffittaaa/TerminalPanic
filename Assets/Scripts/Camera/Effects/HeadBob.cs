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
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        newPosition = viewPoint.transform.position;
        time = 0f;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        if (!anxietyBar.interacted && gameManager.state != IGameStates.Paused)
        {
            BobEffect();
        }
    }
    private void BobEffect() 
    {
        if (playerMovementScript.CurrentType() != BehaviorType.Idleing)
        {
            if (playerMovementScript.CurrentType() == BehaviorType.Crouching && speed != crouchSpeed)
            {
                speed = crouchSpeed;
            }
            else if (playerMovementScript.CurrentType() == BehaviorType.Running && speed != runSpeed)
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
            transform.position = new Vector3(viewPoint.transform.position.x, newPosition.y, viewPoint.transform.position.z);
        }
    }

    private void DetorEffect()
    {
        time += Time.fixedDeltaTime;
        newPosition.y = Mathf.Lerp(newPosition.y, viewPoint.transform.position.y + headDetor * Mathf.Sin(time * speed), Time.fixedDeltaTime * speed);
        transform.position = new Vector3(viewPoint.transform.position.x, newPosition.y, viewPoint.transform.position.z);
    }
}
