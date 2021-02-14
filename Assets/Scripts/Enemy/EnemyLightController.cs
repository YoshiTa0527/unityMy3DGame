using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLightController : MonoBehaviour
{
    [SerializeField] Light m_light = null;
    /// <summary>プレイヤーを見つけたときにこの色に変える</summary>
    [SerializeField] Color m_foundColor;
    [SerializeField] float m_intensity = 5f;
    /// <summary>プレイヤーを見失ったときにこの色に変える</summary>
    [SerializeField] Color m_lostColor;

    Color m_colorTemp;
    float m_intensityTemp;
    Animator m_anim;

    private void Start()
    {
        m_colorTemp = m_light.color;
        m_intensityTemp = m_light.intensity;
        m_anim = GetComponent<Animator>();
    }

    public void ChangeLightColorWhenFound()
    {
        Debug.Log($" ChangeLightColorWhenFound()：Called");
        m_light.color = m_foundColor;
        m_light.intensity = m_intensity;
    }

    public void ChangeLightColorWhenLost()
    {
        Debug.Log($"ChangeLightColorWhenLost()：Called");
        m_anim.SetBool("LostPlayer", true);
        Debug.Log($"ChangeLightColorWhenLost()：セットブール！");
        m_light.color = m_lostColor;
        m_light.intensity = m_intensity;
        
    }

    public void ResetLightColor()
    {
        Debug.Log($" ResetLightColor():Called");
        m_anim.SetBool("LostPlayer", false);
        m_light.color = m_colorTemp;
        m_light.intensity = m_intensityTemp;
       
    }
}
