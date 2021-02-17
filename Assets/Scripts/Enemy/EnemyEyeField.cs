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
    [SerializeField] float m_yearDistance = 2f;

    bool m_enterColider;
    float m_areaRadiusTemp;
    float m_yearDisTemp;
    EnemyEffects m_ef;
    EnemyController m_ec;
    GameObject m_gEf;
    /// <summary>
    /// サーチエリアの半径を半分にし、プレイヤーを判定外に出す
    /// </summary>
    public void CloseSearchArea()
    {
        Debug.Log($"EnemyEyeField :CloseRadius:Called");
        m_searchArea.radius /= 2;
        m_yearDistance /= 2;
        Debug.Log($"EnemyEyeField :サーチエリアの半径：{m_searchArea.radius}");
    }

    public IEnumerator CloseSearchAreaCorutine()
    {
        Debug.Log($"EnemyEyeField :CloseRadius:Called");
        m_searchArea.radius /= 2;
        m_yearDistance /= 2;
        Debug.Log($"EnemyEyeField :サーチエリアの半径：{m_searchArea.radius}");
        yield return new WaitForSeconds(2f);
    }
    /// <summary>
    /// サーチエリアの半径を元に戻す
    /// </summary>
    public void OpenSearchArea()
    {
        Debug.Log($"EnemyEyeField: OpenRadius:Called");
        m_searchArea.radius = m_areaRadiusTemp;
        m_yearDistance = m_yearDisTemp;
        Debug.Log($"EnemyEyeField :サーチエリアの半径：{m_searchArea.radius}");
    }
    /// <summary>
    /// プレイヤーが一定の距離にいるなら立ち止まり、プレイヤーの方を見てtrueを返す
    /// </summary>
    public bool CheckDistance()
    {
        Debug.Log("CheckDistance()：CheckDistance()が呼ばれた");
        /*プレイヤーまでの距離*/
        float distance = Vector3.Distance(this.transform.position, m_ec.m_player.transform.position);
        if (distance <= m_yearDistance)
        {

            Debug.Log($"CheckDistance()：プレイヤーが近くにいる！");
            return true;
        }
        else
        {
            Debug.Log($"CheckDistance()：いない！");
            return false;
        }
    }
    private void Start()
    {
        m_enterColider = false;
        m_ec = GetComponentInParent<EnemyController>();
        this.m_ef = FindObjectOfType<EnemyEffects>();
        m_areaRadiusTemp = m_searchArea.radius;
        m_yearDisTemp = m_yearDistance;
        Debug.Log($"EnemyEyeField :保存したサーチエリアの半径{m_areaRadiusTemp}。サーチエリアの半径{m_searchArea.radius}");

        Debug.Log(m_ec);
    }

    private void Update()
    {
        if (m_enterColider)
        {
            //敵のまっすぐ前とプレイヤーの位置の角度を計算する
            float angle = Vector3.Angle(this.transform.forward, (m_ec.m_player.transform.position - this.transform.position));
            //Debug.Log($"EyeField：プレイヤーとの角度：{angle}");
            /*見たときに、プレイヤーとの角度が設定された範囲内でプレイヤーとの間に障害物がなかったら、あるいはプレイヤーが一定の距離内にいるならば*/
            if (!m_ec.CheckObstacle(m_obstacle))
            {
                if (angle <= m_eyeFieldAngle || CheckDistance())
                {
                    /*プレイヤーを発見する*/
                    m_ec.OnFoundPlayer();
                }
            }
            else
            {
                if (angle <= m_eyeFieldAngle || CheckDistance())
                {
                    /*found中だったら*/
                    if (m_ec.GetEnemyStatus() == 2)
                    {
                        m_ec.OnSerchPlayer();
                    }
                }
              
               

            }
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log($"Exit{other.gameObject.name}");
            m_enterColider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log($"Exit{other.gameObject.name}");
            m_enterColider = false;

        }
    }
}
