using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateManager : MonoBehaviour, IPersistentData
{
    [SerializeField] private Transform camPivot;

    private CameraThirdPersonState thirdPersonState;
    private CameraFreeState freeCamState;
    private CameraDetachedState detachedState;

    private CameraState currentState;

    private bool hasSavedMouseAxisValues;

    private void Start()
    {
        thirdPersonState = new CameraThirdPersonState(camPivot);
        freeCamState = new CameraFreeState(camPivot);
        detachedState = new CameraDetachedState();

        currentState = thirdPersonState;
        currentState.EnterState(this);

        if(hasSavedMouseAxisValues)
            thirdPersonState.InitSavedMouseAxisValues();
    }

    private void LateUpdate()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(CameraState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void LoadData(GamePersistentData sceneSaveData)
    {
        hasSavedMouseAxisValues = (sceneSaveData != null);
    }

    public void SaveData(ref GamePersistentData sceneSaveData)
    {
        sceneSaveData.GetPlayerSaveData().SetMouseAxisCamRotation(thirdPersonState.GetMouseAxisCamRotation());
    }

    public CameraState GetCurrentState() { return currentState; }

    public CameraThirdPersonState GetThirdPersonState() {  return thirdPersonState; }

    public CameraFreeState GetFreeCamState() {  return freeCamState; }

    public CameraDetachedState GetDetachedState() {  return detachedState; }
}
