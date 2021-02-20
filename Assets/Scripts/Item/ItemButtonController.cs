using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class ItemButtonController : MonoBehaviour
{
    Image m_itemImage;
    Button m_button;
    Text m_itemNameText;

    [SerializeField] Text m_itemCountText;

    ItemSlotManager m_ism;
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
    }


    private void Start()
    {
        m_ism = FindObjectOfType<ItemSlotManager>();
        m_itemImage = this.gameObject.GetComponent<Image>();
        m_button = this.gameObject.GetComponent<Button>();
        m_itemCountText = this.transform.Find("ItemCountText").GetComponent<Text>();
        m_itemNameText = this.transform.Find("ItemNameText").GetComponent<Text>();
        if (this.m_itemNameText.text == "空きスロット")
        {
            m_itemCountText.text = "";
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }

    private void Update()
    {
        //アイテムの個数は、ItemSlotManagerからリストを受け取り、そのリストの中にある”自分と同じ名前のアイテム”の個数を設定する
        if (this.m_itemNameText.text == "空きスロット")
        {
            m_itemCountText.text = "";
        }
        else
        {
            m_itemCountText.text = m_ism.GetItemBaseList().Where(item => item.GetItemName() == this.m_itemNameText.text).Count().ToString();
        }
    }

    public void UseItem()
    {
        m_ism.GetItemBaseList().Where(item => item.GetItemName() == this.m_itemNameText.text).First().Use();
    }
}
