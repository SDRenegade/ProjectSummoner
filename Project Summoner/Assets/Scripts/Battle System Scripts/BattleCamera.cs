using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    private readonly Vector3 CAMERA_POSITION_OFFSET = new Vector3(4f, 0, 5f);
    private readonly Vector3 LOOK_AT_OFFSET = new Vector3(0, 0, 1.5f);

    private Vector3? staticLookAt;

    private void Update()
    {
        if (staticLookAt != null)
            transform.LookAt((Vector3)staticLookAt);
    }

    public void SetStaticLookAt(Transform lookAtTarget, bool isPrimarySide)
    {
        SetStaticLookAt(lookAtTarget.position, lookAtTarget.eulerAngles, isPrimarySide);
    }

    public void SetStaticLookAt(Vector3 lookAtPosition, Vector3 lookAtEulerAngles, bool isPrimarySide)
    {
        Vector3 cameraPosOffset = CAMERA_POSITION_OFFSET;
        cameraPosOffset.x = isPrimarySide ? cameraPosOffset.x : -cameraPosOffset.x;

        transform.position = lookAtPosition + (Quaternion.Euler(lookAtEulerAngles.x, lookAtEulerAngles.y, lookAtEulerAngles.z).normalized * cameraPosOffset);
        staticLookAt = lookAtPosition + (Quaternion.Euler(lookAtEulerAngles.x, lookAtEulerAngles.y, lookAtEulerAngles.z).normalized * LOOK_AT_OFFSET);
    }
}
