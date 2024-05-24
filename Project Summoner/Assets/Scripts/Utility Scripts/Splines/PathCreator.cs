using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    private const float MAX_ANGLE_ERROR = 0.3f;
    private const float MIN_VERTEX_DST = 0.01f;
    private const float ACCURACY = 10f;

    public event EventHandler OnDirty;

    [SerializeField] private Vector3 normal = new Vector3(0, 0, -1);
    [SerializeField] private bool isClosedLoop;
    [SerializeField] private bool isVisableWhenNotSelected;
    [SerializeField] private List<BezierSegment> bezierSegmentList;
    private VertexPath vertexPath;

    public void Awake()
    {
        UpdateVertexPath();
    }

    public void InitializeAnchorList()
    {
        if (bezierSegmentList == null)
            bezierSegmentList = new List<BezierSegment>();
        else
            bezierSegmentList.Clear();

        bezierSegmentList.Add(new BezierSegment());
        bezierSegmentList.Add(new BezierSegment());
        bezierSegmentList[0].anchorPos = new Vector3(-5, 0, 0);
        bezierSegmentList[0].controlAPos = new Vector3(-8, -3, 0);
        bezierSegmentList[0].controlBPos = new Vector3(-2, 3, 0);
        bezierSegmentList[1].anchorPos = new Vector3(5, 0, 0);
        bezierSegmentList[1].controlAPos = new Vector3(2, -3, 0);
        bezierSegmentList[1].controlBPos = new Vector3(8, 3, 0);

        UpdateVertexPath();
    }

    public VertexPath UpdateVertexPath()
    {
        if (vertexPath == null)
            vertexPath = new VertexPath();
        else
            vertexPath.Clear();

        vertexPath.vertices.Add(bezierSegmentList[0].anchorPos);
        vertexPath.tangents.Add(EvaluateCurveDerivative(bezierSegmentList[0], bezierSegmentList[1], 0));
        vertexPath.cumulativeLength.Add(0);

        Vector3 prevPointOnPath = bezierSegmentList[0].anchorPos;
        Vector3 lastAddedPoint = bezierSegmentList[0].anchorPos;
        float cumulativeLength = 0;
        float dstSinceLastVertex = 0;

        // Iterate through all bezier segments and split them into verticies
        for(int segmentIndex = 0; segmentIndex < GetNumSegments(); segmentIndex++) {
            float estimatedSegmentLength = EstimateBezierCurveLength(bezierSegmentList[segmentIndex], bezierSegmentList[(segmentIndex + 1) % bezierSegmentList.Count]);
            int numDivisions = Mathf.CeilToInt(estimatedSegmentLength * ACCURACY);
            float increment = 1f / numDivisions;

            for (float t = increment; t <= 1; t += increment) {
                bool isLastPointOnPath = (t + increment > 1 && segmentIndex == GetNumSegments() - 1);
                if (isLastPointOnPath)
                    t = 1;
                Vector3 pointOnPath = CubicLerp(bezierSegmentList[segmentIndex], bezierSegmentList[(segmentIndex + 1) % bezierSegmentList.Count], t);
                Vector3 nextPointOnPath = CubicLerp(bezierSegmentList[segmentIndex], bezierSegmentList[(segmentIndex + 1) % bezierSegmentList.Count], t + increment);

                dstSinceLastVertex += (pointOnPath - prevPointOnPath).magnitude;
                // angle at current point on path
                float localAngle = 180 - Vector3.Angle((prevPointOnPath - pointOnPath), (nextPointOnPath - pointOnPath));
                // angle between the last added vertex, the current point on the path, and the next point on the path
                float angleFromPrevVertex = 180 - Vector3.Angle((lastAddedPoint - pointOnPath), (nextPointOnPath - pointOnPath));
                float angleError = Mathf.Max(localAngle, angleFromPrevVertex);

                if ((angleError > MAX_ANGLE_ERROR && dstSinceLastVertex >= MIN_VERTEX_DST) || isLastPointOnPath) {
                    cumulativeLength += dstSinceLastVertex;
                    vertexPath.vertices.Add(pointOnPath);
                    vertexPath.tangents.Add(EvaluateCurveDerivative(bezierSegmentList[segmentIndex], bezierSegmentList[(segmentIndex + 1) % bezierSegmentList.Count], t).normalized);
                    vertexPath.cumulativeLength.Add(cumulativeLength);
                    dstSinceLastVertex = 0;
                    lastAddedPoint = pointOnPath;
                }

                prevPointOnPath = pointOnPath;
            }
        }

        return vertexPath;
    }

    // Get the position at a certain distance on the vertex path
    public Vector3 GetPositionAt(float dst)
    {
        dst = dst % vertexPath.cumulativeLength[vertexPath.cumulativeLength.Count - 1];

        int vertexSegemntIndex = 0;
        for(int i = 0; i < vertexPath.vertices.Count; i++) {
            if(dst <= vertexPath.cumulativeLength[i]) {
                vertexSegemntIndex = i - 1 >= 0 ? i - 1 : vertexPath.cumulativeLength.Count - 2;
                break;
            }
        }

        float segmentLength = vertexSegemntIndex == 0 ? vertexPath.cumulativeLength[0] :
            vertexPath.cumulativeLength[vertexSegemntIndex + 1] - vertexPath.cumulativeLength[vertexSegemntIndex];
        float dstOnSegment = dst - vertexPath.cumulativeLength[vertexSegemntIndex];
        float t = dstOnSegment / segmentLength;

        return transform.position + Vector3.Lerp(vertexPath.vertices[vertexSegemntIndex], vertexPath.vertices[vertexSegemntIndex + 1], t);
    }

    public Vector3 GetForwardAt(float dst)
    {
        dst = dst % vertexPath.cumulativeLength[vertexPath.cumulativeLength.Count - 1];

        int vertexSegemntIndex = 0;
        for (int i = 0; i < vertexPath.vertices.Count; i++) {
            if (dst <= vertexPath.cumulativeLength[i]) {
                vertexSegemntIndex = i - 1 >= 0 ? i - 1 : vertexPath.cumulativeLength.Count - 1;
                break;
            }
        }

        return vertexPath.tangents[vertexSegemntIndex];
    }

    public float GetVertexPathLength() { return vertexPath.cumulativeLength[vertexPath.cumulativeLength.Count - 1]; }

    public void AddAnchor()
    {
        if (bezierSegmentList == null)
            bezierSegmentList = new List<BezierSegment>();

        if (bezierSegmentList.Count == 0) {
            bezierSegmentList.Add(new BezierSegment {
                anchorPos = new Vector3(0, 0, 0),
                controlAPos = new Vector3(3f, 0, 0),
                controlBPos = new Vector3(-3f, 0, 0),
            });
        }
        else {
            BezierSegment lastAnchor = bezierSegmentList[bezierSegmentList.Count - 1];
            bezierSegmentList.Add(new BezierSegment {
                anchorPos = lastAnchor.anchorPos + new Vector3(3f, 0, 0),
                controlAPos = lastAnchor.controlAPos + new Vector3(3f, 0, 0),
                controlBPos = lastAnchor.controlBPos + new Vector3(3f, 0, 0),
            });
        }

        UpdateVertexPath();
    }

    public void AddAnchor(Vector3 worldPosition)
    {
        if (bezierSegmentList == null)
            bezierSegmentList = new List<BezierSegment>();

        Vector3 localPosition = worldPosition - transform.position;
        if (bezierSegmentList.Count == 0) {
            bezierSegmentList.Add(new BezierSegment {
                anchorPos = localPosition,
                controlAPos = localPosition + new Vector3(3f, 0, 0),
                controlBPos = localPosition + new Vector3(-3f, 0, 0)
            });
        }
        else {
            BezierSegment lastAnchor = bezierSegmentList[bezierSegmentList.Count - 1];
            bezierSegmentList.Add(new BezierSegment {
                anchorPos = localPosition,
                controlAPos = (lastAnchor.controlAPos - lastAnchor.anchorPos) + localPosition,
                controlBPos = (lastAnchor.controlBPos - lastAnchor.anchorPos) + localPosition
            });
        }

        UpdateVertexPath();
    }

    public void RemoveAnchorAt(int index)
    {
        if(bezierSegmentList == null)
            return;
        if (bezierSegmentList.Count <= 2) {
            Debug.LogWarning("You cannot remove an achor point from a bezier curve when there are 2 or less anchor points left");
            return;
        }

        index = Mathf.Clamp(index, 0, bezierSegmentList.Count - 1);
        bezierSegmentList.RemoveAt(index);

        UpdateVertexPath();
    }

    public void RemoveLastAnchor()
    {
        if (bezierSegmentList == null)
            return;
        if (bezierSegmentList.Count <= 2) {
            Debug.LogWarning("You cannot remove an achor point from a bezier curve when there are 2 or less anchor points left");
            return;
        }

        bezierSegmentList.RemoveAt(bezierSegmentList.Count - 1);

        UpdateVertexPath();
    }

    public void FlattenOnZ()
    {
        foreach (BezierSegment anchor in bezierSegmentList) {
            anchor.anchorPos = new Vector3(anchor.anchorPos.x, anchor.anchorPos.y, 0f);
            anchor.controlAPos = new Vector3(anchor.controlAPos.x, anchor.controlAPos.y, 0f);
            anchor.controlBPos = new Vector3(anchor.controlBPos.x, anchor.controlBPos.y, 0f);
        }
    }

    public void FlattenOnY()
    {
        foreach (BezierSegment anchor in bezierSegmentList) {
            anchor.anchorPos = new Vector3(anchor.anchorPos.x, 0f, anchor.anchorPos.z);
            anchor.controlAPos = new Vector3(anchor.controlAPos.x, 0f, anchor.controlAPos.z);
            anchor.controlBPos = new Vector3(anchor.controlBPos.x, 0f, anchor.controlBPos.z);
        }
    }

    public void SetDirty()
    {
        UpdateVertexPath();

        OnDirty?.Invoke(this, EventArgs.Empty);
    }

    // TODO Move these methods to a utilities class
    /// Returns point at time 't' (between 0 and 1) along quadratic path defined by three points (anchor_1, control, anchor_2)
    public static Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        t = Mathf.Clamp01(t);

        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    /// Returns point at time 't' (between 0 and 1)  along bezier curve defined by 4 points (anchor_1, control_1, control_2, anchor_2)
    public static Vector3 CubicLerp(BezierSegment a1, BezierSegment a2, float t)
    {
        return CubicLerp(a1.anchorPos, a1.controlBPos, a2.controlAPos, a2.anchorPos, t);
    }

    /// Returns point at time 't' (between 0 and 1)  along bezier curve defined by 4 points (anchor_1, control_1, control_2, anchor_2)
    public static Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        t = Mathf.Clamp01(t);

        Vector3 abc = QuadraticLerp(a, b, c, t);
        Vector3 bcd = QuadraticLerp(b, c, d, t);

        return Vector3.Lerp(abc, bcd, t);
    }

    /// Returns a vector tangent to the point at time 't'
    /// This is the vector tangent to the curve at that point
    public static Vector3 EvaluateCurveDerivative(BezierSegment a1, BezierSegment a2, float t)
    {
        return EvaluateCurveDerivative(a1.anchorPos, a1.controlBPos, a2.controlAPos, a2.anchorPos, t);
    }

    /// Calculates the derivative of the curve at time 't'
    /// This is the vector tangent to the curve at that point
    public static Vector3 EvaluateCurveDerivative(Vector3 a1, Vector3 c1, Vector3 c2, Vector3 a2, float t)
    {
        t = Mathf.Clamp01(t);
        return 3 * (1 - t) * (1 - t) * (c1 - a1) + 6 * (1 - t) * t * (c2 - c1) + 3 * t * t * (a2 - c2);
    }

    // Crude, but fast estimation of bezier curve length.
    public static float EstimateBezierCurveLength(BezierSegment anchor1, BezierSegment anchor2)
    {
        float controlNetLength = (anchor1.anchorPos - anchor1.controlBPos).magnitude + (anchor1.controlBPos - anchor2.controlAPos).magnitude + (anchor2.controlAPos - anchor2.anchorPos).magnitude;
        float estimatedCurveLength = (anchor1.anchorPos - anchor2.anchorPos).magnitude + controlNetLength / 2f;
        return estimatedCurveLength;
    }

    public List<BezierSegment> GetBezierSegmentList() { return bezierSegmentList; }

    public VertexPath GetVertexPath() { return vertexPath; }

    public bool IsClosedLoop() { return isClosedLoop; }

    public int GetNumSegments() { return isClosedLoop ? bezierSegmentList.Count : bezierSegmentList.Count - 1; }

    private void OnDrawGizmos()
    {
        GameObject selectedObj = UnityEditor.Selection.activeGameObject;
        if (!isVisableWhenNotSelected || selectedObj == gameObject)
            return;

        // This should be moved into a hook method for when the scene view is opened, however,
        // I couldn't find any such method.
        if (vertexPath == null && bezierSegmentList != null)
            UpdateVertexPath();

        if (vertexPath != null) {
            Gizmos.color = Color.green;
            for(int i = 0; i < vertexPath.vertices.Count; i++) {
                int nextPoint = i + 1;
                if (nextPoint >= vertexPath.vertices.Count) {
                    if (isClosedLoop)
                        nextPoint %= vertexPath.vertices.Count;
                    else
                        break;
                }
                Gizmos.DrawLine(transform.position + vertexPath.vertices[i], transform.position + vertexPath.vertices[nextPoint]);
            }
        }
    }
}

public enum BezierHandleType
{
    None,
    Anchor,
    ControlA,
    ControlB
}

[Serializable]
public class BezierSegment
{
    public Vector3 anchorPos;
    public Vector3 controlAPos;
    public Vector3 controlBPos;

    public Vector3 GetBezierHandlePosition(BezierHandleType bezierHandleType)
    {
        Vector3 handlePosition = Vector3.zero;
        if (bezierHandleType == BezierHandleType.Anchor)
            handlePosition = anchorPos;
        else if(bezierHandleType == BezierHandleType.ControlA)
            handlePosition = controlAPos;
        else if (bezierHandleType == BezierHandleType.ControlB)
            handlePosition = controlBPos;

        return handlePosition;
    }

    public void SetBezierHandlePosition(Vector3 newPosition, BezierHandleType bezierHandleType)
    {
        if (bezierHandleType == BezierHandleType.Anchor)
            anchorPos = newPosition;
        else if (bezierHandleType == BezierHandleType.ControlA)
            controlAPos = newPosition;
        else if (bezierHandleType == BezierHandleType.ControlB)
            controlBPos = newPosition;
    }
}

[Serializable]
public class VertexPath
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector3> tangents = new List<Vector3>();
    public List<float> cumulativeLength = new List<float>();

    public void Clear()
    {
        vertices.Clear();
        tangents.Clear();
        cumulativeLength.Clear();
    }
}