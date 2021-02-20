using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 気になったことをいろいろためすスクリプト
/// </summary>
public class TestScript : MonoBehaviour
{
    List<string> m_stringList = new List<string> { "あいう", "かきく", "かきく", "かきく", "かきく", "さしす", "たちつ", "なにぬ", };


    // Start is called before the first frame update
    void Start()
    {
        var m_distinctList = m_stringList.Distinct().ToList();
        m_distinctList.ForEach(item => Debug.Log(item.ToString()));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
