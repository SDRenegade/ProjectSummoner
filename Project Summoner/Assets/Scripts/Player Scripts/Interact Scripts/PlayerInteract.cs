using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public interface Interactable
{
    void Interact(GameObject player);
}

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactionDistance;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.LeftAlt))
            return;

        foreach(Collider collider in Physics.OverlapSphere(transform.position, interactionDistance)) {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
                interactable.Interact(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        TerraEncounter terraEncounter = collider.GetComponent<TerraEncounter>();
        if (terraEncounter == null)
            return;

        Debug.Log("You have encountered a(n)" + terraEncounter.GetTerraList()[0].GetTerraBase().GetSpeciesName());

        BattleLoader.GetInstance().LoadWildBattle(gameObject.transform.root.GetComponent<PlayerTerraParty>().GetTerraList(), terraEncounter.GetTerraList());
    }
}
