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
    private void Update()
    {
        float f = Input.GetAxis("Mouse ScrollWheel");
        int i = (int)(f * 10);
        if (i > 1)
        {
            i = 1;
        }
        else if (i < -1)
        {
            i = -1;
        }
        Debug.Log($"TestScript::{i}");
    }
}
