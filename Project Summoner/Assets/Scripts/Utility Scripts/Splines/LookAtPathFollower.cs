using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPathFollower : MonoBehaviour
{
    [SerializeField] private List<LookAtPath> lookAtPathList;
    [SerializeField] private float speed;

    private float moveAmount;
    private int currentLookAtByPathIndex;
    private bool isActive;

    private void Start()
    {
        isActive = true;
    }

    private void Update()
    {
        if(!isActive)
            return;

        moveAmount = moveAmount + (speed * Time.deltaTime) < lookAtPathList[currentLookAtByPathIndex].GetPath().GetVertexPathLength() ?
            moveAmount + (speed * Time.deltaTime) : lookAtPathList[currentLookAtByPathIndex].GetPath().GetVertexPathLength();

        transform.position = lookAtPathList[currentLookAtByPathIndex].GetPath().GetPositionAt(moveAmount);
        transform.LookAt(lookAtPathList[currentLookAtByPathIndex].GetLookAtPosition());

        if(moveAmount >= lookAtPathList[currentLookAtByPathIndex].GetPath().GetVertexPathLength()) {
            moveAmount = 0;
            currentLookAtByPathIndex = (currentLookAtByPathIndex + 1) % lookAtPathList.Count;
        }
    }

    public bool IsActive() { return isActive; }

    public void SetIsActive(bool isActive)
    {
        if(!isActive) {
            moveAmount = 0;
            currentLookAtByPathIndex = 0;
        }

        this.isActive = isActive;
    }
}
