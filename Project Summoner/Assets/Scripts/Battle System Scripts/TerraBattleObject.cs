using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraBattleObject
{
    private Terra terra;
    private GameObject terraGO;

    public TerraBattleObject(Terra terra)
    {
        this.terra = terra;
    }

    public Terra GetTerra() { return terra; }

    public void SetTerra(Terra terra) { this.terra = terra; }

    public GameObject GetTerraGO() { return terraGO; }

    public void SetTerraGO(GameObject terraGO) { this.terraGO = terraGO; }
}
