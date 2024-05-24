using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PathHandle;

[CustomEditor(typeof(PathCreator))]
public class SplineEditor : Editor
{
    private readonly float BEZIER_LINE_WIDTH = 3f;
    private readonly Color BEZIER_LINE_COLOR = Color.green;
    private readonly float ANCHOR_SIZE = 0.70f;
    private readonly float CONTROL_SIZE = 0.45f;
    private readonly int WORLD_RAY_DISTANCE = 18;

    private PathCreator pathCreator;
    private Tool LastTool;
    private Tuple<int, BezierHandleType> mouseOverHandle;
    private Tuple<int, BezierHandleType> transformDisplayHandle;

    private void OnEnable()
    {
        pathCreator = (PathCreator)target;

        LastTool = Tools.current;
        Tools.current = Tool.None;
    }

    private void OnDisable()
    {
        Tools.current = LastTool;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        using (var check = new EditorGUI.ChangeCheckScope()) {
            if (GUILayout.Button("Add Anchor")) {
                Undo.RecordObject(pathCreator, "Add Anchor");
                pathCreator.AddAnchor();
                pathCreator.SetDirty();
                serializedObject.Update();
            }

            if (GUILayout.Button("Remove Last Anchor")) {
                Undo.RecordObject(pathCreator, "Remove Last Anchor");
                pathCreator.RemoveLastAnchor();
                pathCreator.SetDirty();
                serializedObject.Update();
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("normal"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isClosedLoop"));

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Flatten on Z axis")) {
                Undo.RecordObject(pathCreator, "Flatten on Z axis");
                pathCreator.FlattenOnZ();
                pathCreator.SetDirty();
                serializedObject.Update();
            }
            if (GUILayout.Button("Flatten on Y axis")) {
                Undo.RecordObject(pathCreator, "Flatten on Y axis");
                pathCreator.FlattenOnY();
                pathCreator.SetDirty();
                serializedObject.Update();
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("isVisableWhenNotSelected"));

            serializedObject.ApplyModifiedProperties();

            // If a change in the scene was detect, refresh the scene view
            if (check.changed) {
                SceneView.RepaintAll();
                EditorApplication.QueuePlayerLoopUpdate();
            }
        }
    }

    private void OnSceneGUI()
    {   
        EventType eventType = Event.current.type;

        using (var check = new EditorGUI.ChangeCheckScope()) {
            DrawBezierCurveSceneEditor();
            ProcessBezierCurveInput(Event.current);

            // Don't allow clicking over empty space to deselect the object
            if (eventType == EventType.Layout)
                HandleUtility.AddDefaultControl(0);

            // If a change in the scene was detect, refresh the scene view
            if (check.changed)
                EditorApplication.QueuePlayerLoopUpdate();
        }

    }

    private void DrawBezierCurveSceneEditor()
    {
        Vector3 transformPosition = pathCreator.transform.position;
        if (pathCreator.GetBezierSegmentList() == null || pathCreator.GetBezierSegmentList().Count == 0)
            pathCreator.InitializeAnchorList();

        List<BezierSegment> bezierSegmentList = pathCreator.GetBezierSegmentList();
        if (bezierSegmentList != null) {
            for (int i = 0; i < pathCreator.GetBezierSegmentList().Count; i++) {
                BezierSegment bezierSegment = pathCreator.GetBezierSegmentList()[i];

                DrawHandle(new Tuple<int, BezierHandleType>(i, BezierHandleType.Anchor));
                if(i != 0 || pathCreator.IsClosedLoop()) {
                    DrawHandle(new Tuple<int, BezierHandleType>(i, BezierHandleType.ControlA));
                    Handles.color = Color.black;
                    Handles.DrawLine(transformPosition + bezierSegment.anchorPos, transformPosition + bezierSegment.controlAPos);
                }
                if(i != pathCreator.GetBezierSegmentList().Count -1 || pathCreator.IsClosedLoop()) {
                    DrawHandle(new Tuple<int, BezierHandleType>(i, BezierHandleType.ControlB));
                    Handles.color = Color.black;
                    Handles.DrawLine(transformPosition + bezierSegment.anchorPos, transformPosition + bezierSegment.controlBPos);
                }
            }

            // Draw Bezier
            for (int i = 0; i < pathCreator.GetBezierSegmentList().Count - 1; i++) {
                BezierSegment segment = pathCreator.GetBezierSegmentList()[i];
                BezierSegment nextSegment = pathCreator.GetBezierSegmentList()[i + 1];
                Handles.DrawBezier(transformPosition + segment.anchorPos, transformPosition + nextSegment.anchorPos, transformPosition + segment.controlBPos, transformPosition + nextSegment.controlAPos, BEZIER_LINE_COLOR, null, BEZIER_LINE_WIDTH);
            }

            if (pathCreator.IsClosedLoop()) {
                BezierSegment segment = pathCreator.GetBezierSegmentList()[pathCreator.GetBezierSegmentList().Count - 1];
                BezierSegment nextSegment = pathCreator.GetBezierSegmentList()[0];
                Handles.DrawBezier(transformPosition + segment.anchorPos, transformPosition + nextSegment.anchorPos, transformPosition + segment.controlBPos, transformPosition + nextSegment.controlAPos, BEZIER_LINE_COLOR, null, BEZIER_LINE_WIDTH);
            }
        }
    }

    private void ProcessBezierCurveInput(Event e)
    {
        int previousMouseOverHandleIndex = (mouseOverHandle == null) ? 0 : mouseOverHandle.Item1;
        mouseOverHandle = null;
        for(int i = 0; i < pathCreator.GetBezierSegmentList().Count; i++) {
            int handleIndex = (previousMouseOverHandleIndex + i) % pathCreator.GetBezierSegmentList().Count;
            Vector3 anchorPos = pathCreator.transform.position + pathCreator.GetBezierSegmentList()[handleIndex].anchorPos;
            Vector3 controlAPos = pathCreator.transform.position + pathCreator.GetBezierSegmentList()[handleIndex].controlAPos;
            Vector3 controlBPos = pathCreator.transform.position + pathCreator.GetBezierSegmentList()[handleIndex].controlBPos;

            float distanceToAnchor = HandleUtility.DistanceToCircle(anchorPos, ANCHOR_SIZE);
            if (distanceToAnchor == 0) {
                mouseOverHandle = new Tuple<int, BezierHandleType>(handleIndex, BezierHandleType.Anchor);
                break;
            }
            float distanceControlA = HandleUtility.DistanceToCircle(controlAPos, CONTROL_SIZE);
            if (distanceControlA == 0) {
                mouseOverHandle = new Tuple<int, BezierHandleType>(handleIndex, BezierHandleType.ControlA);
                break;
            }
            float distanceControlB = HandleUtility.DistanceToCircle(controlBPos, CONTROL_SIZE);
            if (distanceControlB == 0) {
                mouseOverHandle = new Tuple<int, BezierHandleType>(handleIndex, BezierHandleType.ControlB);
                break;
            }
        }

        if (mouseOverHandle == null) {
            // Shift-left click (when mouse not over a handle) to add new segment
            if (e.type == EventType.MouseDown && e.button == 0 && e.shift) {
                Vector2 mousePos = Event.current.mousePosition;

                Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePos);
                Vector3 newAnchorPos = worldRay.origin + (worldRay.direction * WORLD_RAY_DISTANCE);

                Undo.RecordObject(pathCreator, "Added Anchor");
                pathCreator.AddAnchor(newAnchorPos);
            }
        }
        else {
            // Control left click or press delete over an anchor to remove it from the spline
            if (e.keyCode == KeyCode.Backspace || (e.control && e.type == EventType.MouseDown && e.button == 0)) {
                if(mouseOverHandle.Item2 == BezierHandleType.Anchor) {
                    Undo.RecordObject(pathCreator, "Removed Anchor");
                    pathCreator.RemoveAnchorAt(mouseOverHandle.Item1);
                }
            }
        }
    }

    // indexAndType holds the anchor index as item1 value and the point type (Anchor, ControlA, or ControlB) as item2
    private void DrawHandle(Tuple<int, BezierHandleType> handleIndexAndType)
    {
        if (handleIndexAndType.Item1 >= pathCreator.GetBezierSegmentList().Count)
            return;
        if (handleIndexAndType.Item2 == BezierHandleType.None)
            return;

        Vector3 handlePosition = Vector3.zero;
        switch(handleIndexAndType.Item2) {
            case BezierHandleType.Anchor:
                handlePosition = pathCreator.transform.position + pathCreator.GetBezierSegmentList()[handleIndexAndType.Item1].anchorPos;
                break;
            case BezierHandleType.ControlA:
                handlePosition = pathCreator.transform.position + pathCreator.GetBezierSegmentList()[handleIndexAndType.Item1].controlAPos;
                break;
            case BezierHandleType.ControlB:
                handlePosition = pathCreator.transform.position + pathCreator.GetBezierSegmentList()[handleIndexAndType.Item1].controlBPos;
                break;
        }

        float handleSize = (handleIndexAndType.Item2 == BezierHandleType.Anchor) ? ANCHOR_SIZE : CONTROL_SIZE;
        HandleInputType handleInputType;
        handlePosition = PathHandle.DrawHandle(handlePosition, handleSize, out handleInputType, handleIndexAndType);

        bool isTransformHandleVisible = false;
        if (transformDisplayHandle != null)
            isTransformHandleVisible = transformDisplayHandle.Item1 == handleIndexAndType.Item1 && transformDisplayHandle.Item2 == handleIndexAndType.Item2;
        if(isTransformHandleVisible)
            handlePosition = Handles.DoPositionHandle(handlePosition, Quaternion.identity);

        switch (handleInputType) {
            case HandleInputType.LMBDrag:
                transformDisplayHandle = null;
                Repaint();
                break;
            case HandleInputType.LMBRelease:
                transformDisplayHandle = null;
                Repaint();
                break;
            case HandleInputType.LMBClick:
                // Disable move tool if a new point is added with shift click
                if (Event.current.shift)
                    transformDisplayHandle = null;
                else {
                    // disable move tool if clicking on a point that currently has the move tool displayed
                    transformDisplayHandle = isTransformHandleVisible ? null : handleIndexAndType;
                }
                Repaint();
                break;
            case HandleInputType.LMBPress:
                // If the handle pressed down on is not the one that has the trasform tool displayed for,
                // disable the transform tool
                if(transformDisplayHandle != null) {
                    if (transformDisplayHandle.Item1 != handleIndexAndType.Item1 ||
                        transformDisplayHandle.Item2 != handleIndexAndType.Item2) {
                        transformDisplayHandle = null;
                        Repaint();
                    }
                }
                break;
        }

        Vector3 localPosition = handlePosition - pathCreator.transform.position;
        // Update bezier segment anchor/control position. If an anchor position is updated, the corresponding control positions
        // are updated as well. If shift is held and a control point is being updated, the control point positions are mirrored.
        if (pathCreator.GetBezierSegmentList()[handleIndexAndType.Item1].GetBezierHandlePosition(handleIndexAndType.Item2) != localPosition) {
            Undo.RecordObject(pathCreator, "Move point");
            BezierSegment segment = pathCreator.GetBezierSegmentList()[handleIndexAndType.Item1];
            if (handleIndexAndType.Item2 == BezierHandleType.Anchor) {
                segment.controlAPos += localPosition - segment.anchorPos;
                segment.controlBPos += localPosition - segment.anchorPos;
                segment.anchorPos = localPosition;
            }
            else if(handleIndexAndType.Item2 == BezierHandleType.ControlA) {
                segment.controlAPos = localPosition;
                if (Event.current.shift)
                    segment.controlBPos = segment.anchorPos - (segment.controlAPos - segment.anchorPos);
            }
            else {
                segment.controlBPos = localPosition;
                if (Event.current.shift)
                    segment.controlAPos = segment.anchorPos - (segment.controlBPos - segment.anchorPos);
            }

            pathCreator.UpdateVertexPath();
        }
    }
}
