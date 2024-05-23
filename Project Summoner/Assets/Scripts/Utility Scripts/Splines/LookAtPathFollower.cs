using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPathFollower : MonoBehaviour
{
    public event EventHandler<EventArgs> OnEndOfPath;

    [SerializeField] private List<LookAtPath> lookAtPathList;
    [SerializeField] private float speed;
    [SerializeField] private bool isLoop;
    [SerializeField] private bool isActive;

    private float moveAmount;
    private int currentPathIndex;

    private void Update()
    {
        if(!isActive)
            return;

        moveAmount = moveAmount + (speed * Time.deltaTime) < lookAtPathList[currentPathIndex].GetPath().GetVertexPathLength() ?
            moveAmount + (speed * Time.deltaTime) : lookAtPathList[currentPathIndex].GetPath().GetVertexPathLength();

        transform.position = lookAtPathList[currentPathIndex].GetPath().GetPositionAt(moveAmount);
        transform.LookAt(lookAtPathList[currentPathIndex].GetLookAtPosition());

        if(moveAmount >= lookAtPathList[currentPathIndex].GetPath().GetVertexPathLength()) {
            moveAmount = 0;
            currentPathIndex = (currentPathIndex + 1) % lookAtPathList.Count;
            if (currentPathIndex == 0) {
                OnEndOfPath?.Invoke(this, EventArgs.Empty);
                if(!isLoop)
                    isActive = false;
            }
        }
    }

    public bool IsLoop() { return isLoop; }

    public void SetIsLoop(bool isLoop) {  this.isLoop = isLoop; }

    public bool IsActive() { return isActive; }

    public void SetIsActive(bool isActive)
    {
        if(!isActive) {
            moveAmount = 0;
            currentPathIndex = 0;
        }

        this.isActive = isActive;
    }
}
