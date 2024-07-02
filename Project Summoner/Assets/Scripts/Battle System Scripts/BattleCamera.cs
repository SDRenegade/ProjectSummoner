using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    private readonly Vector3 STATIC_CAMERA_POSITION_OFFSET = new Vector3(6f, 0, 4f);
    private readonly Vector3 ATTACK_CAMERA_POSITION_OFFSET = new Vector3(3.5f, 0, 5.5f);
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
        Vector3 cameraPosOffset = STATIC_CAMERA_POSITION_OFFSET;
        cameraPosOffset.x = isPrimarySide ? cameraPosOffset.x : -cameraPosOffset.x;

        transform.position = lookAtPosition + (Quaternion.Euler(lookAtEulerAngles.x, lookAtEulerAngles.y, lookAtEulerAngles.z).normalized * cameraPosOffset);
        staticLookAt = lookAtPosition + (Quaternion.Euler(lookAtEulerAngles.x, lookAtEulerAngles.y, lookAtEulerAngles.z).normalized * LOOK_AT_OFFSET);
    }

    public void SetAttackLookAt(Transform lookAtTarget, bool isPrimarySide)
    {
        SetAttackLookAt(lookAtTarget.position, lookAtTarget.eulerAngles, isPrimarySide);
    }

    public void SetAttackLookAt(Vector3 lookAtPosition, Vector3 lookAtEulerAngles, bool isPrimarySide)
    {
        Vector3 cameraPosOffset = ATTACK_CAMERA_POSITION_OFFSET;
        cameraPosOffset.x = isPrimarySide ? cameraPosOffset.x : -cameraPosOffset.x;

        transform.position = lookAtPosition + (Quaternion.Euler(lookAtEulerAngles.x, lookAtEulerAngles.y, lookAtEulerAngles.z).normalized * cameraPosOffset);
        staticLookAt = lookAtPosition + (Quaternion.Euler(lookAtEulerAngles.x, lookAtEulerAngles.y, lookAtEulerAngles.z).normalized * LOOK_AT_OFFSET);
    }
}
