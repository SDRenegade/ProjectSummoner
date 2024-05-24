using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class PathHandle
{
    public const float extraInputRadius = .005f;
    private static readonly HandleColors ANCHOR_COLORS = new HandleColors(Color.red, new Color32(255, 100, 100, 255), Color.white);
    private static readonly HandleColors CONTROL_COLORS = new HandleColors(Color.blue, new Color32(115, 120, 255, 255), Color.white);

    private static Tuple<int, BezierHandleType> selectedHandle;
    private static bool isMouseOverAHandle;
    private static Vector2 handleDragMouseStart;
    private static Vector2 handleDragMouseEnd;
    private static Vector3 handleDragWorldStart;

    public static Vector3 DrawHandle(Vector3 position, float handleDiameter, out HandleInputType inputType, Tuple<int, BezierHandleType> handleIndexAndType)
    {
        float handleRadius = handleDiameter / 2f;
        float dstToHandle = HandleUtility.DistanceToCircle(position, handleRadius + extraInputRadius);

        // Repaint if mouse is entering/exiting handle (for highlight color)
        if (dstToHandle == 0 && !isMouseOverAHandle) {
            HandleUtility.Repaint();
            isMouseOverAHandle = true;
        }
        else if(isMouseOverAHandle) {
            HandleUtility.Repaint();
            isMouseOverAHandle = false;
        }

        int handleId = GetHandleID(handleIndexAndType);
        inputType = HandleInputType.None;
        switch (Event.current.type) {
            case EventType.MouseDown:
                if (Event.current.button == 0) {
                    if (dstToHandle == 0) {
                        GUIUtility.hotControl = handleId;
                        handleDragMouseEnd = handleDragMouseStart = Event.current.mousePosition;
                        handleDragWorldStart = position;
                        selectedHandle = handleIndexAndType;
                        inputType = HandleInputType.LMBPress;
                    }
                }
                break;

            case EventType.MouseUp:
                if (GUIUtility.hotControl == handleId && Event.current.button == 0) {
                    GUIUtility.hotControl = 0;
                    selectedHandle = null;
                    Event.current.Use();

                    inputType = (Event.current.mousePosition == handleDragMouseStart) ?
                        HandleInputType.LMBClick : HandleInputType.LMBRelease;
                }
                break;

            case EventType.MouseDrag:
                if (GUIUtility.hotControl == handleId && Event.current.button == 0) {
                    handleDragMouseEnd += new Vector2(Event.current.delta.x, -Event.current.delta.y);
                    Vector3 position2 = Camera.current.WorldToScreenPoint(Handles.matrix.MultiplyPoint(handleDragWorldStart))
                        + (Vector3)(handleDragMouseEnd - handleDragMouseStart);
                    inputType = HandleInputType.LMBDrag;
                    position = Handles.matrix.inverse.MultiplyPoint(Camera.current.ScreenToWorldPoint(position2));

                    GUI.changed = true;
                    Event.current.Use();
                }
                break;
        }

        if(Event.current.type == EventType.Repaint) {
            Color originalColour = Handles.color;
            HandleColors handleColors = handleIndexAndType.Item2 == BezierHandleType.Anchor ? ANCHOR_COLORS : CONTROL_COLORS;
            Handles.color = handleColors.defaultColor;

            if (dstToHandle == 0)
                Handles.color = (selectedHandle == null) ? handleColors.highlightedColor : handleColors.selectedColor;
            Handles.SphereHandleCap(0, position, Quaternion.identity, handleDiameter, EventType.Repaint);

            Handles.color = originalColour;
        }

        return position;
    }

    private static int GetHandleID(Tuple<int, BezierHandleType> handleIndexAndType)
    {
        return handleIndexAndType.Item1 * 10 + (int)handleIndexAndType.Item2;
    }

    public enum HandleInputType
    {
        None,
        LMBPress,
        LMBClick,
        LMBDrag,
        LMBRelease,
    };

    public struct HandleColors
    {
        public Color defaultColor;
        public Color highlightedColor;
        public Color selectedColor;

        public HandleColors(Color defaultColor, Color highlightedColor, Color selectedColor)
        {
            this.defaultColor = defaultColor;
            this.highlightedColor = highlightedColor;
            this.selectedColor = selectedColor;
        }
    }
}
