using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AutomationDoorController : MonoBehaviour
{
    DoorController[] m_door;
    private void Start()
    {
        m_door = GetComponentsInChildren<DoorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("接触Enter");
            m_door.ToList().ForEach(d => d.OpenDoor());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("接触Exit");
            m_door.ToList().ForEach(d => d.CloseDoor());
        }
    }
}
