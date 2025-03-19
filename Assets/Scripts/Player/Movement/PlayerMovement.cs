using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorType { Idleing , Walking, Runing }

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int speed;
    [SerializeField] private int sprintMultiplier;
    [SerializeField] private int jumpForce;
    private int initialSpeed;

    [SerializeField] private float horizontalAxis;
    [SerializeField] private float verticalAxis;

    private BehaviorType behaviorType;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;
        behaviorType = BehaviorType.Idleing;
    }

    private void Update()
    {
        //Movement
        Movement();
        Sprint();

        //Jump
        Jump();
    }

    private void Movement()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        Vector3 moveDirection;

        moveDirection = new Vector3(horizontalAxis * speed, rb.velocity.y, verticalAxis * speed);
        moveDirection = transform.TransformDirection(moveDirection);

        rb.velocity = moveDirection;
    }    
    
    private void Sprint()
    {
        if (horizontalAxis != 0 || verticalAxis != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                behaviorType = BehaviorType.Runing;
                if (initialSpeed * sprintMultiplier > speed)
                {
                    speed *= sprintMultiplier;
                }
            }
            else
            {
                behaviorType = BehaviorType.Walking;
                speed = initialSpeed;
            }
        }
        else
        {
            behaviorType = BehaviorType.Idleing;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    public BehaviorType CurrentType()
    {
        return behaviorType;
    }
}
