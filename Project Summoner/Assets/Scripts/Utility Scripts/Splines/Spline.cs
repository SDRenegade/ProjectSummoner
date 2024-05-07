using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public event EventHandler OnDirty;

    [SerializeField] private Vector3 normal = new Vector3(0, 0, -1);
    [SerializeField] private bool isClosedLoop;
    [SerializeField] private List<SplineAnchor> anchorList;

    [SerializeField] private bool isVisableWhenNotSelected;

    private List<SplinePoint> pointList;
    private float pointAmtInCurve;
    private float pointAmountPerUnitInCurve = 2f;

    private void Awake()
    {
        SetupPointList();
    }

    public Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    public Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 abc = QuadraticLerp(a, b, c, t);
        Vector3 bcd = QuadraticLerp(b, c, d, t);

        return Vector3.Lerp(abc, bcd, t);
    }

    public Vector3 GetPositionAt(float t)
    {
        if (anchorList == null)
            return Vector3.zero;
        if (anchorList.Count < 2)
            return anchorList[0].position;

        SplineAnchor anchorA, anchorB;
        if (t == 1) {
            // Full position, special case
            if (isClosedLoop) {
                anchorA = anchorList[anchorList.Count - 1];
                anchorB = anchorList[0];
            }
            else {
                anchorA = anchorList[anchorList.Count - 2];
                anchorB = anchorList[anchorList.Count - 1];
            }
            return transform.position + CubicLerp(anchorA.position, anchorA.controlBPosition, anchorB.controlAPosition, anchorB.position, t);
        }
        else {
            int addClosedLoop = isClosedLoop ? 1 : 0;
            float tFull = t * (anchorList.Count - 1 + addClosedLoop);
            int anchorIndex = Mathf.FloorToInt(tFull);
            float tAnchor = tFull - anchorIndex;

            if (anchorIndex < anchorList.Count - 1) {
                anchorA = anchorList[anchorIndex];
                anchorB = anchorList[anchorIndex + 1]; // Doesn't ever result in an out of bounds error since we know t != 1
            }
            else {
                // anchorIndex is final one, either don't link to "next" one or loop back
                if (isClosedLoop) {
                    anchorA = anchorList[anchorList.Count - 1];
                    anchorB = anchorList[0];
                }
                else {
                    // *** Remove if not being hit ***
                    Debug.Log("Odd case being hit");
                    anchorA = anchorList[anchorIndex - 1];
                    anchorB = anchorList[anchorIndex];
                    tAnchor = 1f;
                }
            }

            return transform.position + CubicLerp(anchorA.position, anchorA.controlBPosition, anchorB.controlAPosition, anchorB.position, tAnchor);
        }
    }

    public Vector3 GetForwardAt(float t)
    {
        SplinePoint pointA = GetPreviousPoint(t);
        int pointBIndex;

        pointBIndex = (pointList.IndexOf(pointA) + 1) % pointList.Count;
        SplinePoint pointB = pointList[pointBIndex];

        return Vector3.Lerp(pointA.forward, pointB.forward, (t - pointA.t) / Mathf.Abs(pointA.t - pointB.t));
    }

    public SplinePoint GetPreviousPoint(float t)
    {
        int previousIndex = 0;
        for (int i = 1; i < pointList.Count; i++) {
            SplinePoint point = pointList[i];
            if (t < point.t) {
                return pointList[previousIndex];
            }
            else {
                previousIndex = i;
            }
        }
        return pointList[previousIndex];
    }

    public SplinePoint GetClosestPoint(float t)
    {
        SplinePoint closestPoint = pointList[0];
        foreach (SplinePoint point in pointList) {
            if (Mathf.Abs(t - point.t) < Mathf.Abs(t - closestPoint.t)) {
                closestPoint = point;
            }
        }
        return closestPoint;
    }

    private void SetupPointList()
    {
        pointList = new List<SplinePoint>();
        pointAmtInCurve = pointAmountPerUnitInCurve * GetSplineLength();
        for (float t = 0; t < 1f; t += 1f / pointAmtInCurve) {
            pointList.Add(new SplinePoint {
                t = t,
                position = GetPositionAt(t),
                normal = normal,
            });
        }

        pointList.Add(new SplinePoint {
            t = 1f,
            position = GetPositionAt(1f),
        });

        UpdateForwardVectors();
    }

    private void UpdatePointList()
    {
        if (pointList == null)
            return;

        foreach (SplinePoint point in pointList)
            point.position = GetPositionAt(point.t);

        UpdateForwardVectors();
    }

    private void UpdateForwardVectors()
    {
        if (pointList == null || pointList.Count == 0)
            return;

        // Set forward vectors
        for (int i = 0; i < pointList.Count - 1; i++) {
            // Set final forward vector
            if (i == pointList.Count - 1 && isClosedLoop)
                pointList[i].forward = (pointList[i + 1].position - pointList[i].position).normalized;
            else
                pointList[i].forward = (pointList[i + 1].position - pointList[i].position).normalized;
        }
    }

    public float GetSplineLength(float stepSize = .01f)
    {
        float splineLength = 0f;

        Vector3 lastPosition = GetPositionAt(0f);

        for (float t = 0; t < 1f; t += stepSize) {
            splineLength += Vector3.Distance(lastPosition, GetPositionAt(t));

            lastPosition = GetPositionAt(t);
        }

        splineLength += Vector3.Distance(lastPosition, GetPositionAt(1f));

        return splineLength;
    }

    public void AddAnchor()
    {
        if (anchorList == null)
            anchorList = new List<SplineAnchor>();

        if (anchorList.Count == 0) {
            anchorList.Add(new SplineAnchor {
                position = new Vector3(0, 0, 0),
                controlAPosition = new Vector3(3f, 0, 0),
                controlBPosition = new Vector3(-3f, 0, 0),
            });
        }
        else {
            SplineAnchor lastAnchor = anchorList[anchorList.Count - 1];
            anchorList.Add(new SplineAnchor {
                position = lastAnchor.position + new Vector3(3f, 0, 0),
                controlAPosition = lastAnchor.controlAPosition + new Vector3(3f, 0, 0),
                controlBPosition = lastAnchor.controlBPosition + new Vector3(3f, 0, 0),
            });
        }
    }

    public void AddAnchor(Vector3 worldPosition)
    {
        if (anchorList == null)
            anchorList = new List<SplineAnchor>();

        Vector3 localPosition = worldPosition - transform.position;
        if (anchorList.Count == 0) {
            anchorList.Add(new SplineAnchor {
                position = localPosition,
                controlAPosition = localPosition + new Vector3(3f, 0, 0),
                controlBPosition = localPosition + new Vector3(-3f, 0, 0)
            });
        }
        else {
            SplineAnchor lastAnchor = anchorList[anchorList.Count - 1];
            anchorList.Add(new SplineAnchor {
                position = localPosition,
                controlAPosition = (lastAnchor.controlAPosition - lastAnchor.position) + localPosition,
                controlBPosition = (lastAnchor.controlBPosition - lastAnchor.position) + localPosition
            });
        }
    }

    public void RemoveAnchorAt(int index)
    {
        if(index >=  anchorList.Count)
            return;

        anchorList.RemoveAt(index);
        Debug.Log("Anchor has been removed");
    }

    public void RemoveLastAnchor()
    {
        if (anchorList == null) {
            anchorList = new List<SplineAnchor>();
            return;
        }

        anchorList.RemoveAt(anchorList.Count - 1);
        Debug.Log("Anchor has been removed");
    }

    public List<SplineAnchor> GetAnchorList() { return anchorList; }


    public List<SplinePoint> GetPointList() { return pointList; }

    public bool IsClosedLoop() { return isClosedLoop; }

    public void SetAllZZero()
    {
        foreach (SplineAnchor anchor in anchorList) {
            anchor.position = new Vector3(anchor.position.x, anchor.position.y, 0f);
            anchor.controlAPosition = new Vector3(anchor.controlAPosition.x, anchor.controlAPosition.y, 0f);
            anchor.controlBPosition = new Vector3(anchor.controlBPosition.x, anchor.controlBPosition.y, 0f);
        }
    }

    public void SetAllYZero()
    {
        foreach (SplineAnchor anchor in anchorList) {
            anchor.position = new Vector3(anchor.position.x, 0f, anchor.position.z);
            anchor.controlAPosition = new Vector3(anchor.controlAPosition.x, 0f, anchor.controlAPosition.z);
            anchor.controlBPosition = new Vector3(anchor.controlBPosition.x, 0f, anchor.controlBPosition.z);
        }
    }

    public void SetDirty()
    {
        UpdatePointList();

        OnDirty?.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        GameObject selectedObj = UnityEditor.Selection.activeGameObject;
        if (!isVisableWhenNotSelected || selectedObj == gameObject)
            return;

        if(anchorList != null) {
            Gizmos.color = Color.green;
            for(int i = 0; i < anchorList.Count; i++) {
                Debug.Log("In point loop");
                int nextPoint = i + 1;
                if(nextPoint >= anchorList.Count) {
                    if (isClosedLoop)
                        nextPoint %= anchorList.Count;
                    else
                        break;
                }
                Gizmos.DrawLine(transform.position + anchorList[i].position, transform.position + anchorList[nextPoint].position);
            }
        }
    }
}

[Serializable]
public class SplineAnchor
{
    public Vector3 position;
    public Vector3 controlAPosition;
    public Vector3 controlBPosition;
}

[Serializable]
public class SplinePoint
{
    public float t;
    public Vector3 position;
    public Vector3 forward;
    public Vector3 normal;
}