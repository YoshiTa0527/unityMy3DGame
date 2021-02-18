using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CreateItem")]
public class Item : ScriptableObject
{
    //　アイテムのアイコン
    [SerializeField]
    private Image icon;
    //　アイテムの名前
    [SerializeField]
    private string itemName;
    //　アイテムの情報
    [SerializeField]
    private string information;

    public Image GetIcon()
    {
        return icon;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public string GetInformation()
    {
        return information;
    }

    public Item GetItem()
    {
        return this;
    }
}
