using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    [SerializeField] GameObject m_bulletPrefab = null;
    [SerializeField] bool m_activated = false;
    [SerializeField] float m_burstTime = 1f;
    float m_timer;

    private void Update()
    {
        if (m_activated)
        {
            Debug.Log("弾をうちだします");

            m_timer += Time.deltaTime;

            if (m_timer > m_burstTime)
            {
                m_timer = 0;
                Instantiate(m_bulletPrefab, this.transform.position, Quaternion.identity);
            }
        }
    }
}
