using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPointManager : MonoBehaviour
{
    [SerializeField] GameObject m_goalCutscene = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("GoalPointManager : GOAL!");
            other.gameObject.SetActive(false);
            Instantiate(m_goalCutscene, this.transform.position, Quaternion.identity);
        }
    }
}
