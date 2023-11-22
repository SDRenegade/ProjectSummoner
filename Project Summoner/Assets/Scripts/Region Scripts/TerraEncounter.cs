using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraEncounter : MonoBehaviour
{
    [SerializeField] private Terra terra;

    public void Start()
    {
        terra.SetCurrentHP(terra.GetMaxHP());
    }

    //TODO Add encounter cry and visual effect
    //public void InvokeEncounter() {}

    public Terra GetTerra() { return terra; }

    public void SetTerra(Terra terra) { this.terra = terra; }
}
