using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int speed;
    [SerializeField] private int jumpForce;

    [SerializeField] private float horizontalAxis;
    [SerializeField] private float verticalAxis;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Movement
        Movement();

        //Jump
        Jump();
    }

    private void Movement()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");


        Vector3 moveDirection = Vector3.zero;

        moveDirection = new Vector3(horizontalAxis * speed, rb.velocity.y, verticalAxis * speed);
        moveDirection = transform.TransformDirection(moveDirection);

        rb.velocity = moveDirection;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }
}
