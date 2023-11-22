using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour, IPersistentData
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float turnSmoothTime;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float gravity;
    [SerializeField] private float airborneSpeed;
    [SerializeField] private float airborneDeceleration;
    [SerializeField] private bool isMovementEnabled;

    private float moveSpeed;
    private float turnSmoothVelocity;
    private Vector3 moveVelocity;

    private void Update()
    {
        if (!isMovementEnabled)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude > 0.1f) {
            moveSpeed = (Input.GetKey("q")) ? runSpeed : walkSpeed;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Rotates Vector3.forward targetAngle degrees around the Y-axis
            Vector3 forwardVector = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * moveSpeed;
            moveVelocity.x = forwardVector.x;
            moveVelocity.z = forwardVector.z;
        }
        else if(characterController.isGrounded)
            moveVelocity = Vector3.zero;

        if(characterController.isGrounded) {
            moveVelocity.y = 0f;

            if (Input.GetKeyDown(KeyCode.Space))
                moveVelocity.y = jumpVelocity;
        }
        else {
            moveVelocity.y += gravity * Time.deltaTime;
        }

        moveVelocity.y += gravity * Time.deltaTime;

        characterController.Move(moveVelocity * Time.deltaTime);
    }

    //Override from ISaveData interface
    public void LoadData(GamePersistentData sceneSaveData)
    {
        if (sceneSaveData == null)
            return;

        transform.position = sceneSaveData.GetPlayerSaveData().GetPlayerPosition();
        transform.rotation = Quaternion.Euler(sceneSaveData.GetPlayerSaveData().GetPlayerRotation());
    }

    //Override from ISaveData interface
    public void SaveData(ref GamePersistentData sceneSaveData)
    {
        SaveSystem.GetInstance().GetGamePersistentData().GetPlayerSaveData().SetPlayerPosition(transform.position);
        SaveSystem.GetInstance().GetGamePersistentData().GetPlayerSaveData().SetPlayerRotation(transform.rotation.eulerAngles);
    }

    public bool GetIsMovementEnabled() { return isMovementEnabled; }
    
    public void SetIsMovementEnabled(bool isMovementEnabled) {  this.isMovementEnabled = isMovementEnabled; }
}   