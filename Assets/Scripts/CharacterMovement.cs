using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 10f;
    // Update is called once per frame
    void Update()
    {
        // Gather Keyboard inputs
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        
        Vector3 moveDir = transform.right * xInput + transform.forward * zInput;
        controller.Move(moveDir * speed * Time.deltaTime);
    }
}
