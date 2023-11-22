using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class TerraParty : MonoBehaviour
{
    public static readonly int MAX_PARTY_SIZE = 6;

    [SerializeField] protected List<Terra> terraList;

    private void Start()
    {
        //Null check might not be necessary for a serialized field
        if (terraList != null) {
            foreach (Terra terra in terraList)
                terra.SetCurrentHP(terra.GetMaxHP());
        }
    }

    public bool AddPartyMember(Terra terra)
    {
        if (terraList.Count < MAX_PARTY_SIZE) {
            terraList.Add(terra);
            return true;
        }

        return false;
    }

    public Terra RemovePartyMember(int index)
    {
        if(index >= terraList.Count)
            return null;

        Terra removedTerra = terraList[index];
        terraList.RemoveAt(index);

        return removedTerra;
    }

    public void CopyTerraParty(TerraParty newParty)
    {
        terraList.Clear();

        for(int i = 0; i < newParty.GetTerraList().Count; i++)
            terraList.Add(newParty.GetTerraList()[i]);
    }

    public List<Terra> GetTerraList() { return terraList; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < terraList.Count; i++) {
            if (terraList[i] == null)
                break;

            if(i != 0)
                sb.Append(", ");
            sb.Append(terraList[i].GetTerraBase().GetSpeciesName());
        }

        return sb.ToString();
    }
}
