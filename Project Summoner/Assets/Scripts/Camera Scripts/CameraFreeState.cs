using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeState : CameraState
{
    private const float DEFAULT_ROTATION_SPEED = 2.75f;
    private const int CAM_UPPER_PITCH_CLAMP = 80;
    private const int CAM_LOWER_PITCH_CLAMP = -80;
    private readonly Vector3 CAM_STARTING_VECTOR = new Vector3(0, 0.5f, 1);
    private const float CAM_STARTING_DISTANCE = 1.5f;
    private const float STANDARD_SPEED = 30f;
    private const float SPRINT_SPEED = 60f;
    private const float VERTICAL_SPEED = 25f;

    private Transform camPivot;
    private float rotationSpeed;

    private float mouseX;
    private float mouseY;

    public CameraFreeState(Transform camPivot)
    {
        this.camPivot = camPivot;
        rotationSpeed = DEFAULT_ROTATION_SPEED;
    }

    public override void EnterState(CameraStateManager camManager)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        camManager.transform.position = camPivot.position + (Quaternion.Euler(0, camPivot.parent.transform.eulerAngles.y, 0) * CAM_STARTING_VECTOR.normalized * CAM_STARTING_DISTANCE);
        mouseX = camPivot.parent.transform.eulerAngles.y;
        mouseY = 0;
    }

    public override void UpdateState(CameraStateManager camManager)
    {
        //Camera Rotation
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, CAM_LOWER_PITCH_CLAMP, CAM_UPPER_PITCH_CLAMP);

        camManager.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

        //Camera Movement
        float xAxisRaw = Input.GetAxisRaw("Horizontal");
        float zAxisRaw = Input.GetAxisRaw("Vertical");

        float moveSpeed = (Input.GetKey("q")) ? SPRINT_SPEED : STANDARD_SPEED;

        float verticalVector = 0f;
        if (Input.GetKey(KeyCode.Space))
            verticalVector = VERTICAL_SPEED;
        else if(Input.GetKey(KeyCode.LeftShift))
            verticalVector = -VERTICAL_SPEED;

        Vector3 horizontalVector = new Vector3(xAxisRaw, 0, zAxisRaw).normalized * moveSpeed;
        Vector3 moveVector = new Vector3(horizontalVector.x, verticalVector, horizontalVector.z);
        camManager.transform.position = camManager.transform.position + (Quaternion.Euler(0, camManager.transform.eulerAngles.y, 0) * moveVector * Time.deltaTime);
    }
}
