using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Vector3 playerRotation;
    [SerializeField] private Vector2 mouseAxisCamRotation;

    [SerializeField] private List<TerraSavable> terraSavableList;

    public PlayerSaveData()
    {
        playerPosition = new Vector3();
        playerRotation = new Vector3();
        mouseAxisCamRotation = new Vector2();

        terraSavableList = new List<TerraSavable>();
    }

    public Vector3 GetPlayerPosition() { return playerPosition; }

    public void SetPlayerPosition(Vector3 playerPosition) { this.playerPosition = playerPosition; }

    public Vector3 GetPlayerRotation() { return playerRotation; }

    public void SetPlayerRotation(Vector3 playerRotation) { this.playerRotation = playerRotation; }

    public Vector2 GetMouseAxisCamRotation() { return mouseAxisCamRotation; }

    public void SetMouseAxisCamRotation(Vector2 mouseAxisCamRotation) { this.mouseAxisCamRotation = mouseAxisCamRotation; }

    public List<Terra> GetTerraList()
    {
        List<Terra> terraList = new List<Terra>();
        for(int i = 0; i < terraSavableList.Count; i++)
            terraList.Add(new Terra(terraSavableList[i]));

        return terraList;
    }

    public void SetTerraSavableList(List<Terra> terraList)
    {
        terraSavableList.Clear();

        for(int i = 0; i < terraList.Count; i++)
            terraSavableList.Add(new TerraSavable(terraList[i]));
    }
}
