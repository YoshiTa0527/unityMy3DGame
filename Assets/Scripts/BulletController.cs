using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float m_bulletSpeed = 5f;
    [SerializeField] float m_playerHeadLine = 0.2f;
    HumanoidController m_hc;
    Rigidbody m_rb;


    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_hc = FindObjectOfType<HumanoidController>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        /*キャラクターの座標の少し上を狙う*/
        Vector3 shotDir = new Vector3((player.transform.position - this.transform.position).x,
                                      (player.transform.position - this.transform.position).y + 0.5f,
                                      (player.transform.position - this.transform.position).z);
        //Debug.Log($"{shotDir}");
        m_rb.AddForce(shotDir * m_bulletSpeed, ForceMode.Impulse);
    }

    private void Update()
    {
        Destroy(this.gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter:{other.gameObject.name}:に当たった");
        Destroy(this.gameObject);
    }

}
