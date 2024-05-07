using System;
using System.Collections.Generic;
using UnityEngine;
using CryoStorage;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemyMultiPoolsManager : MonoBehaviour
{
    [SerializeField] private List<EnemyVariantPooler> enemyPools;
    [SerializeField] private int startingEnemyCount;
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnRadius = 33.3f;
    [SerializeField] private int maxActiveEnemies;
    [Tooltip("Layer mask for checking if spawn point is valid. Should be set to Default, Ground & Enemies.")]
    [SerializeField] private LayerMask spawnLayerMask;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onWaveComplete;
    [SerializeField] private GameEvent onWaveStart;
    
    private float _spawnTimer;
    private ObjectPool<Enemy> _pools;
    private List<Enemy> _activeEnemies = new();
    
    private Transform _playerTransform;
    
    private int _defeatedEnemies;
    private int _waveTarget;
    private bool _waveComplete;
    private bool overrideStop;
    protected  void Start()
    {
        _waveTarget = startingEnemyCount;
        _playerTransform = FindObjectOfType<Player>().transform;
        foreach (var pooler in enemyPools)    
        {
            pooler.EnemyMultiPoolsManager = this;
        }
    }

    public void Stop()
    {
        overrideStop = true;
    }
    protected void Update()
    {
        if (_waveComplete) return;
        _spawnTimer += Time.deltaTime;
        if (!(_spawnTimer >= spawnRate)) return;
        _spawnTimer = 0;
        if (1 -_activeEnemies.Count + _defeatedEnemies < _waveTarget)
        {
            SpawnEnemy();
        }
    }
    
    public void RemoveEnemy(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
    }
    
    public void CountDefeatedEnemy()
    {
        _defeatedEnemies++;
        if (_defeatedEnemies < _waveTarget) return;
        onWaveComplete.Raise();
        // _waveComplete = true;
        Debug.Log("Wave Complete!");
    }

    private int IncrementWaveTarget()
    {
        var rand = Random.Range(1, 4);
        return _waveTarget + rand;
    }
    
    public void BeginWave()
    {
        _waveTarget = IncrementWaveTarget();
        Debug.Log("Begin Wave! Target: " + _waveTarget);
        _defeatedEnemies = 0;
        _waveComplete = false;
        onWaveStart.Raise();
    }

    private void SpawnEnemy()
    {
        if(overrideStop) return;
        var randomPool = enemyPools[Random.Range(0, enemyPools.Count)];
        var enemy = randomPool.GetEnemy();
        var spawnPoint = CryoMath.PointOnRadius(_playerTransform.position ,Random.Range(0,360), spawnRadius);
        while (CheckSpawnPointValid(spawnPoint))
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
}
