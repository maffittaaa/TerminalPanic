using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum BehaviorType { Idleing, Walking, Running, Crouching }

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
    [SerializeField] private Animator anim;
    [SerializeField] private AudioManager audioManager;
    public float initialSpeed { get; private set; }

    private bool crouched;

    [SerializeField] private float horizontalAxis;
    [SerializeField] private float verticalAxis;

    [field:SerializeField] public BehaviorType behaviorType { get; private set; }
    [SerializeField] private AnxietyBar anxietyBar;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;
        behaviorType = BehaviorType.Idleing;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (!anxietyBar.interacted && gameManager.state != IGameStates.Paused)
        {
            Move();
        }
        else 
        {
            anim.SetTrigger("idle");
            speed = 0;
            rb.velocity = Vector3.zero;
            if (audioManager.runPlayer.isPlaying || audioManager.walkPlayer.isPlaying || audioManager.crouchPlayer.isPlaying)
            {
                audioManager.runPlayer.Stop();
                audioManager.crouchPlayer.Stop();
                audioManager.walkPlayer.Stop();
            }
        }
    }

    private void Move()
    {
        Movement();
        Crouch();
        
        if (crouched)
        {
            behaviorType = BehaviorType.Crouching;
            if (!audioManager.crouchPlayer.isPlaying)
            {
                audioManager.walkPlayer.Stop();
                audioManager.runPlayer.Stop();
                audioManager.crouchPlayer.Play();
            }

            if (horizontalAxis != 0 || verticalAxis != 0)
            {
                anim.SetBool("running", false);
                anim.SetBool("idle", false);
                anim.SetBool("walking", true);
            }
        }
        else if (Sprint())
        {
            behaviorType = BehaviorType.Running;

            if (!audioManager.runPlayer.isPlaying)
            {
                anim.SetBool("walking", false);
                anim.SetBool("idle", false);
                anim.SetBool("running", true);
                audioManager.crouchPlayer.Stop();
                audioManager.walkPlayer.Stop();
                audioManager.runPlayer.Play();
            }
        }
        else if (horizontalAxis != 0 || verticalAxis != 0)
        {
            behaviorType = BehaviorType.Walking;

            if (!audioManager.walkPlayer.isPlaying)
            {
                anim.SetBool("running", false);
                anim.SetBool("idle", false);
                anim.SetBool("walking", true);
                audioManager.runPlayer.Stop();
                audioManager.crouchPlayer.Stop();
                audioManager.walkPlayer.Play();
            }
        }

        if (horizontalAxis == 0 && verticalAxis == 0)
        {
            behaviorType = BehaviorType.Idleing;

            if (audioManager.runPlayer.isPlaying || audioManager.walkPlayer.isPlaying || audioManager.crouchPlayer.isPlaying)
            {
                anim.SetBool("running", false);
                anim.SetBool("walking", false);
                anim.SetBool("idle", true);
                audioManager.runPlayer.Stop();
                audioManager.crouchPlayer.Stop();
                audioManager.walkPlayer.Stop();
            }
        }
    }

    public bool KeysRealeased()
    {
        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            return true;
        }
        return false;
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
            else
                speed = initialSpeed;
        }
        return false;
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetBool("crouching", true);
            crouched = true;
            speed = initialSpeed * crouchMultiplier;
            viewPoint.transform.position = new Vector3(viewPoint.transform.position.x, viewPoint.transform.position.y - crouchView, viewPoint.transform.position.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetBool("crouching", false);
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
