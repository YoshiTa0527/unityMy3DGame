using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 取得したアイテムをアイテムスロットに表示するスクリプト
/// 未完成。所持数上限を設け、一定数以上のアイテムを入手できないようにする
/// </summary>
public class ItemSlotManager : MonoBehaviour
{
    /// <summary>取得したアイテムをこのゲームオブジェクトの子要素にして表示する</summary>
    [SerializeField] GameObject m_AllItemPanel = null;
    /// <summary>アイテムの情報を表示するボタンのプレハブ</summary>
    [SerializeField] GameObject m_itemButtonPanelPrefab = null;
    /// <summary>アイテムからImageが取得できなかった場合に設定するImage</summary>
    [SerializeField] Sprite m_alterImage;
    /// <summary>選択したアイテムをこのゲームオブジェクトの子要素にして表示する</summary>
    [SerializeField] GameObject m_useItemPanel = null;

    /// <summary>アイテム取得履歴を保存しておくためにもう一つリストを作る</summary>
    List<ItemBase> m_getItemBaseHisory = new List<ItemBase>();

    ItemBase m_newItem;
    List<ItemBase> m_itemBase = new List<ItemBase>();

    [SerializeField] EmptyItem m_emptyItem;

    Animator m_anim;

    UseItemPanelController m_uipc;


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
        m_getItemBaseHisory.Add(item);
        m_newItem = item;
        CreateItemPanel();
        Debug.Log("ItemSlotManager::終了");
    }

    public void RemoveFromList(ItemBase item)
    {
        Debug.Log($"Remove::アイテムをリムーブする前{m_itemBase.Count()}");
        m_itemBase.Remove(item);
        Debug.Log($"Remove::アイテムをリムーブする後{m_itemBase.Count()}");
    }

    /// <summary>
    /// 新しく入手したアイテムがすでにリスト内に存在するならtrueを返す
    /// </summary>
    public bool CheckAlreadyExist(ItemBase newItem)
    {
        if (newItem != null && m_itemBase != null)
        {
            /*そのアイテムを持っているかどうかチェックする*/
            /*取得履歴リストの中に、取得したアイテムと同じ名前のアイテムがあるかどうかを確認する*/
            int itemCount = m_getItemBaseHisory.Where(item => newItem.GetItemName() == item.GetItemName()).Count();
            if (itemCount == 1)
            {
                Debug.Log($"CheckAlreadyExist::新しいアイテムを入手しました");
                return false;
            }
            else
            {
                Debug.Log($"CheckAlreadyExist::同じアイテムを{itemCount}持っています");
                return true;
            }
        }
        else { Debug.LogError("何も持っていません！"); return false; }
    }
    GameObject m_itemButton;
    /// <summary>
    /// パネルを生成する
    /// </summary>
    public void CreateItemPanel()
    {
        /*リスト内に、同じ名前のアイテムが一個より多く存在するならパネルを作る*/
        Debug.Log("CreateItemPanel::アイテムを作ります");
        if (!CheckAlreadyExist(m_newItem))
        {
            m_itemButton = Instantiate(m_itemButtonPanelPrefab, m_AllItemPanel.transform);
            m_itemButton.transform.Find("ItemNameText").GetComponent<Text>().text = m_itemBase.Last().GetItemName();
            m_itemButton.GetComponent<Image>().sprite = m_itemBase.Last().GetItemSprite();
            if (!m_itemButton.GetComponent<Image>().sprite) m_itemButton.GetComponent<Image>().sprite = m_alterImage;
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

        Debug.Log($"CheckElements()::履歴の配列の長さ{m_getItemBaseHisory.Count}");
        Debug.Log($"CheckElements()::履歴リストの最初のアイテム{m_getItemBaseHisory.First()}");
        Debug.Log($"CheckElements()::履歴リストの最後のアイテム{m_getItemBaseHisory.Last()}");
        if (m_itemBase != null)
        {
            var counter = m_itemBase.GroupBy(item => item.GetType())
                            .Select(item => new { Key = item.Key, Count = item.Count() }).ToArray();
            foreach (var item in counter)
            {
                Debug.Log($"アイテム{item.Key}を{item.Count}個所持しています");
            }

            var counter2 = m_getItemBaseHisory.GroupBy(item => item.GetType())
                            .Select(item => new { Key = item.Key, Count = item.Count() }).ToArray();
            foreach (var item in counter2)
            {
                Debug.Log($"アイテム{item.Key}を{item.Count}個所持しています");
            }
        }
    }



    bool m_inventoryIsActive;
    private void Start()
    {
        m_uipc = FindObjectOfType<UseItemPanelController>();
        m_anim = m_AllItemPanel.GetComponent<Animator>();
        m_inventoryIsActive = false;
        m_anim.SetBool("IsActive", m_inventoryIsActive);
        m_itemBase.Add(m_emptyItem);
        m_getItemBaseHisory.Add(m_emptyItem);
        CreateItemPanel();
    }

    private void Update()
    {
        //m_uipc.ItemSelecter(m_itemBase);
        if (Input.GetButtonDown("Fire1"))
        {
            //CheckElements();
            //m_uipc.ItemSelecter(m_itemBase);
        }

        if (!m_inventoryIsActive && Input.GetButtonDown("OpenInventory"))
        {
            m_inventoryIsActive = true;
            m_anim.SetBool("IsActive", m_inventoryIsActive);
            m_itemButton.GetComponent<ItemButtonController>().SelectDefaultItem();
            Debug.Log($"メニューを開きます");
        }
        else if (m_inventoryIsActive && Input.GetButtonDown("OpenInventory"))
        {
            m_inventoryIsActive = false;
            m_anim.SetBool("IsActive", m_inventoryIsActive);
            Debug.Log($"メニューを閉じます");
        }

    }

}
