using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorType { Idleing, Walking, Runing, Jumping, Crouching }

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float crouchMultiplier;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private GameObject viewPoint;
    [SerializeField] private float crouchView;
    private bool crouched;
    [SerializeField] private float jumpForce;
    private float initialSpeed;

    [SerializeField] private float horizontalAxis;
    [SerializeField] private float verticalAxis;

    private int jumpCount = 0;

    [SerializeField] private BehaviorType behaviorType;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;
        behaviorType = BehaviorType.Idleing;
    }

    private void Update()
    {
        Movement();
        Sprint();
        Jump();
        Crouch();
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
        if (horizontalAxis != 0 || verticalAxis != 0 && !crouched)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (initialSpeed * sprintMultiplier > speed)
                {
                    speed *= sprintMultiplier;
                }

                if (jumpCount == 0)
                {
                    behaviorType = BehaviorType.Runing;
                }
            }
            else
            {
                if (jumpCount == 0)
                {
                    behaviorType = BehaviorType.Walking;
                }
                speed = initialSpeed;
            }
        }
        else if (jumpCount == 0)
        {
            behaviorType = BehaviorType.Idleing;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0)
        {
            jumpCount++;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        if(jumpCount > 0)
        {
            behaviorType = BehaviorType.Jumping;
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            speed = initialSpeed * crouchMultiplier;
            crouched = true;
            viewPoint.transform.position = new Vector3(viewPoint.transform.position.x, viewPoint.transform.position.y - crouchView, viewPoint.transform.position.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            speed = initialSpeed;
            crouched = false;
            viewPoint.transform.position = new Vector3(viewPoint.transform.position.x, viewPoint.transform.position.y + crouchView, viewPoint.transform.position.z);
        }
    }

    public BehaviorType CurrentType()
    {
        return behaviorType;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}
