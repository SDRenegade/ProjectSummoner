using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraEncounter : MonoBehaviour
{
    [SerializeField] private List<Terra> terraList;

    public void Start()
    {
        for(int i = 0; i < terraList.Count; i++)
            terraList[i].SetCurrentHP(terraList[i].GetMaxHP());

    }

    public List<Terra> GetTerraList() { return terraList; }

    public void SetTerraList(List<Terra> terraList) { this.terraList = terraList; }
}
