using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    [SerializeField] private ThirdPersonMovement thirdPersonMovement;
    [SerializeField] private CameraStateManager cameraStateManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            if(cameraStateManager.GetCurrentState() == cameraStateManager.GetThirdPersonState()) {
                cameraStateManager.SwitchState(cameraStateManager.GetFreeCamState());
                thirdPersonMovement.SetIsMovementEnabled(false);
            }
            else if(cameraStateManager.GetCurrentState() == cameraStateManager.GetFreeCamState()) {
                cameraStateManager.SwitchState(cameraStateManager.GetThirdPersonState());
                thirdPersonMovement.SetIsMovementEnabled(true);
            }
        }
    }
}
