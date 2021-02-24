using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
/// <summary>
/// アイテムのインベントリにクローンされるボタンのスクリプト
/// 未完成。アイテムインベントリにあるアイテムを、キーボード操作で選択、使用できるようにする
/// </summary>
public class ItemButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    Text m_itemNameText;
    Text m_itemCountText;
    /// <summary>アイテムが0個の時に鳴らすサウンド</summary>
    [SerializeField] AudioClip m_audio = null;

    /// <summary>情報を表示するときに使うフィールド</summary>
    Text m_itemInformationText;
    [SerializeField] GameObject m_itemInfoPanel;
    Animator m_itemInfoAnim;
    bool m_infoIsActive;
    /// <summary>アイテムをセットする場所</summary>
    GameObject m_itemPanelToSet;


    ItemSlotManager m_ism;
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
    }

    /// <summary>
    /// マウスカーソルが重なっているときにそのアイテムのInformationを表示する
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        m_infoIsActive = true;
        m_itemInfoAnim.SetBool("IsActive", m_infoIsActive);
    }
    /// <summary>
    /// マウスカーソルが外れたときにアイテムのInformationを閉じる
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        m_infoIsActive = false;
        m_itemInfoAnim.SetBool("IsActive", m_infoIsActive);
    }


    private void Start()
    {
        m_itemPanelToSet = GameObject.Find("UseItemPanel");

        m_ism = FindObjectOfType<ItemSlotManager>();
        m_itemCountText = this.transform.Find("ItemCountText").GetComponent<Text>();
        m_itemNameText = this.transform.Find("ItemNameText").GetComponent<Text>();

        m_itemInfoAnim = m_itemInfoPanel.GetComponent<Animator>();
        m_infoIsActive = false;
        m_itemInfoAnim.SetBool("IsActive", m_infoIsActive);
        m_itemInformationText = m_itemInfoPanel.transform.Find("ItemInfo").GetComponent<Text>();
        m_itemInformationText.text = m_ism.GetItemBaseList().Where(item => item.GetItemName() == this.m_itemNameText.text).First().GetItemInforMation();

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
            m_itemCountText.text = ItemCounter(m_ism.GetItemBaseList()).ToString();
        }
    }

    public void UseItem()
    {
        if (ItemCounter(m_ism.GetItemBaseList()) == 0 || this.m_itemNameText.text == "空きスロット")
        {
            AudioSource.PlayClipAtPoint(this.m_audio, Camera.main.transform.position);
        }
        else
        {
            m_ism.GetItemBaseList().Where(item => item.GetItemName() == this.m_itemNameText.text).First().Use();
        }
    }

    public void SetItemToUseSlot()
    {
        /*イメージ*/
        m_itemPanelToSet.transform.Find("ItemImage").GetComponent<Image>().sprite = this.gameObject.GetComponent<Image>().sprite;
        /*名前*/
        m_itemPanelToSet.transform.Find("ItemNameText").GetComponent<Text>().text = this.m_itemNameText.text;
        /*個数*/
        m_itemPanelToSet.transform.Find("ItemCountText").GetComponent<Text>().text = this.m_itemCountText.text;
    }

    /// <summary>
    /// 引数から受け取ったリストの中に、自分と同じ名前のアイテムがいくつあるかを返す
    /// </summary>
    /// <returns></returns>
    int ItemCounter(List<ItemBase> itemList)
    {
        return itemList.Where(item => item.GetItemName() == this.m_itemNameText.text).Count();
    }
}
