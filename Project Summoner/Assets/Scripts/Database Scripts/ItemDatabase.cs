using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<ItemSO> itemList;

    public List<ItemSO> GetItemList() { return itemList; }
}
