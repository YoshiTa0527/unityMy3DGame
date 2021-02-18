using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 取得したアイテムをアイテムスロットに表示するスクリプト
/// </summary>
public class ItemSlotManager : MonoBehaviour
{
    [SerializeField] ItemDataBase m_itemDataBase;
    /// <summary>アイテム数を管理するディクショナリー</summary>
    Dictionary<Item, int> numOfItem = new Dictionary<Item, int>();
    /// <summary>取得したアイテムを表示するUI</summary>
    [SerializeField] GameObject m_itemUI = null;
    Text m_itemName;
    Text m_itemCount;
    Image m_itemImage;
    List<GameObject> m_itemList = new List<GameObject>();
    List<ItemBase> m_itemBaseList = new List<ItemBase>();

    private void Start()
    {
        m_itemName = GameObject.Find("ItemNameText").GetComponent<Text>();
        if (m_itemName) { Debug.Log(m_itemName.gameObject.name); }
        m_itemCount = GameObject.Find("ItemCountText").GetComponent<Text>();
        m_itemImage = GameObject.Find("Itemimage").GetComponent<Image>();

        /*とりあえず０個アイテムを持っておく*/

        //test
        //for (int i = 0; i < m_itemDataBase.GetItemLists().Count; i++)
        //{
        //    //適当にアイテムの数を設定する
        //    numOfItem.Add(m_itemDataBase.GetItemLists()[i], 0);
        //    //　確認の為データ出力
        //    Debug.Log(m_itemDataBase.GetItemLists()[i].GetItemName() + ": " + m_itemDataBase.GetItemLists()[i].GetInformation());

        //}

        //Debug.Log(numOfItem[GetItem("HealthPack")]);
    }

    int m_value = 0;
    public void SetItem(string item)
    {
        m_value++;
        /*addするのはitemdatabaseのリストの一要素*/
        numOfItem[GetItem("HealthPack")] = m_value;
        Debug.Log($"アイテムを拾った::m_value:{m_value}");
    }

    public Item GetItem(string searchName)
    {
        if (m_itemDataBase.GetItemLists().Find(itemName => itemName.GetItemName() == searchName))
        {
            return m_itemDataBase.GetItemLists().Find(itemName => itemName.GetItemName() == searchName);
        }
        else
        {
            Debug.LogError("GetItem　エラー");
            return null;
        }
    }
    /// <summary>
    /// リストにアイテムを加える
    /// </summary>
    /// <param name="item"></param>
    //public void SetItem(GameObject item)
    //{
    //    //m_itemList.Add(item);
    //    m_itemBaseList.Add(item);
    //}

    //public void SetItem(ItemBase item)
    //{
    //    m_itemBaseList.Add(item);
    //}
    /// <summary>
    /// リストのアイテムによって表示を更新する
    /// </summary>
    void UpdateItemList()
    {
        //m_itemCount.text = m_itemList.Count.ToString();
        m_itemCount.text = numOfItem[GetItem("HealthPack")].ToString();


    }

    private void Update()
    {
        /*アイテムを持っているなら、アイテムリストの画像、名前、個数を表示する*/
        if (numOfItem != null)
        {
            UpdateItemList();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            m_itemBaseList[0].Use();
        }
    }



}
