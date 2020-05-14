using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent agent;

    private void OnTriggerStay(Collider colider)
    {
        if (colider.tag == "Player")
        {
            agent = GetComponent<NavMeshAgent>();
            Vector3 targetPosition = colider.transform.position;
            agent.SetDestination(targetPosition);
        }
    }
}
