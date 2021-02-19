using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// 取得したアイテムをアイテムスロットに表示するスクリプト
/// </summary>
public class ItemSlotManager : MonoBehaviour
{
    /// <summary>取得したアイテムをこのゲームオブジェクトの子要素にして表示する</summary>
    [SerializeField] GameObject m_AllItemPanel = null;
    /// <summary>アイテムの情報を表示するボタンのプレハブ</summary>
    [SerializeField] GameObject m_itemButtonPanelPrefab = null;
    /// <summary>アイテムからImageが取得できなかった場合に設定するImage</summary>
    [SerializeField] Image m_alterImage;


    ItemBase m_newItem;
    List<ItemBase> m_itemBase = new List<ItemBase>();

    [SerializeField] EmptyItem m_emptyItem;
    [SerializeField] HealthItem m_hItem;
    [SerializeField] SuperItem m_sItem;


    public List<ItemBase> GetItemBaseList()
    {
        return m_itemBase;
    }

    /// <summary>
    /// プレイヤーがアイテムと接触したときにアイテムが呼ぶ関数。
    /// アイテムをアイテムリストに加えて表示を更新する
    /// </summary>
    public void AddToList(ItemBase item)
    {
        Debug.Log("ItemSlotManager::呼ばれた");
        /*とりあえずリストに加える*/
        m_itemBase.Add(item);
        m_newItem = item;
        CreateItemPanel();


        Debug.Log("ItemSlotManager::終了");
    }

    public void RemoveFromList(ItemBase item)
    {
        m_itemBase.Remove(item);
    }

    /// <summary>
    /// 新しく入手したアイテムがすでにリスト内に存在するならtrueを返す
    /// </summary>
    public bool CheckAlreadyExist(ItemBase newItem)
    {
        if (newItem != null && m_itemBase != null)
        {
            /*そのアイテムを持っているかどうかチェックする*/
            /*リストの中に、同じ名前のアイテムが幾つあるか数える*/
            int itemCount = m_itemBase.Where(item => newItem.GetItemName() == item.GetItemName()).Count();
            /*アイテムの個数が一個だけだったら初めて入手したアイテム*/
            if (itemCount == 1)
            {
                Debug.Log($"CheckAlreadyExist::新しいアイテムを{itemCount}個入手しました");
                return false;
            }
            else
            {
                Debug.Log($"CheckAlreadyExist::同じアイテムを既に{itemCount}個持っています");
                return true;
            }
        }
        else { Debug.LogError("何も持っていません！"); return false; }
    }

    /// <summary>
    /// パネルを生成する
    /// </summary>
    public void CreateItemPanel()
    {
        /*リスト内に、同じ名前のアイテムが、一個以上存在するならパネルを作る*/
        Debug.Log("CreateItemPanel::アイテムを作ります");
        if (!CheckAlreadyExist(m_newItem))
        {
            GameObject itemButton = Instantiate(m_itemButtonPanelPrefab, m_AllItemPanel.transform);
            //Image itemImage = m_itemBase.Last().GetItemImage();
            itemButton.transform.Find("ItemNameText").GetComponent<Text>().text = m_itemBase.Last().GetItemName();


            //if (!itemImage) itemImage = m_alterImage;
            ////string itemName = m_itemBase.Last().GetItemName();
            //if (itemName == null) itemName = "aiueo";
        }

    }
    /// <summary>
    /// リストの中身を確認する
    /// </summary>
    void CheckElements()
    {
        Debug.Log("CheckElements()");
        Debug.Log($"CheckElements()::現在の配列の長さ{m_itemBase.Count}");
        Debug.Log($"CheckElements()::リストの最初のアイテム{m_itemBase.First()}");
        Debug.Log($"CheckElements()::リストの最後のアイテム{m_itemBase.Last()}");
        if (m_itemBase != null)
        {
            var counter = m_itemBase.GroupBy(item => item.GetType())
                            .Select(item => new { Key = item.Key, Count = item.Count() }).ToArray();
            foreach (var item in counter)
            {
                Debug.Log($"アイテム{item.Key}を{item.Count}個所持しています");
            }
        }

    }



    private void Start()
    {

        m_itemBase.Add(m_emptyItem);
        CreateItemPanel();
        //CreateItemPanel();
        //m_itemBase.Add(m_hItem);
        //m_itemBase.Add(m_sItem);
        //Debug.Log($"リストの動作確認::現在の配列の長さ{m_itemBase.Count}");
        //for (int i = 0; i < m_itemBase.Count; i++)
        //{
        //    Debug.Log($"{i}番目のアイテムは{m_itemBase[i].GetItemName()}です");
        //}



    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            CheckElements();
        }
    }
}
