using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : ItemBase
{
    [SerializeField] Transform m_hidePos;
    [SerializeField] int m_healPoint = 2;


    GameObject m_player;
    ItemSlotManager m_ism;
    PlayerControllerAI m_pca;
    ItemDataBase m_itemDataBase;
    [SerializeField] Item m_item;
    private void Start()
    {

        m_ism = FindObjectOfType<ItemSlotManager>();
        m_pca = FindObjectOfType<PlayerControllerAI>();

        this.gameObject.name = m_item.GetItemName();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("item::当たった");
            AddMyselfToPlayer();
        }
    }
    void AddMyselfToPlayer()
    {
        m_ism.SetItem(this.m_item.GetItemName());
        this.transform.position = m_hidePos.position;
    }
    public override void Use()
    {

        m_pca.Heal(m_healPoint);

    }
}
