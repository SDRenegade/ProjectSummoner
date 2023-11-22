using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerraSpawnRegion : MonoBehaviour
{
    private BoxCollider region;
    [SerializeField] private GameObject wildTerraParentObject;
    [SerializeField] private List<TerraSpawnEntry> terraSpawnList;
    [SerializeField] private bool isSpawning;
    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        region = GetComponent<BoxCollider>();
        if (region == null)
            Debug.Log("No box collider found for terra spawn region on game object " + gameObject.name);
    }

    public bool InvokeRandomSpawn()
    {
        if(terraSpawnList.Count == 0)
            return false;

        int totalWeight = 0;
        for(int i = 0; i < terraSpawnList.Count; ++i)
            totalWeight += terraSpawnList[i].GetWeight();

        int randWeightValue = Random.Range(0, totalWeight);
        for (int i = 0; i < terraSpawnList.Count; ++i) {
            if(randWeightValue < terraSpawnList[i].GetWeight()) {
                Vector3 rayOrigin = new Vector3(
                    Random.Range(region.bounds.min.x, region.bounds.max.x),
                    region.bounds.max.y,
                    Random.Range(region.bounds.min.z, region.bounds.max.z));
                Ray ray = new Ray(rayOrigin, new Vector3(0, -1, 0));
                if(Physics.Raycast(ray, out RaycastHit hit, region.bounds.max.y - region.bounds.min.y, layerMask)) {
                    Vector3 spawnPosition = new Vector3(rayOrigin.x, hit.transform.position.y, rayOrigin.z);
                    GameObject spawnedTerraGameObject = Instantiate(terraSpawnList[i].GetTerraBase().GetTerraGameObject(), spawnPosition, Quaternion.identity);
                    spawnedTerraGameObject.transform.parent = wildTerraParentObject.transform;
                    spawnedTerraGameObject.AddComponent<TerraEncounter>().SetTerra(new Terra(terraSpawnList[i].GetTerraBase(), Random.Range(terraSpawnList[i].GetMinLevel(), terraSpawnList[i].GetMaxLevel())));
                }
                else
                    Debug.Log("No spawnable area detected for layerMask " + layerMask.value);
                break;
            }
            randWeightValue -= terraSpawnList[i].GetWeight();
        }

        return true;
    }

    public BoxCollider GetRegion() { return region; }

    public void SetRegion(BoxCollider region) { this.region = region; }

    public List<TerraSpawnEntry> GetTerraSpawnMap() { return terraSpawnList; }

    public void SetTerraSpawnMap(List<TerraSpawnEntry> terraSpawnList) { this.terraSpawnList = terraSpawnList; }

    public bool GetIsSpawning() { return isSpawning; }

    public void SetIsSpawning(bool isSpawning) { this.isSpawning = isSpawning; }
}
