﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardController : MonoBehaviour
{
    void Update()
    {
        // オブジェクトとカメラの方向を合わせる
        this.transform.forward = Camera.main.transform.forward;
    }
}
