using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class SplineFollower : MonoBehaviour
{
    private readonly float MAX_MOVEMENT = 1f;

    [SerializeField] private Spline spline;
    [SerializeField] private float speed;

    private float moveAmount;

    private void Update()
    {
        moveAmount = (moveAmount + (Time.deltaTime * speed)) % MAX_MOVEMENT;

        transform.position = spline.GetPositionAt(moveAmount);
        transform.forward = spline.GetForwardAt(moveAmount);
    }
}
