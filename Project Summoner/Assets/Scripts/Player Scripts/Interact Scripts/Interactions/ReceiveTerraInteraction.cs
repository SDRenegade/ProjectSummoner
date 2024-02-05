using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveTerraInteraction : MonoBehaviour, Interactable
{
    [SerializeField] private Terra terra;

    public void Interact(GameObject player)
    {
        TerraParty party = player.GetComponentInParent<TerraParty>();
        if(party == null)
            return;

        if (party.AddPartyMember(terra)) {
            Debug.Log("Contragulations you have just received a " + terra.GetTerraBase().GetSpeciesName());
            Debug.Log("Level: " + terra.GetLevel() + " Move set: " + terra.GetMoves()[0]?.GetMoveSO().GetMoveName() + ", " + terra.GetMoves()[1]?.GetMoveSO().GetMoveName());
        }
        else
            Debug.Log("Your party is full");
    }
}
