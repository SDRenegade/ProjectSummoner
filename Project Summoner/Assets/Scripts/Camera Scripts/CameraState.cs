using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraState
{
    public abstract void EnterState(CameraStateManager camManager);
    public abstract void UpdateState(CameraStateManager camManager);
}
