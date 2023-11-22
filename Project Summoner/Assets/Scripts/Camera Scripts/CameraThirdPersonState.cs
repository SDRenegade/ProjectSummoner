using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class CameraThirdPersonState : CameraState
{
    private const float DEFAULT_NATURAL_CAM_DISTANCE = 6.0f;
    private const float DEFAULT_CAM_ROTATION_SPEED = 2.75f;
    private const float DEFAULT_CAM_PITCH = 20.0f;
    private const int CAM_UPPER_PITCH_CLAMP = 60;
    private const int CAM_LOWER_PITCH_CLAMP = -40;
    private const float OBSTACLE_PADDING = 0.5f;

    private Transform camPivot;
    private float naturalCamDistance;
    private float rotationSpeed;
    private LayerMask layerMask;

    private float mouseX;
    private float mouseY;

    public CameraThirdPersonState(Transform camPivot)
    {
        this.camPivot = camPivot;
        naturalCamDistance = DEFAULT_NATURAL_CAM_DISTANCE;
        rotationSpeed = DEFAULT_CAM_ROTATION_SPEED;
        layerMask = LayerMask.GetMask("Ground");
    }

    public override void EnterState(CameraStateManager camManager)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Setting initial camera position
        mouseX = camPivot.parent.transform.eulerAngles.y;
        mouseY = DEFAULT_CAM_PITCH;
    }

    public override void UpdateState(CameraStateManager camManager)
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, CAM_LOWER_PITCH_CLAMP, CAM_UPPER_PITCH_CLAMP);

        Vector3 CamPosition = camPivot.position + (Quaternion.Euler(mouseY, mouseX, 0f) * -Vector3.forward.normalized * naturalCamDistance);
        camManager.transform.position = CamPosition;

        camManager.transform.LookAt(camPivot);

        CameraCollision(camManager);
    }

    private void CameraCollision(CameraStateManager camManager)
    {
        Vector3 baseToCamVector = (camManager.transform.position - camPivot.transform.position).normalized;

        if (Physics.Raycast(camPivot.transform.position, baseToCamVector, out RaycastHit hitInfo, naturalCamDistance, layerMask)) {
            Vector3 paddedVector = hitInfo.point + ((camPivot.transform.position - hitInfo.point).normalized * OBSTACLE_PADDING);
            camManager.transform.position = paddedVector;
        }
        else
            camManager.transform.position = camPivot.transform.position + (baseToCamVector * naturalCamDistance);
    }

    public Vector2 GetMouseAxisCamRotation() { return new Vector2(mouseX, mouseY); }

    public void InitSavedMouseAxisValues()
    {
        mouseX = SaveSystem.GetInstance().GetGamePersistentData().GetPlayerSaveData().GetMouseAxisCamRotation().x;
        mouseY = SaveSystem.GetInstance().GetGamePersistentData().GetPlayerSaveData().GetMouseAxisCamRotation().y;
    }
}
