using UnityEngine;

// Pool manager set up for consumption by MultiPoolsManager
public class EnemyVariantPooler : PoolManager<Enemy>
{
    public MultiPoolsManager MultiPoolsManager { get; set; }
    [SerializeField] private int poolSize = 8;
    protected override void Start()
    {
        base.Start();
        PoolSize = poolSize;
    }
    protected override void OnTakeFromPool(Enemy go)
    {
        go.gameObject.SetActive(true);
    }

    protected override void OnReturnedToPool(Enemy go)
    {
        go.gameObject.SetActive(false);
        MultiPoolsManager.RemoveEnemy(go);
        
    }

    protected override void OnDestroyPoolObject(Enemy go)
    {
        Destroy(go);
    }
    
    public Enemy GetEnemy()
    {
        return Pool.Get();
    }
}
