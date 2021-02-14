using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineFreeLook m_vcum = null;
    [SerializeField] float m_speed = 20f;
    /// <summary>マウスのYのスピードを保存しておく変数</summary>
    [SerializeField] float m_YAxisSpeed = 10f;
    /// <summary>マウスのXのスピードを保存しておく変数</summary>
    [SerializeField] float m_XAxisSpeed = 100f;
    float m_scroll;

    private void Start()
    {
        m_vcum = GetComponent<CinemachineFreeLook>();
    }
    private void Update()
    {
        m_scroll = -1 * (Input.GetAxis("Mouse ScrollWheel"));
        m_vcum.m_Lens.FieldOfView += m_scroll * m_speed;

        if (Input.GetButton("Fire2"))
        {
            Debug.Log("右クリックが押された");
            m_vcum.m_YAxis.m_MaxSpeed = m_YAxisSpeed;
            m_vcum.m_XAxis.m_MaxSpeed = m_XAxisSpeed;
            Debug.Log($"Yのスピード：{m_vcum.m_YAxis.m_MaxSpeed}　保存したYのスピード{ m_YAxisSpeed}");
            Debug.Log($"Xのスピード：{ m_vcum.m_XAxis.m_MaxSpeed}");
        }
        else
        {
            m_vcum.m_YAxis.m_MaxSpeed = 0;
            m_vcum.m_XAxis.m_MaxSpeed = 0;
        }
    }

    public void LockCamera()
    {
        Debug.Log("LockCamera()");
    }
}
