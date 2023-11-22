using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTerraParty : TerraParty, IPersistentData
{
    public void LoadData(GamePersistentData sceneSaveData)
    {
        if (sceneSaveData == null)
            return;

        terraList = sceneSaveData.GetPlayerSaveData().GetTerraList();
    }

    public void SaveData(ref GamePersistentData sceneSaveData)
    {
        sceneSaveData.GetPlayerSaveData().SetTerraSavableList(terraList);
    }

}
