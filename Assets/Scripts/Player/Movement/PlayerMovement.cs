using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum BehaviorType { Idleing, Walking, Runing, Jumping, Crouching }

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [field: SerializeField] public float speed { get; set; }

    [SerializeField] private float playerHeigth;
    [SerializeField] private float jumpTolerance;
    [SerializeField] private float crouchMultiplier;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private GameObject viewPoint;
    [SerializeField] private float crouchView;
    [SerializeField] private float jumpForce;
    public float initialSpeed { get; private set; }

    private bool crouched;

    [SerializeField] private float horizontalAxis;
    [SerializeField] private float verticalAxis;

    [field:SerializeField] public BehaviorType behaviorType { get; private set; }
    [SerializeField] private AnxietyBar anxietyBar;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;
        behaviorType = BehaviorType.Idleing;
    }

    private void Update()
    {
        Movement();
        Crouch();
        if (Jump())
        {
            behaviorType = BehaviorType.Jumping;
        }
        else if (crouched)
        {
            behaviorType = BehaviorType.Crouching;
        }
        else if (Sprint()) 
        {
            behaviorType = BehaviorType.Runing;
        }
        else if (horizontalAxis != 0 || verticalAxis != 0)
        {
            behaviorType = BehaviorType.Walking;
        }

        if (horizontalAxis == 0 && verticalAxis == 0)
        {
            behaviorType = BehaviorType.Idleing;
        }
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
    
    private bool Sprint()
    {
        if (horizontalAxis != 0 || verticalAxis != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (initialSpeed * sprintMultiplier > speed)
                    speed *= sprintMultiplier;
                return true;
            }
            else if (!anxietyBar.interacted)
                speed = initialSpeed;
        }
        return false;
    }

    private bool Jump()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(0, -1, 0), out hit, playerHeigth + jumpTolerance))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouched = true;
            speed = initialSpeed * crouchMultiplier;
            viewPoint.transform.position = new Vector3(viewPoint.transform.position.x, viewPoint.transform.position.y - crouchView, viewPoint.transform.position.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouched = false;
            speed = initialSpeed;
            viewPoint.transform.position = new Vector3(viewPoint.transform.position.x, viewPoint.transform.position.y + crouchView, viewPoint.transform.position.z);
        }
    }

    public BehaviorType CurrentType()
    {
        return behaviorType;
    }
}
