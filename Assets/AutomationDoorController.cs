using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AutomationDoorController : MonoBehaviour
{
    DoorController[] m_door;
    [SerializeField] DoorStatus m_doorStatus;
    public bool m_isLocked { get; set; }
    private void Start()
    {
        m_door = GetComponentsInChildren<DoorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("接触Enter");

            switch (m_doorStatus)
            {
                case DoorStatus.StartDoor:
                    m_door.ToList().ForEach(d => d.OpenDoor());
                    break;
                case DoorStatus.GoalDoor:
                    if (!m_isLocked)
                    {
                        m_door.ToList().ForEach(d => d.OpenDoor());
                    }
                    break;
                default:
                    m_door.ToList().ForEach(d => d.OpenDoor());
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("接触Exit");

            switch (m_doorStatus)
            {
                case DoorStatus.StartDoor:
                    m_door.ToList().ForEach(d => d.CloseDoor());
                    break;
                case DoorStatus.GoalDoor:
                    if (!m_isLocked)
                    {
                        m_door.ToList().ForEach(d => d.CloseDoor());
                    }
                    break;
                default:
                    m_door.ToList().ForEach(d => d.CloseDoor());
                    break;
            }

        }
    }
}


enum DoorStatus
{
    StartDoor = 1,
    GoalDoor = 2,
}
