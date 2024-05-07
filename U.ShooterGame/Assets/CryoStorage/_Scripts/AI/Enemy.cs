using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public enum EnemyState
    {
        Moving,
        Shooting,
        Dead
    }
public class Enemy : MonoBehaviour,IAimTarget, IAttackable, IPoolableGo<Enemy>
{
    public ObjectPool<Enemy> ParentPool { get; set; }
    public Vector3 Position => transform.position;
    public Vector3 Velocity => _rb.velocity;
    public EnemyState CurrentState => _currentState;
    
    [Header("Enemy Settings")]
    [SerializeField] private EnemyParameters enemyParameters;
    [SerializeField]private LayerMask rayMask;
    
    [Header("Serialized References")]
    private EnemyShoot _enemyShoot;
    private EnemyMovement _enemyMovement;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onEnemyDied;
    [SerializeField] private GameEvent onEnemyHit;

    private Rigidbody _rb;
    private MeshRenderer _meshRenderer;
    private Animator _animator;
    private Collider _collider;
    private int _currentHealth;
    private EnemyState _currentState;
    private Transform _playerTransform;
    
    bool _initialized;
    private static readonly int Destroyed = Animator.StringToHash("destroyed");

    private void Initialize()
    {
        if (_initialized) return;
        Prepare();
        _initialized = true;
    }

    private void Update()
    {
        if (_currentState == EnemyState.Dead) return;
        var dist = Vector3.Distance(transform.position, _playerTransform.position);
        if (dist > enemyParameters.attackRange)
        {
            _currentState = EnemyState.Moving;
            return;
        }
        _currentState = CheckLineOfSight(dist) ? EnemyState.Shooting : EnemyState.Moving;
    }
    
    private bool CheckLineOfSight(float distance)
    {
        Debug.DrawRay(transform.position, _playerTransform.position - transform.position, Color.magenta);

        var dir = _playerTransform.position - transform.position;
        return Physics.Raycast(transform.position, dir, out var hit, distance, rayMask) && hit.collider.CompareTag("Player");
    }
    
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        onEnemyHit.Raise();
        if (_currentHealth <= 0) Die();
    }

    public void Die()
    {
        _currentState = EnemyState.Dead;
        _animator.SetBool(Destroyed, true);
        onEnemyDied.Raise();
        StartCoroutine(CorDespawnTimer());
    }

    IEnumerator CorDespawnTimer()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(enemyParameters.despawnTime);
        Deactivate();
    }

    public void Activate(Vector3 position, Quaternion rotation)
    {
        Initialize();
        _enemyMovement.enabled = true;
        _enemyShoot.enabled = true;
        _collider.enabled = true;
        _meshRenderer.enabled = true;
        _currentHealth = enemyParameters.maxHealth;
        _animator.enabled = true;
        _currentState = EnemyState.Moving;
        _enemyMovement.WarpTo(position);
        _animator.SetBool(Destroyed, false);
    }

    public void Deactivate()
    {
        _enemyMovement.enabled = false;
        _enemyShoot.enabled = false;
        _collider.enabled = false;
        _meshRenderer.enabled = false;
        _animator.enabled = false;
        ParentPool.Release(this);
    }
    
    private void Prepare()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _collider = gameObject.GetComponent<BoxCollider>();
        _meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        _enemyShoot = GetComponent<EnemyShoot>();
        _enemyMovement = GetComponent<EnemyMovement>();
        var player = FindObjectOfType<Player>().gameObject;
        _animator = GetComponentInChildren<Animator>();
        _playerTransform = player.transform;    
        
        _enemyShoot.Initiate(enemyParameters, _playerTransform, this);
        _enemyMovement.Initiate(enemyParameters, _playerTransform, this);
        
        _currentHealth = enemyParameters.maxHealth;
        
    }
}
