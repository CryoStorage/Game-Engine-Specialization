using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[SelectionBase]
public class Enemy : MonoBehaviour, ILightVulnerable
{
    public ObjectPool<Enemy> EnemyPool {get; set;}
    [HideInInspector] public bool IsSlowed;
    
    [Header("Enemy Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float damageCooldownTime = .5f;
    [SerializeField] private float attackCooldownTime = 1f;
    [SerializeField] private float attackRange = 1;
    
    
    [Header("GameVariables")]
    [SerializeField] BoolVariableSO isPlayerInLightHouse;
    
    [Header("GameEvents")]
    [SerializeField] protected GameEvent onEnemyDied;
    
    [Header("Serialized References")]
    [SerializeField]GameObject vfx;
    private GameObject _target;
    
    private NavMeshAgent _navAgent;
    private int _currentHealth;
    private bool _alive = true;
    private bool _canAttack = true;
    private bool _canTakeDamage = true;

    void Awake()
    {
        Prepare();
        _navAgent.speed = Random.Range(2,speed) ;
        if(isPlayerInLightHouse.value) TargetLightHouse();
        else TargetPlayer();
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        // Debug.Log($"{gameObject.name} | is alive: {_alive} | current health: {_currentHealth}");
        Attack();   
        if(!_alive) return;
        if (IsSlowed) _navAgent.speed = speed / 3;
        else _navAgent.speed = speed;
        _navAgent.SetDestination(_target.transform.position);
    }

    public void TakeDamage(int damage)
    {
        if(!_canTakeDamage) return;
        _currentHealth -= damage;
        IsSlowed = true;
        StartCoroutine(CorDamageCooldown());
        if(_currentHealth <= 0) Die();
    }
    
    private void Attack()
    {
        if(!_canAttack) return;
        if(Vector3.Distance(transform.position, _target.transform.position) > attackRange) return;
        _target.GetComponent<IAttackable>().TakeDamage(1);
        StartCoroutine(CorAttackCooldown());
    }
    
    private IEnumerator CorAttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(attackCooldownTime);
        _canAttack = true;
        IsSlowed = false;
    }
    
    private IEnumerator CorDamageCooldown()
    {
        _canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldownTime);
        _canTakeDamage = true;
        IsSlowed = false;
    }
    
    public virtual void Die()
    {
        onEnemyDied.Raise();
        _alive = false;
        vfx.SetActive(false);
        _navAgent.enabled = false;
        transform.position = new Vector3(0, -100, 0);
        EnemyPool.Release(this);
    }

    public void Respawn(Vector3 position)
    {
        _navAgent.speed = Random.Range(2, speed);
        _canAttack = true;
        IsSlowed = false;
        _canTakeDamage = true;
        _alive = true;
        vfx.SetActive(true);
        _currentHealth = maxHealth;
        _navAgent.enabled = true;
        _navAgent.Warp(position);
    }

    public void TargetPlayer()
    {
        _target = FindObjectOfType<Player>().gameObject;
        _navAgent.SetDestination(_target.transform.position);
        attackRange = 2;
    }
    
    public void TargetLightHouse()
    {
        _target = FindObjectOfType<LightHouse>().gameObject;
        _navAgent.SetDestination(_target.transform.position);
        attackRange = 5;
    }

    private void Prepare()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }
}
