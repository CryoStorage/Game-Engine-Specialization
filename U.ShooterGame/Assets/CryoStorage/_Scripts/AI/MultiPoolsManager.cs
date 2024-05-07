
using System;
using System.Collections.Generic;
using UnityEngine;
using CryoStorage;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class MultiPoolsManager : MonoBehaviour
{
    [SerializeField] private List<EnemyVariantPooler> enemyPools;
    [SerializeField] private int startingEnemyCount;
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnRadius;
    [SerializeField] private int maxActiveEnemies;
    [Tooltip("Layer mask for checking if spawn point is valid. Should be set to Default, Ground & Enemies.")]
    [SerializeField] private LayerMask spawnLayerMask;
    
    private float _spawnTimer;
    private ObjectPool<Enemy> _pools;
    private List<Enemy> _activeEnemies = new();
    private Transform _playerTransform;
    protected  void Start()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
        foreach (var eP in enemyPools)    
        {
            eP.MultiPoolsManager = this;
        }
    }
    protected void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (!(_spawnTimer >= spawnRate)) return;
        _spawnTimer = 0;
        if (_activeEnemies.Count < maxActiveEnemies)
        {
            SpawnEnemy();
        }
    }
    
    public void RemoveEnemy(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
    }

    private void SpawnEnemy()
    {
        var randomPool = enemyPools[Random.Range(0, enemyPools.Count)];
        var enemy = randomPool.GetEnemy();
        var spawnPoint = CryoMath.PointOnRadius(_playerTransform.position ,Random.Range(0,360), spawnRadius);
        while (!CheckSpawnPointValid(spawnPoint))
        {
            spawnPoint = CryoMath.PointOnRadius(_playerTransform.position ,Random.Range(0,360), spawnRadius);
        }
        enemy.Activate(spawnPoint, Quaternion.identity);
        _activeEnemies.Add(enemy);
    }

    private bool CheckSpawnPointValid(Vector3 point)
    {
        return(Physics.CheckSphere(point, 1f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
    }
}
