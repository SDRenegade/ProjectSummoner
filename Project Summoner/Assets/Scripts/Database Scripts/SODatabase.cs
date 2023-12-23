using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Singleton class for storing scriptable object lookup tables
public class SODatabase : MonoBehaviour
{
    private static SODatabase instance;
    [SerializeField] private TerraDatabase terraDatabase;
    [SerializeField] private TerraMoveDatabase terraMoveDatabase;
    [SerializeField] private StatusEffectDatabase statusEffectDatabase;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public TerraBase GetTerraByID(int id)
    {
        if (id >= terraDatabase.GetTerraBases().Count)
            return null;

        return terraDatabase.GetTerraBases()[id];
    }

    public TerraBase GetTerraByName(string speciesName)
    {
        TerraBase terra = null;
        foreach(TerraBase terraBase in terraDatabase.GetTerraBases()) {
            if(terraBase.GetSpeciesName() == speciesName) {
                terra = terraBase;
                break;
            }
        }

        return terra;
    }

    public TerraMoveBase GetTerraMoveByID(int id)
    {
        if (id >= terraMoveDatabase.GetTerraMoves().Count)
            return null;

        return terraMoveDatabase.GetTerraMoves()[id];
    }

    public TerraMoveBase GetTerraMoveByName(string moveName)
    {
        TerraMoveBase terraMove = null;
        foreach (TerraMoveBase terraMoveBase in terraMoveDatabase.GetTerraMoves()) {
            if (terraMoveBase.GetMoveName() == moveName) {
                terraMove = terraMoveBase;
                break;
            }
        }

        return terraMove;
    }

    public StatusEffectBase GetStatusEffectByID(int id)
    {
        if (id >= statusEffectDatabase.GetStatusEffectBases().Count)
            return null;

        return statusEffectDatabase.GetStatusEffectBases()[id];
    }

    public StatusEffectBase GetStatusEffectByName(string statusEffectName)
    {
        StatusEffectBase statusEffect = null;
        foreach (StatusEffectBase statusEffectBase in statusEffectDatabase.GetStatusEffectBases()) {
            if (statusEffectBase.GetStatusName() == statusEffectName) {
                statusEffect = statusEffectBase;
                break;
            }
        }

        return statusEffect;
    }

    public static SODatabase GetInstance() { return instance; }
}
