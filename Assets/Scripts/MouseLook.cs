using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Gather mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        RotateCharLeftRight(mouseX);
        RotateCharUpDown(mouseY);
    }

    void RotateCharLeftRight(float mouseX){
        // Rotate around Y-Axis of the player body
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void RotateCharUpDown(float mouseY){
        // Rotate around X-Axis of camera
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
