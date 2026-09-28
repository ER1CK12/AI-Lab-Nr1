using Unity.VisualScripting;
using UnityEngine;

public class Follow_En_Behaviour : MonoBehaviour
{
    private StateMachine stateMachine;
    private Movement movement;
    private Detection detection;
    private Player_Health playerHealth;
    public GameObject realPlayerPos;

    [SerializeField] private float idleTime = 3f;
    [SerializeField] private float searchTime = 2f;
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float attackCooldown = 2f;

    [SerializeField] private Transform[] patrolPoints = new Transform[5];

    [SerializeField] private float stateTimer = 0f;
    [SerializeField] private int currentPatrolIndex = 0;
    [SerializeField] private float lastAttackTime;
    private Vector3 lastPlayerPosition;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        movement = GetComponent<Movement>();
        detection = GetComponent<Detection>();
    }

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Health>();
    }
    

    private void Update()
    {
        switch (stateMachine.currentState)
        {
            case StateMachine.States.idle:
                HandleIdleState();
                break;
            case StateMachine.States.patrol:
                HandlePatrolState();
                break;
            case StateMachine.States.chase:
                HandleChaseState();
                break;
            case StateMachine.States.attack:
                HandleAttackState();
                break;
            case StateMachine.States.search:
                HandleSearchState();
                break;
        }
    }

    private void HandleIdleState()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer >= idleTime)
        {
            stateMachine.currentState = StateMachine.States.patrol;
            stateTimer = 0f;
        }
    }
    private void HandlePatrolState()
    {
        movement.SetDestination(patrolPoints[currentPatrolIndex].position);
        Debug.Log("Patrol State initiated");

        if (Vector3.Distance(transform.position, movement.Target) < 1.2f)
        {
            Debug.Log("Reached patrol point");
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Length)
            {
                currentPatrolIndex = 0;
            }
        }
        if (detection.isPlayerDetected)
        {
            stateMachine.currentState = StateMachine.States.chase;
            lastPlayerPosition = detection.player;
        }
        
    }

    private void HandleChaseState()
    {
        if (detection.isPlayerDetected == true)
        {
            Debug.Log("Chase State initiated");
            lastPlayerPosition = detection.player;
            movement.SetDestination(lastPlayerPosition);

            if (Vector3.Distance(transform.position, lastPlayerPosition) < 1.3f)
            {
                stateMachine.currentState = StateMachine.States.attack;
            }
        }
        else
        {
            Debug.Log("Player not detected, searching...");
            stateMachine.currentState = StateMachine.States.search;
            stateTimer = 0f;
        }
    }

    private void HandleAttackState()
    {
        Debug.Log("Attack State initiated");
        lastAttackTime += Time.deltaTime;
        if (Vector3.Distance(transform.position, realPlayerPos.transform.position) < 1.3f)
        {
            if (lastAttackTime >= attackCooldown)
            {
                playerHealth.TakeDamage(10);
                lastAttackTime = 0f;
                Debug.Log("Player damaged");
            }
        } else if (!detection.isPlayerDetected)
        {
            Debug.Log("Player out of range, searching...");
            stateMachine.currentState = StateMachine.States.search;
        } else
        {
            stateMachine.currentState = StateMachine.States.chase;
            lastPlayerPosition = detection.player;
        }
        
    }

    private void HandleSearchState()
    {
        Debug.Log("Search State initiated");
        Vector3 distance = movement.GetDestination() - transform.position; 
        if (distance.magnitude < 0.1f)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer >= searchTime)
            {
                Debug.Log("Search time elapsed, returning to idle state.");
                stateMachine.currentState = StateMachine.States.idle;
                stateTimer = 0f;
            }

        }

        if (detection.isPlayerDetected)
        {
            stateMachine.currentState = StateMachine.States.chase;
            lastPlayerPosition = detection.player;
        }
    }

    
}
