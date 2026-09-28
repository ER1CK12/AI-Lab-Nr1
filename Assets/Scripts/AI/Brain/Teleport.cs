using UnityEngine;
using UnityEngine.AI;

public class Teleport : MonoBehaviour
{
    public Transform actualPalyerPosition;
    private Transform transform;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        transform = GetComponent<Transform>();
    }



    public void TeleportToPlayer()
    {
        Vector3 targetPosition = actualPalyerPosition.position;

        Vector3 offset = new Vector3(2f, 0f, 0f); 
        targetPosition += offset;

        agent.Warp(targetPosition);

    }



}
