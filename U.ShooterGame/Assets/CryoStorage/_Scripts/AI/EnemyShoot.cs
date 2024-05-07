using UnityEngine;

public class EnemyShoot : PoolManager<Projectile>
{
    [SerializeField] private Transform bulletSpawnPoint;
    
    private EnemyParameters _enemyParameters;
    private IAimTarget _aimTarget;
    private float _shootTimer;
    private Enemy _parentEnemy;
    public void Initiate(EnemyParameters parameters, Transform target, Enemy parentEnemy)
    {
        _enemyParameters = parameters;
        _parentEnemy = parentEnemy;
        _aimTarget = target.GetComponent<IAimTarget>();
    }
    private void Update()
    {
        _shootTimer += Time.deltaTime;
        if(_parentEnemy.CurrentState == EnemyState.Shooting)
            Shoot();
    }

    protected override void OnTakeFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    protected override void OnReturnedToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    protected override void OnDestroyPoolObject(Projectile projectile)
    {
        Destroy(projectile);
    }
    
    private Vector3 PredictPosition(IAimTarget target, float projectileSpeed, float maxOffset)
    {
        // Predicted position with adjustment for projectile travel time
        Vector3 predictedPosition = target.Position + target.Velocity * (Vector3.Distance(transform.position, target.Position) / projectileSpeed);

        // Add randomness to predicted position within a certain range
        float randomOffsetX = Random.Range(-maxOffset, maxOffset);
        // float randomOffsetY = Random.Range(-maxOffset, maxOffset);
        float randomOffsetZ = Random.Range(-maxOffset, maxOffset);
        Vector3 randomOffset = new Vector3(randomOffsetX, 0, randomOffsetZ);
        predictedPosition += randomOffset;

        return predictedPosition;
    }
    
    private void Shoot()
    {
        if (_shootTimer < _enemyParameters.attackRate) return;


        var projectile = Pool.Get();
        Vector3 predictedPosition = PredictPosition(_aimTarget, projectile.BaseSpeed, _enemyParameters.shootingError);
        projectile.damage = _enemyParameters.damage;
        projectile.Activate(bulletSpawnPoint.position, Quaternion.LookRotation(predictedPosition - transform.position));
        _shootTimer = 0;
    }
   
}
