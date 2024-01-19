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
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private StatusEffectDatabase statusEffectDatabase;
    [SerializeField] private VolatileStatusEffectDatabase volatileStatusEffectDatabase;
    [SerializeField] private MetronomeMovesDatabase metronomeMovesDatabase;

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
        if (id >= terraMoveDatabase.GetTerraMoveList().Count)
            return null;

        return terraMoveDatabase.GetTerraMoveList()[id];
    }

    public TerraMoveBase GetTerraMoveByName(string name)
    {
        TerraMoveBase terraMove = null;
        foreach (TerraMoveBase terraMoveBase in terraMoveDatabase.GetTerraMoveList()) {
            if (terraMoveBase.GetMoveName() == name) {
                terraMove = terraMoveBase;
                break;
            }
        }

        return terraMove;
    }

    public ItemBase GetItemByID(int id)
    {
        if (id >= itemDatabase.GetItemList().Count)
            return null;

        return itemDatabase.GetItemList()[id];
    }

    public ItemBase GetItemByName(string name)
    {
        ItemBase item = null;
        foreach (ItemBase itemBase in itemDatabase.GetItemList()) {
            if (itemBase.GetItemName() == name) {
                item = itemBase;
                break;
            }
        }

        return item;
    }

    public StatusEffectBase GetStatusEffectByID(int id)
    {
        if (id >= statusEffectDatabase.GetStatusEffectList().Count)
            return null;

        return statusEffectDatabase.GetStatusEffectList()[id];
    }

    public StatusEffectBase GetStatusEffectByName(string name)
    {
        StatusEffectBase effect = null;
        foreach (StatusEffectBase statusEffect in statusEffectDatabase.GetStatusEffectList()) {
            if (statusEffect.GetStatusName() == name) {
                effect = statusEffect;
                break;
            }
        }

        return effect;
    }

    public VolatileStatusEffectBase GetVolatileStatusEffectByID(int id)
    {
        if (id >= statusEffectDatabase.GetStatusEffectList().Count)
            return null;

        return volatileStatusEffectDatabase.GetVolatileStatusEffectList()[id];
    }

    public VolatileStatusEffectBase GetVolatileStatusEffectByName(string name)
    {
        VolatileStatusEffectBase effect = null;
        foreach (VolatileStatusEffectBase vStatusEffect in volatileStatusEffectDatabase.GetVolatileStatusEffectList()) {
            if (vStatusEffect.GetStatusName() == name) {
                effect = vStatusEffect;
                break;
            }
        }

        return effect;
    }

    public TerraMoveBase GetRandomMetronomeMove()
    {
        if (metronomeMovesDatabase.GetMetronomeMoveList().Count == 0)
            return null;

        return metronomeMovesDatabase.GetMetronomeMoveList()[Random.Range(0, metronomeMovesDatabase.GetMetronomeMoveList().Count - 1)];
    }

    public static SODatabase GetInstance() { return instance; }
}