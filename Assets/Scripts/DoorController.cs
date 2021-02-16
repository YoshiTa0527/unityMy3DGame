using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator m_anim = null;

    private void Start()
    {
        this.m_anim = GetComponent<Animator>();
        if (this.m_anim != null) { Debug.LogError("アニメーターを取得できていない  "); }
        else { Debug.Log($"DoorController：{this.m_anim.gameObject.name}"); }
        CloseDoor();
    }

    public void OpenDoor()
    {
        this.m_anim.SetBool("IsOpened", true);
    }
    public void CloseDoor()
    {
        this.m_anim.SetBool("IsOpened", false);
    }
}
