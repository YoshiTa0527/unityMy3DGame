using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
[CreateAssetMenu(fileName ="ItemDataBase", menuName = "CreateItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField] private List<Item> m_itemLists = new List<Item>();

    public List<Item> GetItemLists()
    {
        return m_itemLists;
    }
}
