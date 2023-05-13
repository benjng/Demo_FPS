using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        GroundedCheck();

        // Gather Keyboard inputs
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        MoveCharacter(xInput, zInput);
        CalGravity();
    }

    void MoveCharacter(float xInput, float zInput){
        // transform.right/forward: right/front local dir of the player, not world x/z axies
        Vector3 moveDir = transform.right * xInput + transform.forward * zInput;
        controller.Move(moveDir * speed * Time.deltaTime);
    }

    void CalGravity(){
        // y = 0.5*g * t^2
        velocity.y += gravity * (Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    void GroundedCheck(){
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0){
            velocity.y = -2f; // constantly forcing the player falls
        }
    }
}
