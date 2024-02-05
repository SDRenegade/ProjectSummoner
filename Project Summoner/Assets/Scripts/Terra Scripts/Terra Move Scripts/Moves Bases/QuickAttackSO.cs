using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerraMoveBase", menuName = "TerraMove/Quick Attack")]
public class QuickAttackSO : TerraMoveSO
{
    public override TerraMoveBase CreateTerraMoveAction(TerraAttack terraAttack) { return null; }
}
