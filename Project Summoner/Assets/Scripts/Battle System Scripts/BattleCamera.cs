using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    private readonly Vector3 STATIC_CAMERA_DISTANCE = new Vector3(5f, 0, 5f);
    private readonly Vector3 LOOK_AT_OFFSET = new Vector3(0, 0, 2f);

    private Vector3? staticLookAt;

    private void Update()
    {
        if (staticLookAt != null)
            transform.LookAt((Vector3)staticLookAt);
    }

    public void SetStaticLookAt(Transform lookAtTarget)
    {
        staticLookAt = lookAtTarget.position;
        transform.position = lookAtTarget.position + (Quaternion.Euler(lookAtTarget.eulerAngles.x, lookAtTarget.eulerAngles.y, lookAtTarget.eulerAngles.z).normalized * STATIC_CAMERA_DISTANCE);
        staticLookAt = lookAtTarget.position + (Quaternion.Euler(lookAtTarget.eulerAngles.x, lookAtTarget.eulerAngles.y, lookAtTarget.eulerAngles.z).normalized * LOOK_AT_OFFSET);
    }
}
