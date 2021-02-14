using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPointManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("GOAL!");
        }
    }
}
