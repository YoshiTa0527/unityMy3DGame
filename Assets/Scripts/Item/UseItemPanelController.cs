using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UseItemPanelController : MonoBehaviour
{
    [SerializeField] Text m_itemNameText;
    [SerializeField] Text m_itemCountText;
    [SerializeField] Image m_itemImage;
    ItemSlotManager m_ism;

    private void Start()
    {
        m_ism = FindObjectOfType<ItemSlotManager>();
    }

    public void SetDate(Text itemNameText, Text itemCountText, Image m_itemImage)
    {
        this.m_itemNameText.text = itemNameText.text;
        this.m_itemCountText.text = itemCountText.text;
        this.m_itemImage.sprite = m_itemImage.sprite;
    }

    int m_index = 0;
    public ItemBase ItemSelecter(List<ItemBase> item)
    {
        /*重複を除いてリストにする*/
        var distinctItemList = item.Select(i => i.GetItemName()).Distinct().ToList();
        Debug.Log($"ItemSelector::配列の長さ{distinctItemList.Count}");
        /*マウスホイールでアイテムを切り替える*/
        //if (distinctItemList.Count > 1)
        //{
        //    float f = Input.GetAxis("Mouse ScrollWheel");
        //    int intIndex = (int)(f * 10);
        //    if (intIndex > 1) { intIndex = 1; }
        //    else if (intIndex < -1) { intIndex = -1; }

        //    int countIndexTemp = 0;

        //    m_index += intIndex;
        //    if (m_index < 0) { Debug.Log("indexが0を下回った"); m_index = 0; }
        //    else if (m_index > distinctItemList.Count) { Debug.Log("indexが配列の長さをうわまった"); m_index = distinctItemList.Count - 1; }
            

        //    Debug.Log($"ItemSelector::{m_index}");

        //}

        /*アイテムベースの中にある、distinctlistと同じ名前のアイテムを返す*/
        ItemBase selectedItem = item.Where(i => distinctItemList[m_index] == i.GetItemName()).FirstOrDefault();
        return selectedItem;

    }
}
