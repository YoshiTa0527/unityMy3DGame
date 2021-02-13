using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuController : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button m_firstButton = null;
    void Start()
    {
        m_firstButton.Select();

        if (m_firstButton == null) Debug.Log("a");
        Debug.Log("b");
    }


}
