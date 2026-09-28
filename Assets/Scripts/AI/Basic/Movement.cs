using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float stoppingDistance = 0.1f;
    [SerializeField] private float rotationSpeed = 5f;
    private bool hasReachedDestination = false;
    public Vector3 Target;
    private NavMeshAgent agent;


    private void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = true;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        agent.SetDestination(Target);

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 1.2f)
                {
                    hasReachedDestination = true;
                }
            }
            else
            {
                hasReachedDestination = false;
            }
        }
    }

    private void HandleRotation()
    {
        Vector3 direction = (Target - transform.position).normalized;

        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            Vector3 moveDirection = agent.velocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public bool HasReachedTarget()
    {
        return hasReachedDestination;
    }

    public Vector3 GetDestination()
    {
        return Target;
    }

    public void SetDestination(Vector3 target)
    {
        Target = target;
    }
}
