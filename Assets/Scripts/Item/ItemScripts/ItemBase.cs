using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アイテムのベースとなる抽象クラス。アイテムはこのクラスを継承して使う
/// </summary>
public class ItemBase : MonoBehaviour
{
    [SerializeField] string m_itemName = null;
    [SerializeField] string m_itemInformation = null;
    [SerializeField] Sprite m_itemImage = null;
    [SerializeField] Transform m_hidePos = null;
    ItemSlotManager m_ism;
    private void Start()
    {
        m_ism = FindObjectOfType<ItemSlotManager>();
        if (m_ism) { Debug.Log("ItemBase::取得成功"); }
        else { Debug.LogError("ItemBase::取得失敗"); }
    }
    public string GetItemName()
    {
        return this.m_itemName;
    }

    public string GetItemInforMation()
    {
        return this.m_itemInformation;
    }

    public Sprite GetItemSprite()
    {
        return this.m_itemImage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("アイテムを拾った");
            m_ism.AddToList(this);
            if (m_hidePos != null)
            {
                this.transform.position = m_hidePos.position;
            }
            else { Debug.LogError("ハイドポジションが設定されていない"); }
        }
    }

    public virtual void Use()
    {
        Debug.Log("アイテムを使います");
        m_ism.RemoveFromList(this);
    }
}
