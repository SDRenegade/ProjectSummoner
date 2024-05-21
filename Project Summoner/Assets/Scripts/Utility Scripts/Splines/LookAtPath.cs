using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LookAtPath
{
    [SerializeField] private Spline path;
    [SerializeField] private Transform lookAtPos;

    public Spline GetPath() { return path; }

    public Transform GetLookAtPosition() {  return lookAtPos; }
}
