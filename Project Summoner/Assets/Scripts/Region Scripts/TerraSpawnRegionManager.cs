using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraSpawnRegionManager : MonoBehaviour
{
    [SerializeField] private List<TerraSpawnRegion> terraSpawnRegionList;

    void Start() {}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)) {
            foreach (TerraSpawnRegion region in terraSpawnRegionList) {
                Debug.Log("Invoking random spawn in region: " + region.gameObject.name);
                for(int i = 0; i < 3; i++)
                    region.InvokeRandomSpawn();
            }
        }
    }

    public List<TerraSpawnRegion> GetTerraSpawnRegionList() { return  terraSpawnRegionList; }
}
