using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrolling,
    Chasing,
    Returning,
    Attacking
}

public class SmartEnemyController : MonoBehaviour
{
    public float MinDistanceToFollow = 10.0f;
    public float MinDistanceToPatrolPoint = 2.0f;
    public float MinDistanceToAttack = 0.5f;

    public GameObject PointToPatroll;
    public float patrollAreaRadius = 4.0f;
    public GameObject target;

    NavMeshAgent agent;
    EnemyState state;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        state = EnemyState.Patrolling;
    }

    void Update()
    {
        switch (state)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
            case EnemyState.Returning:
                Return();
                break;
        }
    }

    private void Patrol()
    {
        if (!agent.hasPath)
        {
            Vector2 randomPoint = Random.insideUnitCircle;
            randomPoint *= patrollAreaRadius;

            Vector3 destination = PointToPatroll.transform.position;
            destination.x += randomPoint.x;
            destination.z += randomPoint.y;

            agent.SetDestination(destination);
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < MinDistanceToFollow)
        {
            state = EnemyState.Chasing;
            agent.ResetPath();
        }
    }

    private void Chase()
    {
        agent.SetDestination(target.transform.position);

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance >= MinDistanceToFollow)
        {
            state = EnemyState.Returning;
            agent.ResetPath();
        }
        else if (distance <= MinDistanceToAttack)
        {
            state = EnemyState.Attacking;
            agent.ResetPath();
        }
    }

    private void Attack()
    {
        // TODO

        state = EnemyState.Returning;
        agent.ResetPath();
    }

    private void Return()
    {
        if (!agent.hasPath)
        {
            Vector3 destination = PointToPatroll.transform.position;
            agent.SetDestination(destination);
        }

        if (Vector3.Distance(transform.position, target.transform.position) < MinDistanceToFollow)
        {
            state = EnemyState.Chasing;
            agent.ResetPath();
        }
        else if (Vector3.Distance(transform.position, PointToPatroll.transform.position) < MinDistanceToPatrolPoint)
        {
            state = EnemyState.Patrolling;
            agent.ResetPath();
        }
    }
}
