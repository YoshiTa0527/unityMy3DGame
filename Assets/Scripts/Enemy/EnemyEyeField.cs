using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 敵の視界
/// </summary>
public class EnemyEyeField : MonoBehaviour
{
    /// <summary>敵がプレイヤーを発見するときの視界の角度</summary>
    [SerializeField] float m_eyeFieldAngle = 45f;
    [SerializeField] LayerMask m_obstacle;
    [SerializeField] SphereCollider m_searchArea;
    [SerializeField] bool m_displayGizmo = true;


    EnemyController m_ec;
    private void Start()
    {
        m_ec = GetComponentInParent<EnemyController>();
        Debug.Log(m_ec);
    }

    private void OnDrawGizmos()
    {
        if (m_displayGizmo)
        {
            Handles.color = Color.red;
            Handles.DrawSolidArc(transform.position, Vector3.up,
                                Quaternion.Euler(0f, -m_eyeFieldAngle, 0f) * transform.forward,
                                m_eyeFieldAngle * 2f, m_searchArea.radius);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        /*プレイヤーがコライダーに侵入したら*/
        if (other.tag == "Player")
        {
            /*一定の距離以内にいるか判定する*/
            if (m_ec.FindPlayerByYear())
            {
                /*プレイヤーの方向を見る*/
                m_ec.LookAtPlayer();
                //敵のまっすぐ前とプレイヤーの位置の角度
                float angle = Vector3.Angle(this.transform.forward, m_ec.m_playersDirection);
                /*見たときに、プレイヤーとの角度が設定された範囲内、かつ、プレイヤーとの間に障害物がなかったら*/
                if ((angle <= m_eyeFieldAngle) && !m_ec.CheckObstacle(m_obstacle))
                {
                    /*プレイヤーを発見する*/
                    m_ec.OnFoundPlayerByEye();
                }
                else
                {
                    m_ec.OnLostPlayer();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_ec.OnLostPlayer();
        }
    }
}
