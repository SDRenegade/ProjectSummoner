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

        StringBuilder sb = new StringBuilder();
        sb.Append("Encountered " + terraEncounter.GetTerra().GetTerraBase().GetSpeciesName() + " at level: " + terraEncounter.GetTerra().GetLevel() + " / Move set: ");
        for (int i = 0; i < terraEncounter.GetTerra().GetMoves().Count; i++) {
            if (terraEncounter.GetTerra().GetMoves()[i] != null) {
                if (i != 0)
                    sb.Append(", ");
                sb.Append(terraEncounter.GetTerra().GetMoves()[i].GetMoveSO().GetMoveName());
            }
        }
        Debug.Log(sb);

        BattleLoader.GetInstance().LoadWildBattle(gameObject.transform.root.GetComponent<PlayerTerraParty>().GetTerraList(), terraEncounter.GetTerra());
    }
}
