using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraSwitch
{
    private int leadingPositionIndex;
    private int benchPositionIndex;
    private bool isPrimarySide;

    public TerraSwitch(int leadingPositionIndex, int benchPositionIndex, bool isPrimarySide)
    {
        this.leadingPositionIndex = leadingPositionIndex;
        this.benchPositionIndex = benchPositionIndex;
        this.isPrimarySide = isPrimarySide;
    }

    public int GetLeadingPositionIndex() { return leadingPositionIndex; }

    public int GetBenchPositionIndex() {  return benchPositionIndex; }

    public bool IsPrimarySide() {  return isPrimarySide; }
}
