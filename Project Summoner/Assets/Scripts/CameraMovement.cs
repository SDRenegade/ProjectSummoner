using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform CamBase;
    [SerializeField]
    private Transform PlayerTransform;
    [SerializeField]
    private float RotationSpeed;

    private float mouseX;
    private float mouseY;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -60, 50);

        Debug.Log("mouse X: " + mouseX);
        Debug.Log("mouse Y: " + mouseY);

        transform.LookAt(CamBase);

        CamBase.rotation = Quaternion.Euler(mouseY, mouseX, 0);
    }
}
