using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private Spline spline;
    [SerializeField] private float speed;
    [SerializeField] private bool isFollowForward;
    //[SerializeField] private bool isFollowNormal;

    private float moveAmount;

    private void Update()
    {
        moveAmount += speed * Time.deltaTime;

        transform.position = spline.GetPositionAt(moveAmount);
        if(isFollowForward)
            transform.forward = spline.GetForwardAt(moveAmount);
    }
}
