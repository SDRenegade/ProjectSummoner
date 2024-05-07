using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spline))]
public class SplineEditor : Editor
{
    private readonly float BEZIER_LINE_WIDTH = 3f;
    private readonly Color BEZIER_LINE_COLOR = Color.green;
    private readonly float ANCHOR_SIZE = 0.75f;
    private readonly float HANDLE_SIZE = 0.4f;
    // TODO Add anchor colors for normal, highlighted, and selected
    private readonly int WORLD_RAY_DISTANCE = 18;

    private Spline spline;
    private Tool LastTool = Tool.None;
    private int mouseOverHandleIndex;
    private bool wasShiftingLastFrame;

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

            if (GUILayout.Button("Set All Z = 0")) {
                Undo.RecordObject(spline, "Set All Z = 0");
                spline.SetAllZZero();
                spline.SetDirty();
                serializedObject.Update();
            }
            if (GUILayout.Button("Set All Y = 0")) {
                Undo.RecordObject(spline, "Set All Y = 0");
                spline.SetAllYZero();
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
        List<SplineAnchor> anchorList = spline.GetAnchorList();
        if (anchorList != null) {
            foreach (SplineAnchor anchor in spline.GetAnchorList()) {
                Handles.color = Color.red;
                Handles.SphereHandleCap(0, spline.transform.position + anchor.position, Quaternion.identity, ANCHOR_SIZE, EventType.Repaint);

                EditorGUI.BeginChangeCheck();
                Vector3 newAnchorPos = Handles.PositionHandle(spline.transform.position + anchor.position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(spline, "Change Anchor Position");
                    anchor.controlAPosition = (anchor.controlAPosition - anchor.position) + newAnchorPos - transformPosition;
                    anchor.controlBPosition = (anchor.controlBPosition - anchor.position) + newAnchorPos - transformPosition;
                    anchor.position = newAnchorPos - transformPosition;
                    spline.SetDirty();
                    serializedObject.Update();
                }

                Handles.color = Color.blue;
                Handles.SphereHandleCap(0, transformPosition + anchor.controlAPosition, Quaternion.identity, HANDLE_SIZE, EventType.Repaint);

                EditorGUI.BeginChangeCheck();
                Vector3 newHandleAPos = Handles.PositionHandle(transformPosition + anchor.controlAPosition, Quaternion.identity);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(spline, "Change Anchor Control A Position");
                    anchor.controlAPosition = newHandleAPos - transformPosition;
                    if (Event.current.shift)
                        anchor.controlBPosition = anchor.position - (anchor.controlAPosition - anchor.position);
                    spline.SetDirty();
                    serializedObject.Update();
                }

                Handles.color = Color.blue;
                Handles.SphereHandleCap(0, transformPosition + anchor.controlBPosition, Quaternion.identity, HANDLE_SIZE, EventType.Repaint);

                EditorGUI.BeginChangeCheck();
                Vector3 newHandleBPos = Handles.PositionHandle(transformPosition + anchor.controlBPosition, Quaternion.identity);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(spline, "Change Anchor Control B Position");
                    anchor.controlBPosition = newHandleBPos - transformPosition;
                    if (Event.current.shift)
                        anchor.controlAPosition = anchor.position - (anchor.controlBPosition - anchor.position);
                    spline.SetDirty();
                    serializedObject.Update();
                }

                Handles.color = Color.black;
                Handles.DrawLine(transformPosition + anchor.position, transformPosition + anchor.controlAPosition);
                Handles.DrawLine(transformPosition + anchor.position, transformPosition + anchor.controlBPosition);
            }

            // Draw Bezier
            for (int i = 0; i < spline.GetAnchorList().Count - 1; i++) {
                SplineAnchor anchor = spline.GetAnchorList()[i];
                SplineAnchor nextAnchor = spline.GetAnchorList()[i + 1];
                Handles.DrawBezier(transformPosition + anchor.position, transformPosition + nextAnchor.position, transformPosition + anchor.controlBPosition, transformPosition + nextAnchor.controlAPosition, BEZIER_LINE_COLOR, null, BEZIER_LINE_WIDTH);
            }

            if (spline.IsClosedLoop()) {
                // Spline is Closed Loop
                SplineAnchor anchor = spline.GetAnchorList()[spline.GetAnchorList().Count - 1];
                SplineAnchor nextAnchor = spline.GetAnchorList()[0];
                Handles.DrawBezier(transformPosition + anchor.position, transformPosition + nextAnchor.position, transformPosition + anchor.controlBPosition, transformPosition + nextAnchor.controlAPosition, BEZIER_LINE_COLOR, null, BEZIER_LINE_WIDTH);
            }
        }
    }

    private void ProcessBezierCurveInput(Event e)
    {
        int previousMouseOverHandleIndex = (mouseOverHandleIndex == -1) ? 0 : mouseOverHandleIndex;
        mouseOverHandleIndex = -1;
        for(int i = 0; i < spline.GetAnchorList().Count; i++) {
            int handleIndex = (previousMouseOverHandleIndex + i) % spline.GetAnchorList().Count;
            float handleRadius = HANDLE_SIZE;
            Vector3 pos = spline.transform.position + spline.GetAnchorList()[handleIndex].position;
            float distance = HandleUtility.DistanceToCircle(pos, handleRadius);
            if (distance == 0) {
                mouseOverHandleIndex = handleIndex;
                break;
            }
        }

        // Shift-left click (when mouse not over a handle) to split or add segment
        if (mouseOverHandleIndex == -1) {
            if (e.type == EventType.MouseDown && e.button == 0 && e.shift) {
                Vector2 mousePos = Event.current.mousePosition;

                Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePos);
                Vector3 newAnchorPos = worldRay.origin + (worldRay.direction * WORLD_RAY_DISTANCE);

                Undo.RecordObject(spline, "Added Anchor");
                spline.AddAnchor(newAnchorPos);



            }
        }
    }
}
