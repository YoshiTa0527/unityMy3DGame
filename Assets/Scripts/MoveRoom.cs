using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoom : MonoBehaviour
{
    /// <summary>動かすオブジェクト</summary>
    [SerializeField] GameObject m_moveObject = null;
    /// <summary>動かす速度</summary>
    [SerializeField] float m_speed = 20f;
    /// <summary>動かす方向</summary>
    [SerializeField] Vector3 m_moveDir = Vector3.forward;
    [SerializeField] float m_MaxMoveDis = 20f;
    Vector3 m_startPosition;
    Rigidbody m_rb;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_startPosition = this.transform.position;
    }

    private void Update()
    {
        float moveDistance = Vector3.Distance(this.transform.position, m_startPosition);
        if (moveDistance >= m_MaxMoveDis)
        {
            this.transform.position = m_startPosition;
        }

        Vector3 velo = m_moveDir * m_speed;
        m_rb.velocity = velo;
    }


}
