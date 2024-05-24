using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LookAtPath
{
    [SerializeField] private PathCreator path;
    [SerializeField] private Transform lookAtPos;

    public PathCreator GetPath() { return path; }

    public Transform GetLookAtPosition() {  return lookAtPos; }
}
