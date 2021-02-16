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

    EnemyEffects m_ef;
    EnemyController m_ec;
    GameObject m_gEf;
    private void Start()
    {
        m_ec = GetComponentInParent<EnemyController>();
        this.m_ef = FindObjectOfType<EnemyEffects>();

        Debug.Log(m_ec);
    }

    private void OnDrawGizmos()
    {
        if (m_displayGizmo)
        {
            m_ec.CheckObstacle(m_obstacle);
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
            //敵のまっすぐ前とプレイヤーの位置の角度を計算する
            float angle = Vector3.Angle(this.transform.forward, (m_ec.m_player.transform.position - this.transform.position));
            Debug.Log($"EyeField：プレイヤーとの角度：{angle}");
            /*見たときに、プレイヤーとの角度が設定された範囲内でプレイヤーとの間に障害物がなかったら、あるいはプレイヤーが一定の距離内にいるならば*/
            if ((angle <= m_eyeFieldAngle) && !m_ec.CheckObstacle(m_obstacle) || (m_ec.CheckDistance() && m_ec.CheckObstacle(m_obstacle)))
            {
                /*プレイヤーを発見する*/
                m_ec.OnFoundPlayer();
            }
            else if (m_ec.CheckObstacle(m_obstacle))
            {
                m_ec.OnLostPlayer();
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
