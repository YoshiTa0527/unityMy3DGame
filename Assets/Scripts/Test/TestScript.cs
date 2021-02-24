using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 気になったことをいろいろためすスクリプト
/// </summary>
public class TestScript : MonoBehaviour
{
    //変更を監視する値
    private int _value = 0;

    //値が変更された時に実行されるイベント
    public event Action<int> ChangedValue = delegate { };

    public event Action<bool> ChangedBoolValue = delegate { };

    [SerializeField] bool m_check = false;
    private void Start()
    {
        //てきとうに値を変更
        SetValue(1);
        SetValue(1);
        SetValue(2);
        SetValue(2);
        SetValue(1);
    }

    private void Update()
    {
        SetValue(m_check);
    }
    //値を設定する
    private void SetValue(int value)
    {
        //同じ値が来た場合は設定しないし、イベントも実行しない
        if (_value == value)
        {
            return;
        }

        _value = value;
        ChangedValue(_value);
    }

    private void SetValue(bool value)
    {
        //同じ値が来た場合は設定しないし、イベントも実行しない
        if (m_check == value)
        {
            Debug.Log("値が一緒");
            return;
        }

        m_check = value;
        ChangedBoolValue(m_check);
        Debug.Log("値が変更された");
    }
}
