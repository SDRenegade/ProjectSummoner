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
    private int currentLookAtByPathIndex;

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
            if (currentLookAtByPathIndex == 0) {
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
            currentLookAtByPathIndex = 0;
        }

        this.isActive = isActive;
    }
}
