using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyがプレイヤーを見つけると起動し、プレイヤーに向かって弾を撃つ
/// </summary>
public class CannonControler : MonoBehaviour
{
    [SerializeField] Animator m_muzzlePrefab = null;
    /// <summary>弾を生産するプレハブ</summary>
    [SerializeField] GameObject m_bulletFactory = null;
    public Vector3 playerPos { get; set; }
    /// <summary>キャノンの状態</summary>
    //public CanonStatus m_status = CanonStatus.NonActive;
    public CanonStatus m_status = CanonStatus.NonActive;
    EnemyController m_ec;
    /// <summary>砲台が飛び出るときのアニメーター</summary>
    Animator m_anim;
    /// <summary>プレイヤー</summary>
    HumanoidController m_hc;

    private void Start()
    {

        m_anim = GetComponent<Animator>();
        m_ec = FindObjectOfType<EnemyController>();
        m_hc = FindObjectOfType<HumanoidController>();
        m_status = CanonStatus.NonActive;

    }

    private void Update()
    {
        playerPos = new Vector3(m_hc.transform.position.x, m_hc.transform.position.y + 0.5f, m_hc.transform.position.z);

        switch (m_status)
        {
            case CanonStatus.NonActive:
                {
                    m_anim.Play("CanonDefault");
                    m_muzzlePrefab.Play("MuzzleDefault");
                    if (m_bulletFactory)
                    {
                        m_bulletFactory.SetActive(false);
                    }
                }
                break;
            case CanonStatus.Active:
                {
                    m_anim.Play("ActiveCanon");
                    m_muzzlePrefab.Play("MuzzleActive");
                    this.gameObject.transform.LookAt(playerPos);

                    if (m_bulletFactory)
                    {
                        m_bulletFactory.SetActive(true);
                    }
                }
                break;
        }
    }



}
public enum CanonStatus
{
    Active, NonActive,
}
