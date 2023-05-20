using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float turnSmoothTime;
    [SerializeField]
    private float jumpVelocity;
    [SerializeField]
    private float gravity;

    private float turnSmoothVelocity;
    private Vector3 moveVelocity;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude > 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Rotates Vector3.forward targetAngle degrees around the Y-axis
            float temp = moveVelocity.y;
            moveVelocity = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * moveSpeed;
            moveVelocity.y = temp;
            //characterController.Move(moveVelocity * moveSpeed * Time.deltaTime);
        }
        else {
            moveVelocity.x = 0f;
            moveVelocity.z = 0f;
        }

        if (characterController.isGrounded) {
            moveVelocity.y = 0f;

            if (Input.GetKey("c"))
            {
                Debug.Log("Jump has been pressed");
                moveVelocity.y = jumpVelocity;
            }
        }

        moveVelocity.y += gravity * Time.deltaTime;

        characterController.Move(moveVelocity * Time.deltaTime);
    }
}
