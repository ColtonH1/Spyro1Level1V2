using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    //moving
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    float startingSpeed;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    //falling
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;
    float gravity = -9.81f;


    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        startingSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        speed = Running();
        Moving();
        Falling();
    }

    private void Moving()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private float Running()
    {
        float moveSpeed = startingSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            return startingSpeed * 3;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = startingSpeed;
        }
        return moveSpeed;
    }

    private void Falling()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }


    /*//falling
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundDistance + .1f))
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
        else
        {
            isGrounded = false;
            Debug.Log("Not Grounded");
        }
    }*/

}
