using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PathHandle;

[CustomEditor(typeof(Spline))]
public class SplineEditor : Editor
{
    private readonly float BEZIER_LINE_WIDTH = 3f;
    private readonly Color BEZIER_LINE_COLOR = Color.green;
    private readonly float ANCHOR_SIZE = 0.70f;
    private readonly float CONTROL_SIZE = 0.45f;
    private readonly int WORLD_RAY_DISTANCE = 18;

    private Spline spline;
    private Tool LastTool;
    private Tuple<int, SplineHandleType> mouseOverHandle;
    private Tuple<int, SplineHandleType> transformDisplayHandle;

    private void OnEnable()
    {
        spline = (Spline)target;

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
                Undo.RecordObject(spline, "Add Anchor");
                spline.AddAnchor();
                spline.SetDirty();
                serializedObject.Update();
            }

            if (GUILayout.Button("Remove Last Anchor")) {
                Undo.RecordObject(spline, "Remove Last Anchor");
                spline.RemoveLastAnchor();
                spline.SetDirty();
                serializedObject.Update();
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("normal"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isClosedLoop"));

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Flatten on Z axis")) {
                Undo.RecordObject(spline, "Flatten on Z axis");
                spline.FlattenOnZ();
                spline.SetDirty();
                serializedObject.Update();
            }
            if (GUILayout.Button("Flatten on Y axis")) {
                Undo.RecordObject(spline, "Flatten on Y axis");
                spline.FlattenOnY();
                spline.SetDirty();
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
        Vector3 transformPosition = spline.transform.position;
        if (spline.GetAnchorList() == null || spline.GetAnchorList().Count == 0)
            spline.InitializeAnchorList();

        List<SplineAnchor> anchorList = spline.GetAnchorList();
        if (anchorList != null) {
            for (int i = 0; i < spline.GetAnchorList().Count; i++) {
                SplineAnchor anchor = spline.GetAnchorList()[i];

                DrawHandle(new Tuple<int, SplineHandleType>(i, SplineHandleType.Anchor));
                if(i != 0 || spline.IsClosedLoop()) {
                    DrawHandle(new Tuple<int, SplineHandleType>(i, SplineHandleType.ControlA));
                    Handles.color = Color.black;
                    Handles.DrawLine(transformPosition + anchor.anchorPos, transformPosition + anchor.controlAPos);
                }
                if(i != spline.GetAnchorList().Count -1 || spline.IsClosedLoop()) {
                    DrawHandle(new Tuple<int, SplineHandleType>(i, SplineHandleType.ControlB));
                    Handles.color = Color.black;
                    Handles.DrawLine(transformPosition + anchor.anchorPos, transformPosition + anchor.controlBPos);
                }
            }

            // Draw Bezier
            for (int i = 0; i < spline.GetAnchorList().Count - 1; i++) {
                SplineAnchor anchor = spline.GetAnchorList()[i];
                SplineAnchor nextAnchor = spline.GetAnchorList()[i + 1];
                Handles.DrawBezier(transformPosition + anchor.anchorPos, transformPosition + nextAnchor.anchorPos, transformPosition + anchor.controlBPos, transformPosition + nextAnchor.controlAPos, BEZIER_LINE_COLOR, null, BEZIER_LINE_WIDTH);
            }

            if (spline.IsClosedLoop()) {
                // Spline is Closed Loop
                SplineAnchor anchor = spline.GetAnchorList()[spline.GetAnchorList().Count - 1];
                SplineAnchor nextAnchor = spline.GetAnchorList()[0];
                Handles.DrawBezier(transformPosition + anchor.anchorPos, transformPosition + nextAnchor.anchorPos, transformPosition + anchor.controlBPos, transformPosition + nextAnchor.controlAPos, BEZIER_LINE_COLOR, null, BEZIER_LINE_WIDTH);
            }
        }
    }

    private void ProcessBezierCurveInput(Event e)
    {
        int previousMouseOverHandleIndex = (mouseOverHandle == null) ? 0 : mouseOverHandle.Item1;
        mouseOverHandle = null;
        for(int i = 0; i < spline.GetAnchorList().Count; i++) {
            int handleIndex = (previousMouseOverHandleIndex + i) % spline.GetAnchorList().Count;
            Vector3 anchorPos = spline.transform.position + spline.GetAnchorList()[handleIndex].anchorPos;
            Vector3 controlAPos = spline.transform.position + spline.GetAnchorList()[handleIndex].controlAPos;
            Vector3 controlBPos = spline.transform.position + spline.GetAnchorList()[handleIndex].controlBPos;

            float distanceToAnchor = HandleUtility.DistanceToCircle(anchorPos, ANCHOR_SIZE);
            if (distanceToAnchor == 0) {
                mouseOverHandle = new Tuple<int, SplineHandleType>(handleIndex, SplineHandleType.Anchor);
                break;
            }
            float distanceControlA = HandleUtility.DistanceToCircle(controlAPos, CONTROL_SIZE);
            if (distanceControlA == 0) {
                mouseOverHandle = new Tuple<int, SplineHandleType>(handleIndex, SplineHandleType.ControlA);
                break;
            }
            float distanceControlB = HandleUtility.DistanceToCircle(controlBPos, CONTROL_SIZE);
            if (distanceControlB == 0) {
                mouseOverHandle = new Tuple<int, SplineHandleType>(handleIndex, SplineHandleType.ControlB);
                break;
            }
        }

        if (mouseOverHandle == null) {
            // Shift-left click (when mouse not over a handle) to add new segment
            if (e.type == EventType.MouseDown && e.button == 0 && e.shift) {
                Vector2 mousePos = Event.current.mousePosition;

                Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePos);
                Vector3 newAnchorPos = worldRay.origin + (worldRay.direction * WORLD_RAY_DISTANCE);

                Undo.RecordObject(spline, "Added Anchor");
                spline.AddAnchor(newAnchorPos);
            }
        }
        else {
            // Control left click or press delete over an anchor to remove it from the spline
            if (e.keyCode == KeyCode.Backspace || (e.control && e.type == EventType.MouseDown && e.button == 0)) {
                if(mouseOverHandle.Item2 == SplineHandleType.Anchor) {
                    Undo.RecordObject(spline, "Removed Anchor");
                    spline.RemoveAnchorAt(mouseOverHandle.Item1);
                }
            }
        }
    }

    // indexAndType holds the anchor index as item1 value and the point type (Anchor, ControlA, or ControlB) as item2
    private void DrawHandle(Tuple<int, SplineHandleType> handleIndexAndType)
    {
        if (handleIndexAndType.Item1 >= spline.GetAnchorList().Count)
            return;
        if (handleIndexAndType.Item2 == SplineHandleType.None)
            return;

        Vector3 handlePosition = Vector3.zero;
        switch(handleIndexAndType.Item2) {
            case SplineHandleType.Anchor:
                handlePosition = spline.transform.position + spline.GetAnchorList()[handleIndexAndType.Item1].anchorPos;
                break;
            case SplineHandleType.ControlA:
                handlePosition = spline.transform.position + spline.GetAnchorList()[handleIndexAndType.Item1].controlAPos;
                break;
            case SplineHandleType.ControlB:
                handlePosition = spline.transform.position + spline.GetAnchorList()[handleIndexAndType.Item1].controlBPos;
                break;
        }

        float handleSize = (handleIndexAndType.Item2 == SplineHandleType.Anchor) ? ANCHOR_SIZE : CONTROL_SIZE;
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


        Vector3 localPosition = handlePosition - spline.transform.position;
        // Update spline anchor/control position. If an anchor position is updated, the corresponding control positions are updated as well.
        // If shift is held and a control point is being updated, the control point positions are mirrored.
        if (spline.GetAnchorList()[handleIndexAndType.Item1].GetSplineHandlePosition(handleIndexAndType.Item2) != localPosition) {
            Undo.RecordObject(spline, "Move point");
            SplineAnchor anchor = spline.GetAnchorList()[handleIndexAndType.Item1];
            if (handleIndexAndType.Item2 == SplineHandleType.Anchor) {
                anchor.controlAPos += localPosition - anchor.anchorPos;
                anchor.controlBPos += localPosition - anchor.anchorPos;
                anchor.anchorPos = localPosition;
            }
            else if(handleIndexAndType.Item2 == SplineHandleType.ControlA) {
                anchor.controlAPos = localPosition;
                if (Event.current.shift)
                    anchor.controlBPos = anchor.anchorPos - (anchor.controlAPos - anchor.anchorPos);
            }
            else {
                anchor.controlBPos = localPosition;
                if (Event.current.shift)
                    anchor.controlAPos = anchor.anchorPos - (anchor.controlBPos - anchor.anchorPos);
            }

            spline.UpdateVertexPath();
        }
    }
}
