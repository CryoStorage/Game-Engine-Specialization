using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    
    [Header("Enemy Spawn Settings")]
    [SerializeField] private int startingEnemies = 3;
    [SerializeField] private int maxEnemies = 50;
    [Tooltip("The amount of enemies that will be added to the wave after each wave is completed")]
    [SerializeField] private int enemyIncrease = 3;
    [SerializeField] private float timeBetweenWaves = 30f;
    
    [Header("Enemy Spawn Settings")]
    [SerializeField] private GameObject [] spawnPoints;
    [SerializeField] private GameObject enemyStandbyPoint;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject tutorialEnemyPrefab;
    
    [Header("GameVariables")]
    [SerializeField] private IntVariableSo waveNumber;
    [SerializeField] private FloatVariableSo timeUntilNextWave;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onWaveComplete;
    [SerializeField] private GameEvent onBeginWave;

    private ObjectPool<Enemy> _enemyPool;
    private List<Enemy> _liveEnemies = new List<Enemy>();
    private int _currentEnemies;
    
    void Start()
    {
        _enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, maxEnemies, maxEnemies);
        _currentEnemies = startingEnemies;
    }

    public void CheckForWaveCompletion()
    {
        Debug.Log(_liveEnemies.Count);
        if (_liveEnemies.Count != 1) return;
        Debug.Log(_liveEnemies.Count);
        onWaveComplete.Raise();
        Debug.Log("Wave Complete");
        StartCoroutine(CorWaitForNextWave());
    }

    private void StartWave()
    {
        waveNumber.value ++;
        _currentEnemies += enemyIncrease;
        for (int i = 0; i < _currentEnemies; i++)
        {
            var enemy = _enemyPool.Get();
            enemy.Respawn(spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position);
        }
        onBeginWave.Raise();
    }
    
    public void SpawnTutorialEnemy()
    {
        var enemy = Instantiate(tutorialEnemyPrefab, enemyStandbyPoint.transform.position, Quaternion.identity).GetComponent<Enemy>();
        enemy.Respawn(spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position);
    }
    
    public void OverrideWave()
    {
        StopAllCoroutines();
        StartCoroutine(CorWaitForNextWave());
    }
    
    private IEnumerator CorWaitForNextWave()
    {
        timeUntilNextWave.value = timeBetweenWaves;
        while (timeUntilNextWave.value > 0)
        {
            timeUntilNextWave.value -= Time.deltaTime;
            yield return null;
        }

        StartWave();
    }
    
    private Enemy CreateEnemy()
    {
        var enemy = Instantiate(enemyPrefab, enemyStandbyPoint.transform.position, Quaternion.identity).GetComponent<Enemy>();
        enemy.EnemyPool = this._enemyPool;
        return enemy;
    }
    
    private void OnTakeFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.Respawn(spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position);
        _liveEnemies.Add(enemy);
    }
    
    private void OnReturnedToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        _liveEnemies.Remove(enemy);
        if (_liveEnemies.Count == 0)
        {
            onWaveComplete.Raise();
        }
    }
    
    private void OnDestroyPoolObject(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
    
}
