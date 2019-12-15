using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrolling,
    Chasing,
    Returning
}

public class EnemyController : MonoBehaviour
{
    public float MinDistanceToFollow = 10.0f;
    public float MinDistanceToPatrolPoint = 2.0f;

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
        if (state == EnemyState.Patrolling)
        {
            Patrol();
        }
        else if (state == EnemyState.Chasing)
        {
            Chasing();
        }
        else if (state == EnemyState.Returning)
        {
            Return();
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

    private void Chasing()
    {
        if (!agent.hasPath)
        {
            agent.SetDestination(target.transform.position);
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance >= MinDistanceToFollow)
        {
            state = EnemyState.Returning;
            agent.ResetPath();
        }
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
