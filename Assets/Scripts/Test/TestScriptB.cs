using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptB : MonoBehaviour
{
    [SerializeField]
    private TestScript m_testScript = null;
    private void Awake()
    {
        //値が変更されたらログを出すように
        //m_testScript.ChangedValue += (value) => Debug.Log(value);

    }

    private void Update()
    {
        m_testScript.ChangedBoolValue += (value) => Debug.Log(value);
    }
}
