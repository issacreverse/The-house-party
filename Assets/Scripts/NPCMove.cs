using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoTO(Vector3 dest)
    {
        agent.SetDestination(dest);
    }
    void Update()
    {
        
    }
}
