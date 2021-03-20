using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AutomationDoorController : MonoBehaviour
{
    DoorController[] m_door;

    [SerializeField] Light m_doorLight;
    [SerializeField] Color m_changeColor;
    Color m_doorLightDefaultColor;
    public bool m_isLocked { get; set; }

    GameManager m_gm;
    private void Start()
    {
        m_door = GetComponentsInChildren<DoorController>();
        m_gm = FindObjectOfType<GameManager>();
        m_doorLightDefaultColor = m_doorLight.color;
    }

    void ChangeDoorLight()
    {
        m_doorLight.color = m_changeColor;
    }

    private void FixedUpdate()
    {
        if (GameManager.PlayerIsFound)
        {
            ChangeDoorLight();
        }
        else
        {
            m_doorLight.color = m_doorLightDefaultColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("接触Enter");
            if (!GameManager.PlayerIsFound)
            {
                m_door.ToList().ForEach(d => d.OpenDoor());
            }
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

