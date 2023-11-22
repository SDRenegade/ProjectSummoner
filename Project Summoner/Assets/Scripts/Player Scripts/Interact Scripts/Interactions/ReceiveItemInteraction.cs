using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveItemInteraction : MonoBehaviour, Interactable
{
    [SerializeField] private string itemName;

    public void Interact(GameObject player)
    {
        Debug.Log("You have received the Item: " + itemName);
    }
}
