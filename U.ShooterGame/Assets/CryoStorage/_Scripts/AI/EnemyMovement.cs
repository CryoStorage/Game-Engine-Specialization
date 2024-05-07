using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform _target;
    private EnemyParameters _enemyParameters;
    private Enemy _parentEnemy;

    public void Initiate(EnemyParameters parameters, Transform target, Enemy parentEnemy)
    {
        _enemyParameters = parameters;
        _target = target;
        _parentEnemy = parentEnemy;
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _enemyParameters.speed;
    }
    
    private void Update()
    {
        _navMeshAgent.isStopped = _parentEnemy.CurrentState != EnemyState.Moving;
        Move();
    }
    
    private void Move()
    {
        _navMeshAgent.SetDestination(_target.position);
    }

    public void WarpTo(Vector3 position)
    {
        _navMeshAgent.Warp(position);
    }
}
